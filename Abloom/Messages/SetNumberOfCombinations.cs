using System.Numerics;

namespace Abloom.Messages
{
    public sealed class SetNumberOfCombinations
    {
        public BigInteger NumberOfCombinations { get; }

        public SetNumberOfCombinations(BigInteger numberOfCombinations)
        {
            NumberOfCombinations = numberOfCombinations;
        }
    }
}
