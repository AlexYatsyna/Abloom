using Abloom.Messages;
using AbloomWorkingNode.Actors.Processmanager.Processors;
using Akka.Actor;
using Akka.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                case SendToWorkinNode:
                    PasswordCheckerRef.Forward(message);
                    break;
                case "Ready for checking":
                    Context.Parent.Forward(message);
                    break;
            }

        }

    }
}
