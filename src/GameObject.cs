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

        //Polygons are assumed to be relative to the characters origin
        //So we move them by how much the origin of the object
        //Is away from the worlds actual origin
        public GameObject(Vector2 position, Polygon[] polygons)
        {
            collisionPolys = polygons;
            this.position = position;
            for (int x = 0; x < collisionPolys.Length; x++)
                collisionPolys[x].Move(position);
        }

        public void Move(Vector2 diff)
        {
            position += diff;
            for (int x = 0; x < collisionPolys.Length; x++)
                collisionPolys[x].Move(diff);
        }

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
