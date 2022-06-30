using Abloom.Messages;
using Akka.Actor;
using Akka.Cluster;
using Akka.Event;
using Akka.Routing;


namespace Abloom.Actors.ClusterManagr
{
    internal class ClusterListener : UntypedActor
    {
        private ILoggingAdapter Log { get; } = Context.GetLogger();
        private Cluster Cluster { get; } = Cluster.Get(Context.System);

        protected override void PreStart()
        {
            Cluster.Subscribe(Self, ClusterEvent.InitialStateAsEvents, typeof(ClusterEvent.IMemberEvent));
        }
        protected override void PostStop()
        {
            Cluster.Unsubscribe(Self);
        }


        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case ClusterEvent.MemberUp member:
                    Log.Info("\nMember is up: {0}\n", member.Member);
                    if (member.Member.HasRole("working-node"))
                    {
                        var path = member.Member.Address + "/user/node";
                        var actorRef = Context.ActorSelection(path).ResolveOne(TimeSpan.FromSeconds(2)).Result;
                        var actorRoutee = Routee.FromActorRef(actorRef);
                        Context.Parent.Tell(new SetPathRoutee(actorRoutee, path));
                    }
                    break;

                case ClusterEvent.UnreachableMember member:
                    Log.Info("\nMember is unreachable: {0}\n", member.Member);
                    break;

                case ClusterEvent.MemberRemoved member:
                    Log.Info("\nMember is removed: {0}\n", member.Member);
                    if (member.Member.HasRole("working-node"))
                    {
                        var path = member.Member.Address + "/user/node";
                        Context.Parent.Tell(new RemovePathRoutee(path));
                    }
                    break;
            }
        }
    }
}
