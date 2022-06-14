using System;
using System.Numerics;


namespace Abloom.Messages
{
    public class RespondPassword
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
