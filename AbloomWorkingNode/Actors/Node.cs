using AbloomWorkingNode.Actors.RouterManager;
using AbloomWorkingNode.Actors.Processmanager;
using AbloomWorkingNode.Actors.ClusterManagr;
using Akka.Actor;
using AbloomWorkingNode.Messages;

namespace AbloomWorkingNode.Actors
{
    internal class Node : UntypedActor
    {
        private IActorRef ProcessmanagerRef { get; set; }
        private IActorRef RouterManagerRef { get; set; }
        private IActorRef ClusterManagerRef { get; set; }

        protected override void PreStart()
        {
            ProcessmanagerRef = Context.ActorOf<ProcessManager>("process-manager");
            RouterManagerRef = Context.ActorOf<CustomRouterManager>("router-manager");
            ClusterManagerRef = Context.ActorOf<ClusterManager>("cluster-manager");
        }

        protected override void OnReceive(object message)
        {

            switch (message)
            {
                case "Ready for checking":
                    Context.ActorSelection("*/task-router").Tell(message);
                    break;

                case SetPathRoutee:
                    RouterManagerRef.Forward(message);
                    break;

                case RemovePathRoutee:
                    RouterManagerRef.Forward(message);
                    break;

                case SendToWorkinNode:
                    ProcessmanagerRef.Forward(message);
                    break;
            }
        }
    }
}
