using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vest
{
    public static class PolyFactory
    {
        public static Polygon Rect (float x, float y, float width, float height)
        {
            return new Polygon (new Vector2[]
            {
                new Vector2 (x, y),
                new Vector2 (x + width, y),
                new Vector2 (x + width, y + height),
                new Vector2 (x, y + height)
            });
        }
    }
}
