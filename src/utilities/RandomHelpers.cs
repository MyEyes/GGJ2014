using System;

namespace Otherworld.Utilities
{
    public static class RandomHelpers
    {
        public static float NextFloat (this Random random)
        {
            return (float)random.NextDouble();
        }
    }
}
