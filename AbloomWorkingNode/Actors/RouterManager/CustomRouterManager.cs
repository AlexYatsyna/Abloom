using AbloomWorkingNode.Messages;
using Akka.Actor;
using Akka.Routing;

namespace AbloomWorkingNode.Actors.RouterManager
{
    public class CustomRouterManager : UntypedActor
    {
        private IActorRef? RouterRef { get; set; }
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

                    if (RouteeStorage.Count == 1)
                    {
                        Thread.Sleep(100);
                        Context.Parent.Tell("Ready for checking");
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
