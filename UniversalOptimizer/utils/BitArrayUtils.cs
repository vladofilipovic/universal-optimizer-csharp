using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalOptimizer.utils
{
    public static class BitArrayUtils
    {
        /// <summary>
        /// Counts the ones helper.
        /// </summary>
        /// <param name="representation">The representation.</param>
        /// <returns></returns>
        public static int CountOnes(this BitArray representation)
        {
            int onesCount = 0;
            foreach (var x in representation)
                if ((int)x != 0)
                    onesCount++;
            return onesCount;
        }

    }
}
