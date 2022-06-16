using Abloom.Actors.ClusterManger;
using Abloom.Actors.Processmanager;
using Abloom.Messages;
using Akka.Actor;
using Akka.Routing;
using System;
using System.Linq;

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
            //BalanceRouterRef.Tell(new AddRoutee(new Routee()))
            //var routee = BalanceRouterRef.Ask<Routees>(new GetRoutees()).ContinueWith(tr =>{
            //    if (tr.Result.Members.Count() > 0)
            //    {
            //        foreach(var member in tr.Result.Members)
            //        {
            //            Console.WriteLine(((ActorSelectionRoutee)member).Selection.PathString);
            //            Console.WriteLine(((ActorSelectionRoutee)member).Selection.Anchor.Path);
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("no");
            //    }
            //});



        }
        protected override void OnReceive(object message)
        {

            switch (message)
            {
                case "start":
                    ProcessmanagerRef.Tell("data");
                    break;

                case SendToWorkinNode:
                    BalanceRouterRef.Forward(message);
                    break;
                    
                    
            }
        }
    }
}
