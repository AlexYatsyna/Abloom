using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbloomWorkingNode.Messages
{
    public sealed class RemovePathRoutee
    {
        public string Path { get; }

        public RemovePathRoutee(string path)
        {
            Path = path;
        }
    }
}
