using Abloom.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Timers;

namespace Abloom.UI
{
    public class UserUIManager
    {
        private Timer timer;

        public UserUIManager()
        {
            timer = new Timer();
            timer.Interval = 3000;
            timer.Elapsed += DisplayInfo;
            timer.AutoReset = true;

        }

        public void GetDataFromUser()
        {
            Console.WriteLine("Enter password hash:");
            CommonData.InputHash = Console.ReadLine();

            Console.WriteLine("Enter password length:");
            CommonData.PasswordLength = Convert.ToInt32(Console.ReadLine());
        }
        private void DisplayInfo(object source, ElapsedEventArgs e)
        {
            Console.Clear();

            var percent = CommonData.CurrentNumberOfComb * 100 / CommonData.NumberOfPassCombinations;

            Console.WriteLine("\n\t\t\tPress 'x' to exit");
            Console.WriteLine($"\n\n{percent} % , {CommonData.CurrentNumberOfComb - 1:N0} / {CommonData.NumberOfPassCombinations:N0} combinations for password length {CommonData.PasswordLength} \n\n");
            GetHotkeysFromUser();
        }

        private void GetHotkeysFromUser()
        {
            var exit = Console.ReadKey();
            CommonData.isExit = exit.KeyChar == 'x';
        }

        public void StartTimer()
        {
            timer.Enabled = true;
        }
    }
}
