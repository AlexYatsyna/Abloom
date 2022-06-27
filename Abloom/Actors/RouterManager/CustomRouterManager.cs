using Abloom2.Messages;
using Akka.Actor;
using Akka.Routing;

namespace Abloom2.Actors.RouterManager
{
    internal class CustomRouterManager : UntypedActor
    {
        private Dictionary<string, Routee> RouteeStorage = new Dictionary<string, Routee>();
        private IActorRef? RouterRef { get; set; }
        protected override void PreStart()
        {
            RouterRef = Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "router");
        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SetPathRoutee data:
                    RouteeStorage.Add(data.Path, data.ActorRoutee);
                    RouterRef.Tell(new AddRoutee(data.ActorRoutee));
                    if (RouteeStorage.Count == 1)
                    {
                        Thread.Sleep(100);
                    }
                    break;

                case RemovePathRoutee data:
                    if (RouteeStorage.ContainsKey(data.Path))
                    {
                        RouterRef.Tell(new RemoveRoutee(RouteeStorage[data.Path]));
                        RouteeStorage.Remove(data.Path);
                    }
                    break;
            }

        }
    }
}
