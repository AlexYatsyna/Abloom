using System.Numerics;


namespace AbloomWorkingNode.Messages
{

    public class RespondPassword : IAppProtocol
    {
        public Guid Id { get; }
        public string Password { get; }
        public bool IsFound { get; }
        public BigInteger IntervalSize { get; }

        public RespondPassword(Guid id, bool isFound, BigInteger intervalSize, string password = "")
        {
            Id = id;
            Password = password;
            this.IsFound = isFound;
            IntervalSize = intervalSize;
        }
    }
}
