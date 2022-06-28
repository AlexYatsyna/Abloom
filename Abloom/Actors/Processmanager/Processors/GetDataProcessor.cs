using Abloom2.Messages;
using Akka.Actor;

namespace Abloom2.Actors.Processmanager.Processors
{
    internal class GetDataProcessor : UntypedActor
    {
        private string? Hash { get; set; }
        private int PassLength { get; set; }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "start":
                    GetData();

                    var displayProcessor = Context.ActorSelection("../display-processor");
                    var sendRecieveProcessor = Context.ActorSelection("../password-processor");

                    displayProcessor.Tell(new SetInitialData(Hash!, PassLength));
                    sendRecieveProcessor.Tell(new SetInitialData(Hash!, PassLength));

                    break;
            }
        }

        private void GetData()
        {
            Console.WriteLine("Enter password hash:");
            Hash = Console.ReadLine();

            Console.WriteLine("Enter password length:");
            PassLength = Convert.ToInt32(Console.ReadLine());
            Console.Clear();
        }
    }
}
