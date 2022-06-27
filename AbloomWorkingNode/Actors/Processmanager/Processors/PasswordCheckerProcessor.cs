﻿using AbloomWorkingNode.Hashers;
using AbloomWorkingNode.Messages;
using Akka.Actor;
using System.Numerics;

namespace AbloomWorkingNode.Actors.Processmanager.Processors
{
    internal class PasswordCheckerProcessor : UntypedActor
    {
        private CustomPasswordHasher? Hasher { get; set; }
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
            if (Hasher!.VerifyHashedPassword(hash, password))
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
