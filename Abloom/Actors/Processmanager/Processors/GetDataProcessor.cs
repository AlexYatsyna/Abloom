using Abloom.Messages;
using Akka.Actor;

namespace Abloom.Actors.Processmanager.Processors
{
    internal class GetDataProcessor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "start":
                    Console.WriteLine("Enter password hash:");
                    var hash = Console.ReadLine();

                    Console.WriteLine("Enter password length:");
                    if (!int.TryParse(Console.ReadLine(), out int passLength))
                    {
                        Console.Clear();
                        Console.WriteLine("Incorrect password length!!!");

                        Self.Tell("start");
                        break;
                    }

                    Console.Clear();

                    var displayProcessor = Context.ActorSelection("../display-processor");
                    var sendRecieveProcessor = Context.ActorSelection("../password-processor");
                    var messag = new SetInitialData(hash!, passLength);

                    displayProcessor.Tell(messag);
                    sendRecieveProcessor.Tell(messag);

                    break;
            }
        }

    }
}
