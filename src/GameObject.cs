using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vest
{
    class GameObject
    {
        Polygon[] collisionPolys;
        Vector2 position;

        public bool Collides(GameObject other)
        {
            for (int x = 0; x < collisionPolys.Length; x++)
                for (int y = 0; y < other.collisionPolys.Length; y++)
                    if (collisionPolys[x].Intersects(other.collisionPolys[x]))
                        return true;
            return false;
        }
    }
}
