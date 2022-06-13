using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
