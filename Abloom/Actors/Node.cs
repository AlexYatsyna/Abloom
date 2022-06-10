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

        protected override void PreStart()
        {
            ClusterManagerRef = Context.ActorOf<ClusterManager>("cluster-manager");
        }
        protected override void OnReceive(object message)
        {
            //throw new NotImplementedException();
        }
    }
}
