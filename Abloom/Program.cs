using Abloom2.Actors;
using AbloomWorkingNode.Messages;
using AbloomWorkingNode.Serializers;
using Akka.Actor;
using Akka.Actor.Setup;
using Akka.Configuration;
using Akka.Serialization;
using System.Collections.Immutable;


SerializationSetup GetSerializationSettings() =>
    SerializationSetup.Create(actorSystem =>
        ImmutableHashSet<SerializerDetails>.Empty.Add(
            SerializerDetails.Create("app-protocol",
                new MySerializer(actorSystem),
                ImmutableHashSet<Type>.Empty.Add(typeof(IAppProtocol)))));


var Bootstrap = BootstrapSetup.Create().WithConfig(
            ConfigurationFactory.ParseString(
                File.ReadAllText($"{Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent}\\App.conf")));

var ActorSystemSettings = ActorSystemSetup.Create(GetSerializationSettings(), Bootstrap);

var system = ActorSystem.Create("msys", ActorSystemSettings);
var node = system.ActorOf<Node>("node");
node.Tell("start");

system.WhenTerminated.Wait();

