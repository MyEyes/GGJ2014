using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Otherworld
{
    public struct Range
    {
        public Range (float min, float max)
        {
            Assert.IsLessThanOrEqual (min, max);

            Min = min;
            Max = max;
            Delta = max - min;
        }

        public readonly float Min;
        public readonly float Max;
        public readonly float Delta;
    }

    public static class RangeHelpers
    {
        public static float RandomSample (this Range self, Random random)
        {
            return (float)random.NextDouble() * (self.Max - self.Min) + self.Min;
        }
    }
}
