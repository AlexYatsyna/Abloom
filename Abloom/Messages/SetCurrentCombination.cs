using System.Numerics;


namespace Abloom.Messages
{
    public sealed class SetCurrentCombination
    {
        public BigInteger CurrentCombination { get; }

        public SetCurrentCombination(BigInteger currentCombination)
        {
            CurrentCombination = currentCombination;
        }
    }
}
