﻿using Abloom.Actors.Processmanager.Processors;
using Abloom.Actors.Processors;
using Abloom.Messages;
using Akka.Actor;

namespace Abloom.Actors.Processmanager
{
    internal class ProcessManager : UntypedActor
    {
        private IActorRef GetDataRef { get; set; }
        private IActorRef DisplayInfoRef { get; set; }
        private IActorRef SendRecievePassRef { get; set; }

        protected override void PreStart()
        {
            GetDataRef = Context.ActorOf<GetDataProcessor>("get-data-processor");
            DisplayInfoRef = Context.ActorOf<DisplayProcessor>("display-processor");
            SendRecievePassRef = Context.ActorOf<SendReceivePassProcessor>("password-processor");
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case ReadyForChecking:
                    SendRecievePassRef.Forward(message);
                    break;

                case "End":
                    Context.Parent.Forward(message);
                    break;
            }
        }
    }
}
