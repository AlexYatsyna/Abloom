using Akka.Cluster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Messages
{
    public sealed class RespondMembers
    {
        public List<Member>? Members { get; }

        public RespondMembers(List<Member>? members)
        {
            Members = members;
        }
    }
}
