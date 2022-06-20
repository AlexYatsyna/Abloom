﻿using Abloom.Messages;
using Akka.Actor;


namespace Abloom.Actors.ClusterMangr
{
    internal class ClusterManager : UntypedActor
    {
        private IActorRef ClusterListenerRef { get; set; }

        protected override void PreStart()
        {
            ClusterListenerRef = Context.ActorOf<ClusterListener>("cluster-listener");
        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SetPathRoutee:
                    Context.Parent.Forward(message);
                    break;

                case RemovePathRoutee:
                    Context.Parent.Forward(message);
                    break;
            }
        }
    }
}