using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Messages
{
    public sealed class GetMembers
    {
        public string Role { get; }

        public GetMembers(string role)
        {
            Role = role;
        }
    }
}
