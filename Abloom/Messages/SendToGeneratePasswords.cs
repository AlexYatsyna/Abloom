
namespace Abloom.Messages
{
    public sealed class SendToGeneratePasswords
    {
        public string CorrectHash { get; }
        public int EstimatedPassword { get; }

        public SendToGeneratePasswords(string correctHash, int estimatedPassword)
        {
            CorrectHash = correctHash;
            this.EstimatedPassword = estimatedPassword;
        }
    }
}
