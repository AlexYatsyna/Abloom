using System;
using System.Collections.Generic;

namespace Abloom.Models
{
    public sealed class SentPassword
    {
        public DateTime DispatchTime { get; }
        public DateTime ExpectedResponseTime { get; }
        public List<string> Passwords { get; }

        public SentPassword(DateTime dispatchTime, DateTime expectedResponseTime, List<string> passwords)
        {
            DispatchTime = dispatchTime;
            ExpectedResponseTime = expectedResponseTime;
            Passwords = passwords;
        }
    }
}
