using AbloomWorkingNode.Actors.Processmanager.Processors;
using AbloomWorkingNode.Messages;
using Akka.Actor;

namespace AbloomWorkingNode.Actors.Processmanager
{
    internal class ProcessManager : UntypedActor
    {
        private IActorRef PasswordBalancerRef { get; set; }
        protected override void PreStart()
        {
            PasswordBalancerRef = Context.ActorOf<PasswordCheckerBalancer>("password-balancer");
        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "Ready for checking":
                    Context.Parent.Forward(message);
                    break;

                case SendToWorkinNode:
                    PasswordBalancerRef.Forward(message);
                    break;
            }

        }

    }
}
