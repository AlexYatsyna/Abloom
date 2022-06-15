using Abloom.Actors.Processmanager.Processors;
using Abloom.Actors.Processors;
using Abloom.Messages;
using Akka.Actor;
using System.Timers;

namespace Abloom.Actors.Processmanager
{
    internal class ProcessManager : UntypedActor
    {
        private IActorRef GetDataRef { get; set; }
        private IActorRef DisplayInfoRef { get; set; }
        private IActorRef PasswordGeneratorRef { get; set; }

        private Timer timer = new Timer();
        protected override void PreStart()
        {
            GetDataRef = Context.ActorOf<GetDataProcessor>("get-data-processor");
            DisplayInfoRef = Context.ActorOf<DisplayProcessor>("display-processor");
            PasswordGeneratorRef = Context.ActorOf<PasswordGeneratorProcessor>("password-processor");

            timer.Interval = 10;
            timer.AutoReset = true;
            timer.Elapsed += RequestSendingPasswords;
        }

        private void RequestSendingPasswords(object source, ElapsedEventArgs e)
        {
            PasswordGeneratorRef.Tell("Send");
        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "data":
                    GetDataRef.Tell("Get data");
                    break;

                case "StartSending":
                    timer.Enabled = true;
                    break;

                case "StopSending":
                    timer.Enabled = false;
                    break;

                case SendToWorkinNode:
                    Context.Parent.Forward(message);
                    break;
            }
        }
    }
}
