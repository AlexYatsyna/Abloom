using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Messages
{
    public sealed class SendToGeneratePasswords
    {
        public string CorrectHash { get; }
        public int estimatedPassword { get; }

        public SendToGeneratePasswords(string correctHash, int estimatedPassword)
        {
            CorrectHash = correctHash;
            this.estimatedPassword = estimatedPassword;
        }
    }
}
