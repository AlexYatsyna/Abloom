using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Actors.Processors
{
    internal class GetDataProcessor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "Get data":
                    GetData();
                    break;
            }
        }

        private void GetData()
        {
            Console.WriteLine("Enter password hash:");
            var hash = Console.ReadLine();

            Console.WriteLine("Enter password length:");
            var length = Convert.ToInt32(Console.ReadLine());
        }
    }
}
