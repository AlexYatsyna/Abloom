using Akka.Actor;
using System;
using System.Collections.Generic;

namespace Abloom.Messages
{
    public sealed class SendToWorkinNode
    {
        public List<string> Passwords { get; }
        public string Hash { get; }
        public Guid Id { get; }
        public IActorRef ReplyTo { get; }

        public SendToWorkinNode(List<string> passwords, string correcthash, Guid id, IActorRef replyTo)
        {
            Passwords = passwords;
            Hash = correcthash;
            Id = id;
            ReplyTo = replyTo;
        }
    }
}
