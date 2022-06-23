using AbloomWorkingNode.Hashers;
using Akka.Actor;
using Microsoft.AspNet.Identity;
using AbloomWorkingNode.Messages;
using System;
using System.Numerics;

namespace AbloomWorkingNode.Actors.Processmanager.Processors
{
    internal class PasswordCheckerProcessor : UntypedActor
    {
        private CustomPasswordHasher Hasher { get; set; }
        private BigInteger Counter { get; set; } = 0;
        protected override void PreStart()
        {
            Hasher = new CustomPasswordHasher();
        }

        public RespondPassword StartCheck(SendToWorkinNode message)
        {
            foreach (var password in message.Passwords)
            {
                Counter++;
                if (VerificatePassword(password, message.Hash))
                    return new RespondPassword(message.Id, true, Counter, password);
            }
            return new RespondPassword(message.Id, false, message.Passwords.Count, "");
        }

        private bool VerificatePassword(string password, string hash)
        {
            if (Hasher.VerifyHashedPassword(hash, password) == PasswordVerificationResult.Success)
            {
                return true;
            }
            return false;
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
