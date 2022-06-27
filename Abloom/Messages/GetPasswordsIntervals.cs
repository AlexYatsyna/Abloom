namespace Abloom2.Messages
{
    public sealed class GetPasswordsIntervals
    {
        public int NumberOfIntervals { get; }
        public int NumebrOfPasswordsInTheInterval { get; }

        public GetPasswordsIntervals(int numberOfIntervals, int numebrOfPasswordsInTheInterval)
        {
            NumberOfIntervals = numberOfIntervals;
            NumebrOfPasswordsInTheInterval = numebrOfPasswordsInTheInterval;
        }
    }
}
