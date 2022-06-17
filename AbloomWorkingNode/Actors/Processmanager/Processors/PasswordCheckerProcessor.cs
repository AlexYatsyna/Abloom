using Abloom.Hashers;
using Abloom.Messages;
using Akka.Actor;
using Akka.Util.Internal;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbloomWorkingNode.Actors.Processmanager.Processors
{
    internal class PasswordCheckerProcessor : UntypedActor
    {
        private CustomPasswordHasher Hasher { get; set; }
        protected override void PreStart()
        {
            Hasher = new CustomPasswordHasher();
            Context.Parent.Tell("Ready for checking");
        }

        public RespondPassword StartCheck(SendToWorkinNode message)
        {
            foreach (var password in message.Passwords)
            {
                if (VerificatePassword(password, message.Hash))
                    return new RespondPassword(message.Id, true, message.Passwords.Count, password);
            }
            return new RespondPassword(message.Id, true, message.Passwords.Count, "");
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
                case SendToWorkinNode messag:
                    var result = StartCheck(messag);
                    Sender.Tell(result);
                    Console.WriteLine(Context.AsInstanceOf<ActorCell>().Mailbox.MessageQueue.Count);
                    break;
            }
        }
    }
}
