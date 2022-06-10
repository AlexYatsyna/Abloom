using Abloom.Actors;
using Abloom.ConnWPass;
using Abloom.Data;
using Abloom.UI;
using Akka.Actor;
using Akka.Configuration;
using System;
using System.IO;
using System.Linq;

namespace Abloom
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var test = new UserUIManager();
            //var a = new PasswordGenerator();
            //test.GetDataFromUser();
            //test.StartTimer();
            //a.StartCheck(CommonData.PasswordLength);

            var config = ConfigurationFactory.ParseString(File.ReadAllText("D:\\Abloom\\Abloom\\Configs\\App.conf"));
            var system = ActorSystem.Create("msys", config);
            system.ActorOf<Node>("node");

            system.WhenTerminated.Wait();
        }
    }
}
