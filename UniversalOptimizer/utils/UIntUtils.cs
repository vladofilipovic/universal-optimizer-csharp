using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalOptimizer.utils
{
    public static class UIntUtils
    {
        /// <summary>
        /// Counts the ones.
        /// </summary>
        /// <param name="no">The no.</param>
        /// <returns></returns>
        public static int CountOnes(this UInt32 no)
        {
            int count = 0;
            while (no > 0)
            {
                if (no % 2 != 0)
                    count++;
                no /= 2;
            }
            return count;
        }
    }
}
