using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Messages
{
    internal sealed class SetNumberOfNodes
    {
        public int NumberOfNodes { get; }

        public SetNumberOfNodes(int numberOfNodes)
        {
            NumberOfNodes = numberOfNodes;
        }
    }
}
