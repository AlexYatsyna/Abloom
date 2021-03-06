using System.Numerics;

namespace Abloom.Messages
{
    public sealed class SetInitialData
    {
        public string Hash { get; }
        public int PasswordLength { get; }
        public SetInitialData(string hash, int passwordLength)
        {
            Hash = hash;
            PasswordLength = passwordLength;
        }
    }
}
