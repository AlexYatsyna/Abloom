using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Actors
{
    internal class Node : UntypedActor
    {
        private IActorRef ClusterManagerRef { get; set; }
        private IActorRef ProcessmanagerRef { get; set; }

        protected override void PreStart()
        {
            ClusterManagerRef = Context.ActorOf<ClusterManager>("cluster-manager");
            ProcessmanagerRef = Context.ActorOf<ProcessManager>("process-manager");

        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "start":
                    ProcessmanagerRef.Tell("data");
                    break;
            }
        }
    }
}
