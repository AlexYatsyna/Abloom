using Akka.Actor;
using Akka.Cluster;
using Akka.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Abloom.Actors
{
    internal class ClusterListener : UntypedActor
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
                    if(member.Member.Roles.Contains("working-node"))
                        Console.WriteLine("Member is up: {0}\n", member.Member);
                    break;
                case ClusterEvent.UnreachableMember member:
                    //Log.Info("\n\nMember is unreachable: {0}\n\n", member.Member);
                    if (member.Member.Roles.Contains("working-node"))
                        Console.WriteLine("Member is unreachable: {0}\n", member.Member);
                    break;
                case ClusterEvent.MemberRemoved member:
                    //Log.Info("\n\nMember is removed: {0}\n\n", member.Member);
                    if (member.Member.Roles.Contains("working-node"))
                        Console.WriteLine("Member is removed: {0}\n", member.Member);
                    break;
            }
        }
    }
}
