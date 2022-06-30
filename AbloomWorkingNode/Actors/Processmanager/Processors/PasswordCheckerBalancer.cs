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
        private readonly int numberOfParts = 15;
        private Guid CurrentID { get; set; }

        protected override void PreStart()
        {
            RouterRef = Context.ActorOf(Props.Create<PasswordCheckerProcessor>().WithRouter
                (new RoundRobinPool(1, new DefaultResizer(3, Environment.ProcessorCount - 2, backoffThreshold: 0.4, backoffRate: 0.3, messagesPerResize: 3))), "check-router");
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 10,
                withinTimeRange: TimeSpan.FromMinutes(1),
                localOnlyDecider: ex =>
                {
                    Context.ActorSelection(RespondPath).Tell(ex);
                    return Directive.Resume;
                }
            );
        }

        public IEnumerable<IEnumerable<T>> Split<T>(IEnumerable<T> source, int count)
        {
            return source.Select((x, y) => new { Index = y, Value = x })
                .GroupBy(x => x.Index % count)
                .Select(x => x.Select(y => y.Value)
                .ToList()).ToList();
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SendToWorkinNode data:

                    RespondPath = Sender.Path.Address + "/user/node/process-manager/password-processor";
                    Counter = data.Passwords.Count;
                    CurrentID = data.Id;

                    var parts = Split<string>(data.Passwords, numberOfParts);
                    parts.ToList().ForEach(item => { RouterRef.Tell(new SendToWorkinNode(item.ToList(), data.Hash, data.Id)); });
                    break;

                case RespondPassword data:

                    if (CurrentID != data.Id)
                        break;

                    Counter -= data.IntervalSize;
                    ProcessedPasswords += data.IntervalSize;

                    if (data.IsFound || Counter == 0)
                    {
                        Context.ActorSelection(RespondPath).Tell(new RespondPassword(data.Id, data.IsFound, ProcessedPasswords, data.Password));

                        ProcessedPasswords = 0;
                        Counter = 0;
                        CurrentID = Guid.Empty;

                        Context.Parent.Tell("Ready for checking");
                    }

                    break;
            }
        }
    }
}
