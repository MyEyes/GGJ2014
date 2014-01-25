using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Otherworld.Utilities
{
    public static class GeomHelper
    {
        public static float mod (float x, float m)
        {
            return (x % m + m) % m;
        }

        public static float getRotation (this Vector2 vec)
        {
            return (float)Math.Atan2 (vec.Y, vec.X);
        }
    }
}
