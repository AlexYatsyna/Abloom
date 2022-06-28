using Abloom2.Messages;
using Akka.Actor;
using System.Numerics;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Abloom2.Actors.Processmanager.Processors
{
    internal class DisplayProcessor : UntypedActor
    {

        private Timer timer = new();
        private int PasswordLength { get; set; }
        private int NumberOfNodes { get; set; } = 0;
        private BigInteger NumberOfPassCombinations { get; set; }
        private BigInteger CurrentNumberOfComb { get; set; } = 0;

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SetInitialData data:
                    PasswordLength = data.PasswordLength;
                    break;

                case SetNumberOfCombinations data:
                    NumberOfPassCombinations = data.NumberOfCombinations;
                    Self.Tell("Display");
                    break;

                case SetCurrentCombination current:
                    CurrentNumberOfComb += current.CurrentCombination;
                    break;

                case "Display":

                    timer.Interval = 3000;
                    timer.Elapsed += DisplayInfo;
                    timer.AutoReset = true;
                    timer.Enabled = true;

                    Task.Run(() => GetHotkeysFromUser()).PipeTo(Context.Parent);
                    break;

                case RespondFinishExecution finished:
                    timer.Enabled = false;
                    Console.WriteLine($" Password:  {finished.Password} \n Combination number: {CurrentNumberOfComb}");

                    Context.Parent.Tell("End");
                    break;
            }
        }

        private void DisplayInfo(object source, ElapsedEventArgs e)
        {
            var percent = CurrentNumberOfComb * 100 / NumberOfPassCombinations;

            Console.WriteLine($"[INFO] [{DateTime.Now.ToLongTimeString()}] {percent} % , {CurrentNumberOfComb:N0} / {NumberOfPassCombinations:N0}(password length {PasswordLength})");
            Console.WriteLine($"Number of nodes in the cluster: {NumberOfNodes}\n");
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
