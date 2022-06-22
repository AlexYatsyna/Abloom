using AbloomWorkingNode.Hashers;
using Akka.Actor;
using Microsoft.AspNet.Identity;
using AbloomWorkingNode.Messages;

namespace AbloomWorkingNode.Actors.Processmanager.Processors
{
    internal class PasswordCheckerProcessor : UntypedActor
    {
        private CustomPasswordHasher Hasher { get; set; }
        protected override void PreStart()
        {
            Hasher = new CustomPasswordHasher();
        }

        public RespondPassword StartCheck(SendToWorkinNode message)
        {
            foreach (var password in message.Passwords)
            {
                if (VerificatePassword(password, message.Hash))
                    return new RespondPassword(message.Id, true, message.Passwords.Count, password);
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
                    var result = StartCheck(data);
                    var respondPath = Sender.Path.Address + "/user/node/process-manager/password-processor";
                    Context.ActorSelection(respondPath).Tell(result);
                    Context.Parent.Tell("Ready for checking");
                    break;
            }
        }
    }
}
