using Akka.Routing;

namespace Abloom.Messages
{
    public sealed class SetPathRoutee
    {
        public Routee ActorRoutee { get; }
        public string Path { get; }

        public SetPathRoutee(Routee actorRoutee, string path)
        {
            ActorRoutee = actorRoutee;
            Path = path;
        }
    }
}
