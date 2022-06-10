﻿using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Actors
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
            //throw new NotImplementedException();
        }
    }
}