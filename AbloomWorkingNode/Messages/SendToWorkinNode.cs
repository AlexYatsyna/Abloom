using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbloomWorkingNode.Messages
{
    public interface IAppProtocol { }
    public sealed class SendToWorkinNode
    {
        public Guid Id { get; }
        public List<string> Passwords {get;}
        public string Hash { get; }
        public SendToWorkinNode(List<string> passwords, string hash, Guid id)
        {
            Id = id;
            Passwords = passwords;
            Hash = hash;
        }
    }
}
