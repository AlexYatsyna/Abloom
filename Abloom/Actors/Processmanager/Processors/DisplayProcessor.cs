using Abloom.Messages;
using Akka.Actor;
using System;
using System.Numerics;
using System.Timers;

namespace Abloom.Actors.Processors
{
    internal class DisplayProcessor : UntypedActor
    {

        private Timer timer;
        private int PasswordLength { get; set; }
        private BigInteger NumberOfPassCombinations { get; set; }
        private BigInteger CurrentNumberOfComb { get; set; } = 1;


        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SetInitialData data:
                    NumberOfPassCombinations = data.NumberOfPassCombinations;
                    PasswordLength = data.PasswordLength;
                    break;

                case SetCurrentCombination current:
                    CurrentNumberOfComb += current.CurrentCombination;
                    break;

                case "Display":
                    timer = new Timer();
                    timer.Interval = 3000;
                    timer.Elapsed += DisplayInfo;
                    timer.AutoReset = true;
                    timer.Enabled = true;
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
            Console.Clear();

            var percent = CurrentNumberOfComb * 100 / NumberOfPassCombinations;

            Console.WriteLine("\n\t\t\tPress 'x' to exit");
            Console.WriteLine($"\n\n{percent} % , {CurrentNumberOfComb:N0} / {NumberOfPassCombinations:N0} combinations for for password length {PasswordLength} \n\n");
            GetHotkeysFromUser();
        }

        private void GetHotkeysFromUser()
        {
            var exit = Console.ReadKey();
            // CommonData.isExit = exit.KeyChar == 'x';
        }
    }
}
