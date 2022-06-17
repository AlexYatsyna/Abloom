using Abloom.Messages;
using AbloomWorkingNode.Actors.Processmanager;
using Akka.Actor;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbloomWorkingNode.Actors
{
    internal class Node : UntypedActor
    {
        private IActorRef ProcessmanagerRef { get; set; }
        private IActorRef RouterRef { get; set; }

        protected override void PreStart()
        {
            ProcessmanagerRef = Context.ActorOf<ProcessManager>("process-manager");
            RouterRef = Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "task-router");
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
                case SendToWorkinNode:
                    ProcessmanagerRef.Forward(message);
                    break;
                case "Ready for checking":
                    RouterRef.Forward(message);
                    break;

            }
        }
    }
}
