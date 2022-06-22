using AbloomWorkingNode.Actors.Processmanager.Processors;
using AbloomWorkingNode.Messages;
using Akka.Actor;

namespace AbloomWorkingNode.Actors.Processmanager
{
    internal class ProcessManager : UntypedActor
    {
        private IActorRef PasswordCheckerRef { get; set; }
        protected override void PreStart()
        {
            PasswordCheckerRef = Context.ActorOf<PasswordCheckerProcessor>("password-checker");
        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "Ready for checking":
                    Context.Parent.Forward(message);
                    break;

                case SendToWorkinNode:
                    PasswordCheckerRef.Forward(message);
                    break;
            }

        }

    }
}
