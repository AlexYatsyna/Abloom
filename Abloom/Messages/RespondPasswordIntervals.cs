using System.Collections.Generic;

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
