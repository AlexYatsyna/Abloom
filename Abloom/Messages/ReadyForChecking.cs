using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Messages
{
    public sealed class ReadyForChecking
    {
        public IActorRef ReplyTo { get; }
        public ReadyForChecking(IActorRef replyTo)
        {
            ReplyTo = replyTo;
        }
    }
}
