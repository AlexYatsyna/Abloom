using AbloomWorkingNode.Messages;
using Akka.Actor;
using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AbloomWorkingNode.Actors.Processmanager.Processors
{
    internal class PasswordCheckerBalancer : UntypedActor
    {
        private IActorRef RouterRef { get; set; }
        private BigInteger Counter { get; set; } = 0;
        private BigInteger ProcessedPasswords { get; set; } = 0;
        private string RespondPath { get; set; }
        private int numberOfPasswordsInInterval = 50;

        protected override void PreStart()
        {
            RouterRef = Context.ActorOf(Props.Create<PasswordCheckerProcessor>().WithRouter
                (/*FromConfig.Instance*/new RoundRobinPool(1, new DefaultResizer(2, 5, 1, 0.2, 0.5, 0.3, 5))), "check-router");
        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SendToWorkinNode data:

                    RespondPath = Sender.Path.Address + "/user/node/process-manager/password-processor";
                    Counter = data.Passwords.Count;
                    var numberOfIntervals = (int)Counter / numberOfPasswordsInInterval;

                    for (int i = 0; i < numberOfIntervals; i++)
                    {
                        RouterRef.Tell(new SendToWorkinNode(data.Passwords.Skip(i * numberOfPasswordsInInterval).Take(numberOfPasswordsInInterval).ToList(), data.Hash, data.Id));
                    }
                    //var routee = RouterRef.Ask<Routees>(new GetRoutees()).ContinueWith(tr =>
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
                    break;

                case RespondPassword data:
                    Counter -= data.IntervalSize;
                    ProcessedPasswords += data.IntervalSize;
                    //routee = RouterRef.Ask<Routees>(new GetRoutees()).ContinueWith(tr =>
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
                    if (data.IsFound || Counter == 0)
                    {
                        Context.ActorSelection(RespondPath).Tell(new RespondPassword(data.Id, data.IsFound, ProcessedPasswords, data.Password));

                        ProcessedPasswords = 0;
                        Counter = 0;

                        Context.Parent.Tell("Ready for checking");
                    }
                    break;
            }
        }
    }
}
