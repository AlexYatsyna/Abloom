using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Messages
{
    public sealed class SendToWorkinNode
    {
        public List<string> Passwords { get; }
        public string Hash { get; }
        public Guid Id { get; }

        public SendToWorkinNode(List<string> passwords, string correcthash, Guid id)
        {
            Passwords = passwords;
            Hash = correcthash;
            Id = id;
        }
    }
}
