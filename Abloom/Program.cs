using Abloom.Actors;
using Akka.Actor;
using Akka.Configuration;
using System.IO;


namespace Abloom
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("D:\\Abloom\\Abloom\\Configs\\App.conf"));
            var system = ActorSystem.Create("msys", config);
            var node = system.ActorOf<Node>("node");
            node.Tell("start");

            system.WhenTerminated.Wait();
        }
    }
}
