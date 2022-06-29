using AbloomWorkingNode.Messages;
using Akka.Actor;
using Akka.Routing;
using System.Numerics;

namespace AbloomWorkingNode.Actors.Processmanager.Processors
{
    internal class PasswordCheckerBalancer : UntypedActor
    {
        private IActorRef? RouterRef { get; set; }
        private BigInteger Counter { get; set; } = 0;
        private BigInteger ProcessedPasswords { get; set; } = 0;
        private string? RespondPath { get; set; }
        private readonly int numberOfPasswords = 25;
        private Guid CurrentIntervalID { get; set; }

        protected override void PreStart()
        {
            RouterRef = Context.ActorOf(Props.Create<PasswordCheckerProcessor>().WithRouter
                (new RoundRobinPool(1, new DefaultResizer(2, Environment.ProcessorCount - 2, backoffThreshold: 0.4, backoffRate: 0.3, messagesPerResize: 3))), "check-router");
        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SendToWorkinNode data:

                    RespondPath = Sender.Path.Address + "/user/node/process-manager/password-processor";
                    Counter = data.Passwords.Count;
                    CurrentIntervalID = data.Id;
                    var numberOfIntervals = (int)Counter / numberOfPasswords;

                    for (int i = 0; i < numberOfIntervals; i++)
                    {
                        RouterRef.Tell(new SendToWorkinNode(data.Passwords.Skip(i * numberOfPasswords).Take(numberOfPasswords).ToList(), data.Hash, data.Id));
                    }
                    break;

                case RespondPassword data:

                    if (CurrentIntervalID != data.Id)
                        break;

                    Counter -= data.IntervalSize;
                    ProcessedPasswords += data.IntervalSize;

                    if (data.IsFound || Counter == 0)
                    {
                        Context.ActorSelection(RespondPath).Tell(new RespondPassword(data.Id, data.IsFound, ProcessedPasswords, data.Password));

                        ProcessedPasswords = 0;
                        Counter = 0;
                        CurrentIntervalID = Guid.Empty;

                        Context.Parent.Tell("Ready for checking");
                    }

                    break;
            }
        }
    }
}
