using Abloom.Actors.ClusterManagr;
using Abloom.Actors.Processmanager;
using Abloom.Actors.RouterManager;
using Abloom.Messages;
using Akka.Actor;

namespace Abloom.Actors
{
    internal class Node : UntypedActor
    {
        private IActorRef? ClusterManagerRef { get; set; }
        private IActorRef? ProcessmanagerRef { get; set; }
        private IActorRef? RouterManagerRef { get; set; }

        protected override void PreStart()
        {
            ClusterManagerRef = Context.ActorOf<ClusterManager>("cluster-manager");
            ProcessmanagerRef = Context.ActorOf<ProcessManager>("process-manager");
            RouterManagerRef = Context.ActorOf<CustomRouterManager>("router-manager");
        }
        protected override void OnReceive(object message)
        {

            switch (message)
            {
                case "start":
                    ProcessmanagerRef.Tell("start");
                    break;

                case "End":
                    Context.Stop(Self);
                    Context.System.Terminate();
                    break;

                case "Ready for checking":
                    ProcessmanagerRef.Forward(message);
                    break;

                case SetPathRoutee:
                    RouterManagerRef.Forward(message);
                    break;

                case RemovePathRoutee:
                    RouterManagerRef.Forward(message);
                    break;

                case GetMembers:
                    ClusterManagerRef.Forward(message);
                    break;
            }
        }
    }
}
