using Akka.Actor;
using System;
using System.Collections.Generic;

namespace AbloomWorkingNode.Messages
{
    public interface IAppProtocol { }
    public sealed class SendToWorkinNode : IAppProtocol
    {
        public List<string> Passwords { get; }
        public string Hash { get; }
        public Guid Id { get; }


        public SendToWorkinNode(List<string> passwords, string hash, Guid id)
        {
            Passwords = passwords;
            Hash = hash;
            Id = id;
        }
    }
}
