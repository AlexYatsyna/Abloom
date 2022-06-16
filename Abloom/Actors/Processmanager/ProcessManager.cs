using Abloom.Actors.Processmanager.Processors;
using Abloom.Actors.Processors;
using Abloom.Messages;
using Akka.Actor;

namespace Abloom.Actors.Processmanager
{
    internal class ProcessManager : UntypedActor
    {
        private IActorRef GetDataRef { get; set; }
        private IActorRef DisplayInfoRef { get; set; }
        private IActorRef PasswordGeneratorRef { get; set; }

        protected override void PreStart()
        {
            GetDataRef = Context.ActorOf<GetDataProcessor>("get-data-processor");
            DisplayInfoRef = Context.ActorOf<DisplayProcessor>("display-processor");
            PasswordGeneratorRef = Context.ActorOf<SendRecievePassProcessor>("password-processor");
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "data":
                    GetDataRef.Tell("Get data");
                    break;

                case SendToWorkinNode:
                    Context.Parent.Forward(message);
                    break;
            }
        }
    }
}
