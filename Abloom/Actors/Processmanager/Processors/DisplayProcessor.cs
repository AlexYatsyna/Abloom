using Abloom.Messages;
using Akka.Actor;
using Akka.Cluster;
using System.Numerics;

namespace Abloom.Actors.Processmanager.Processors
{
    internal class DisplayProcessor : UntypedActor
    {
        private int PasswordLength { get; set; }
        private List<Member>? WorkerNodes { get; set; }
        private BigInteger NumberOfPassCombinations { get; set; }
        private BigInteger CurrentNumberOfComb { get; set; } = 0;
        private ICancelable? Scheduler { get; set; }


        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "Display":
                    var percent = CurrentNumberOfComb * 100 / NumberOfPassCombinations;
                    Context.Parent.Tell(new GetMembers("working-node"));

                    Console.WriteLine($"[INFO] [{DateTime.Now.ToLongTimeString()}] {percent} % ," +
                        $" {CurrentNumberOfComb:N0} / {NumberOfPassCombinations:N0}(password length {PasswordLength})");
                    Console.WriteLine($"Number of nodes in the cluster: {WorkerNodes?.Count}\n");

                    break;

                case SetInitialData data:
                    PasswordLength = data.PasswordLength;
                    Context.Parent.Tell(new GetMembers("working-node"));
                    Task.Run(() => GetHotkeysFromUser()).PipeTo(Context.Parent);
                    break;

                case SetNumberOfCombinations data:
                    NumberOfPassCombinations = data.NumberOfCombinations;

                    Scheduler = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(
                        TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(3),
                        Self, "Display", ActorRefs.NoSender);
                    break;

                case SetCurrentCombination current:
                    CurrentNumberOfComb += current.CurrentCombination;
                    break;

                case RespondFinishExecution finished:
                    Scheduler?.Cancel();
                    Console.WriteLine($" Password:  {finished.Password} \n Combination number: {CurrentNumberOfComb}");

                    Context.Parent.Tell("End");
                    break;

                case RespondMembers data:
                    WorkerNodes = data.Members;
                    break;
            }
        }

        private string GetHotkeysFromUser()
        {
            var exit = Console.ReadKey();
            if (exit.KeyChar == 'x')
                return "End";
            return "";
        }
    }
}
