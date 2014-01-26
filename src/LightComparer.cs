using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vest
{
    public class LightComparerer : IComparer<Light>
    {
        public int Compare(Light x, Light y)
        {
            int hashX = GetHashCode (x);
            int hashY = GetHashCode (y);

            if (hashX < hashY)
                return -1;
            if (hashX > hashY)
                return 1;

            return 0;
        }

        private int GetHashCode(Light obj)
        {
            return (obj.Mask != null ? obj.Mask.GetHashCode () : 0);
        }
    }
}
