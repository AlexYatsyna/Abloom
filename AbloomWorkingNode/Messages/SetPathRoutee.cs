using Akka.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbloomWorkingNode.Messages
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
