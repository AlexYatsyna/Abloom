using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Data
{
    public class CommonData
    {
        public static string InputHash { get; set; }
        public static int PasswordLength { get; set; }
        public static BigInteger NumberOfPassCombinations { get; set; }
        public static BigInteger CurrentNumberOfComb = 1;
        public static bool isFound = false;
        public static bool isExit = false;
    }
}
