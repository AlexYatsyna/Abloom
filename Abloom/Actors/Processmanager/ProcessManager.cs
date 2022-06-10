using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Actors.Processmanager
{
    internal class ProcessManager : UntypedActor
    {
        private IActorRef GetDataRef { get; set; }
        private IActorRef DisplayInfoRef { get; set; }
        protected override void PreStart()
        {
            GetDataRef = Context.ActorOf<GetDataProcessor>("get-data-processor");
            DisplayInfoRef = Context.ActorOf<DisplayProcessor>("display-processor");
        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "data":
                    GetDataRef.Tell("Get data");
                    break;
            }
        }
    }
}
