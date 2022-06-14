using Abloom.Hashers;
using Abloom.Messages;
using Akka.Actor;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;

namespace AbloomWorkingNode.Actors
{
    internal class PasswordCheckerProcessor : UntypedActor
    {
        private CustomPasswordHasher Hasher { get; set; } 
        private bool isCompleted = false;
        protected override void PreStart()
        {
            Hasher = new CustomPasswordHasher();
        }

        public string StartCheck(List<string> passwords, string hash)
        {
            foreach (var password in passwords)
            {
                if (isCompleted)
                    break;
                if (VerificatePassword(password, hash))
                    return password;

            }
            return "";
        }

        private bool VerificatePassword(string password, string hash)
        {
            if (Hasher.VerifyHashedPassword(hash, password) == PasswordVerificationResult.Success)
            {
                return true;
            }
            return false;
        }

        private void ProcessMessage(SendToWorkinNode message)
        {
            var result = StartCheck(message.Passwords, message.Hash);
            Console.WriteLine(result);
            if (result.Length != 0)
            {
                Sender.Tell(new RespondPassword(message.Id, true, message.Passwords.Count, result));
                return;
            }
            Sender.Tell(new RespondPassword(message.Id, true, message.Passwords.Count));
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SendToWorkinNode mess:
                    ProcessMessage(mess);
                    break;
            }
        }
    }
}
