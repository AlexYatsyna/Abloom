using Abloom.Messages;
using Abloom2.Messages;
using Akka.Actor;
using Akka.Cluster;

namespace Abloom2.Actors.ClusterManagr
{
    internal class ClusterManager : UntypedActor
    {
        private IActorRef? ClusterListenerRef { get; set; }
        private Cluster? Cluster { get; set; }

        protected override void PreStart()
        {
            Cluster = Cluster.Get(Context.System);
            ClusterListenerRef = Context.ActorOf<ClusterListener>("cluster-listener");
        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SetPathRoutee:
                    Context.Parent.Forward(message);
                    break;

                case RemovePathRoutee:
                    Context.Parent.Forward(message);
                    break;

                case GetMembers data:
                    var members = Cluster?.State.Members.Where(member => member.HasRole(data.Role)).ToList();
                    Sender.Tell(new RespondMembers(members));
                    break;
            }
        }
    }
}
