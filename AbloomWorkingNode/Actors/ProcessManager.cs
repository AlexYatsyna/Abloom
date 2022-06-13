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
        private Serialization serialization = Context.System.Serialization;
        protected override void PreStart()
        {
            Console.WriteLine(Self);
        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SendToWorkinNode mes:
                    Console.WriteLine(Sender);
                    break;
            }
            
            //throw new NotImplementedException();
        }
        
    }
}
