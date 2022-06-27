using AbloomWorkingNode.Actors;
using AbloomWorkingNode.Messages;
using AbloomWorkingNode.Serializers;
using Akka.Actor;
using Akka.Actor.Setup;
using Akka.Configuration;
using Akka.Serialization;
using System;
using System.Collections.Immutable;
using System.IO;

namespace AbloomWorkingNode
{
    internal class Program
    {
        public static SerializationSetup SerializationSettings =>
        SerializationSetup.Create(actorSystem =>
            ImmutableHashSet<SerializerDetails>.Empty.Add(
                SerializerDetails.Create("app-protocol",
                    new MySerializer(actorSystem),
                    ImmutableHashSet<Type>.Empty.Add(typeof(IAppProtocol)))));

        public static readonly BootstrapSetup Bootstrap = BootstrapSetup.Create().WithConfig(
            ConfigurationFactory.ParseString(File.ReadAllText(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent + "\\Configs\\App.conf")));

        public static readonly ActorSystemSetup ActorSystemSettings = ActorSystemSetup.Create(SerializationSettings, Bootstrap);

        static void Main(string[] args)
        {
            var system = ActorSystem.Create("msys", ActorSystemSettings);
            system.ActorOf<Node>("node");
            system.WhenTerminated.Wait();
            
        }
    }
    
}
