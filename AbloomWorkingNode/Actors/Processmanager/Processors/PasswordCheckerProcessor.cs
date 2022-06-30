using AbloomWorkingNode.Hashers;
using AbloomWorkingNode.Messages;
using Akka.Actor;
using System.Numerics;

namespace AbloomWorkingNode.Actors.Processmanager.Processors
{
    internal class PasswordCheckerProcessor : UntypedActor
    {
        private CustomPasswordHasher Hasher { get; } = new();
        private BigInteger Counter { get; set; } = 0;

        public RespondPassword StartCheck(SendToWorkinNode message)
        {
            foreach (var password in message.Passwords)
            {
                Counter++;
                if (Hasher!.VerifyHashedPassword(message.Hash, password))
                    return new RespondPassword(message.Id, true, Counter, password);
            }
            return new RespondPassword(message.Id, false, message.Passwords.Count, "");
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SendToWorkinNode data:
                    Counter = 0;
                    var result = StartCheck(data);
                    Sender.Tell(result);
                    break;
            }
        }
    }
}
