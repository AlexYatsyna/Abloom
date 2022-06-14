using Abloom.Actors.ClusterManger;
using Abloom.Actors.Processmanager;
using Abloom.Messages;
using Akka.Actor;
using Akka.Routing;

namespace Abloom.Actors
{
    internal class Node : UntypedActor
    {
        private IActorRef ClusterManagerRef { get; set; }
        private IActorRef ProcessmanagerRef { get; set; }
        private IActorRef BalanceRouterRef { get; set; }

        protected override void PreStart()
        {
            ClusterManagerRef = Context.ActorOf<ClusterManager>("cluster-manager");
            ProcessmanagerRef = Context.ActorOf<ProcessManager>("process-manager");
            BalanceRouterRef = Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "balanceRouter");

        }
        protected override void OnReceive(object message)
        {

            switch (message)
            {
                case "start":
                    ProcessmanagerRef.Tell("data");
                    break;

                case SendToWorkinNode:
                    //var a = Context.ActorSelection("akka.tcp://msys@localhost:52597/user/process");
                    //Console.WriteLine(a);
                    //a.Tell(mes);

                    //var routee = BalanceRouterRef.Ask<Routees>(new GetRoutees()).ContinueWith(tr =>
                    //{
                    //    if (tr.Result.Members.Count() > 0)
                    //    {
                    //        Console.WriteLine(tr.Result.Members.Count());
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("no");
                    //    }
                    //});

                    BalanceRouterRef.Forward(message);
                    break;
                    
                    
            }
        }
    }
}
