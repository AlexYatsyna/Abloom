using Akka.Actor;


namespace Abloom.Actors.ClusterManger
{
    internal class ClusterManager : UntypedActor
    {
        private IActorRef ClusterListenerRef { get; set; }

        protected override void PreStart()
        {
            ClusterListenerRef = Context.ActorOf<ClusterListener>("cluster-listener");
        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {

            }
        }
    }
}
