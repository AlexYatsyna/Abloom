namespace Abloom2.Messages
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
