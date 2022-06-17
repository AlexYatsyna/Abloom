using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Messages
{
    public sealed class RespondPasswordIntervals
    {
        public List<List<string>> Intervals { get; }

        public RespondPasswordIntervals(List<List<string>> intervals)
        {
            Intervals = intervals;
        }
    }
}
