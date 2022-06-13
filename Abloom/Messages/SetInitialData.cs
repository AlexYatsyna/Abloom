using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Messages
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
