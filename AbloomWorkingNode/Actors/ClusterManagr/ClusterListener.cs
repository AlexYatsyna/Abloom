using AbloomWorkingNode.Messages;
using Akka.Actor;
using Akka.Cluster;
using Akka.Event;
using Akka.Routing;
using System;

namespace AbloomWorkingNode.Actors.ClusterManagr
{
    public class ClusterListener : UntypedActor
    {
        private ILoggingAdapter Log { get; } = Context.GetLogger();
        private Cluster cluster = Cluster.Get(Context.System);

        protected override void PreStart()
        {
            cluster.Subscribe(Self, ClusterEvent.InitialStateAsEvents, typeof(ClusterEvent.IMemberEvent));
        }
        protected override void PostStop()
        {
            cluster.Unsubscribe(Self);
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case ClusterEvent.MemberUp member:
                    //Log.Info("\n\nMember is up: {0}\n\n", member.Member);
                    if (member.Member.HasRole("managing-node"))
                    {
                        var path = member.Member.Address + "/user/node";
                        var actorRef = Context.ActorSelection(path).ResolveOne(TimeSpan.FromSeconds(2)).Result;
                        var actorRoutee = Routee.FromActorRef(actorRef);
                        Context.Parent.Tell(new SetPathRoutee(actorRoutee, path));

                        Console.WriteLine("Member is up: {0}\n", member.Member);
                    }
                    break;

                case ClusterEvent.UnreachableMember member:
                    //Log.Info("\n\nMember is unreachable: {0}\n\n", member.Member);
                    if (member.Member.HasRole("managing-node"))
                        Console.WriteLine("Member is unreachable: {0}\n", member.Member);
                    break;

                case ClusterEvent.MemberRemoved member:
                    //Log.Info("\n\nMember is removed: {0}\n\n", member.Member);
                    if (member.Member.HasRole("managing-node"))
                    {
                        var path = member.Member.Address + "/user/node";
                        Context.Parent.Tell(new RemovePathRoutee(path));

                        Console.WriteLine("Member is removed: {0}\n", member.Member);
                    }
                    break;
            }
        }
    }
}
