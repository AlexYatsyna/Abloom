using System.Numerics;

namespace Abloom2.Messages
{
    public sealed class SetInitialData
    {
        public string Hash { get; }
        public int PasswordLength { get; }
        public BigInteger NumberOfPassCombinations { get; }
        public SetInitialData(string hash, int passwordLength)
        {
            Hash = hash;
            PasswordLength = passwordLength;
            NumberOfPassCombinations = BigInteger.Pow(74, passwordLength);
        }
    }
}
