using Abloom.Messages;
using Akka.Actor;
using Akka.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbloomWorkingNode.Actors
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
                case SendToWorkinNode :
                    PasswordCheckerRef.Forward(message);
                    break;
            }
            
        }
        
    }
}
