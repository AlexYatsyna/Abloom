using Abloom.Actors.ClusterMangr;
using Abloom.Actors.Processmanager;
using Abloom.Actors.RouterManager;
using Abloom.Messages;
using Akka.Actor;
using Akka.Routing;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Abloom.Actors
{
    internal class Node : UntypedActor
    {
        private IActorRef ClusterManagerRef { get; set; }
        private IActorRef ProcessmanagerRef { get; set; }
        private IActorRef RouterManagerRef { get; set; }

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
                    ProcessmanagerRef.Tell("data");
                    break;

                case "End":
                    Context.Stop(Self);
                    break;

                case ReadyForChecking:
                    ProcessmanagerRef.Forward(message);
                    break;

                case SetPathRoutee:
                    RouterManagerRef.Forward(message);
                    break;

                case RemovePathRoutee:
                    RouterManagerRef.Forward(message);
                    break;
            }
        }
    }
}
