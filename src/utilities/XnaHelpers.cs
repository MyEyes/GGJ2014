using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Otherworld.Utilities
{
    public static class XnaHelpers
    {
        public static Color toColor (Int32 color)
        {
            int red = (color & 0xFF0000) >> 16;
            int green = (color & 0x00FF00) >> 8;
            int blue = (color & 0x0000FF);

            return new Color (red, green, blue, 255);
        }

        public static uint ToPackedARGB (this Color color)
        {
            return ((uint)color.A << 24)
                & ((uint)color.R << 16)
                & ((uint)color.G << 8)
                & color.B;
        }

        public static Vector3 ToVector3 (this Vector2 vec2)
        {
            return new Vector3 (vec2.X, vec2.Y, 0);
        }
    }
}
