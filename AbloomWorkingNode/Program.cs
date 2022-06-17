using AbloomWorkingNode.Actors;
using AbloomWorkingNode.Actors.Processmanager;
using Akka.Actor;
using Akka.Configuration;
using System;
using System.IO;

namespace AbloomWorkingNode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("D:\\Abloom\\AbloomWorkingNode\\Configs\\App.conf"));
            var system = ActorSystem.Create("msys", config);
            system.ActorOf<Node>("node");
            system.WhenTerminated.Wait();
        }
    }
}
