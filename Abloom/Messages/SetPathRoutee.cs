using Akka.Routing;

namespace Abloom2.Messages
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
