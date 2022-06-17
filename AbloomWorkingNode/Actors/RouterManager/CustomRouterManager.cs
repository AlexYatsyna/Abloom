using Abloom.Messages;
using Akka.Actor;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbloomWorkingNode.Actors.RouterManager
{
    public class CustomRouterManager : UntypedActor
    {
        private IActorRef RouterRef { get; set; }
        private Dictionary<string, Routee> RouteeStorage = new Dictionary<string, Routee>();
        protected override void PreStart()
        {
            RouterRef = Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "task-router");
        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SetPathRoutee data:
                    RouteeStorage.Add(data.Path, data.ActorRoutee);
                    RouterRef.Tell(new AddRoutee(data.ActorRoutee));
                    break;

                case RemovePathRoutee data:
                    RouterRef.Tell(new RemoveRoutee(RouteeStorage[data.Path]));
                    RouteeStorage.Remove(data.Path);
                    break;

                case "Ready for checking":
                    RouterRef.Forward(message);
                    break;
            }

        }
    }
}
