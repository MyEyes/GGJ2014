using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Vest.graphics;

namespace Vest
{
    public abstract class GameObject
    {
        public Polygon[] collisionPolys;

        public Vector2 position
        {
            get { return _position; }
            set
            {
                movePolygons (value);
                _position = value;
            }
        }

        private Vector2 _position;

        //Polygons are assumed to be relative to the characters origin
        //So we move them by how much the origin of the object
        //Is away from the worlds actual origin
        public GameObject(Vector2 position, Polygon[] polygons)
        {
            collisionPolys = polygons;
            this.position = position;
            movePolygons (position);
        }

        public abstract void Update (float dt);
        public abstract void Draw(OSpriteBatch batch);

        public virtual void Move(Vector2 diff)
        {
            movePolygons (position + diff);
            _position += diff;
        }

        public bool Collides(GameObject other)
        {
            return Collides (other.collisionPolys);
        }

        public bool Collides(IEnumerable<Polygon> polys)
        {
            // I need this for testing :<
            for (int x = 0; x < collisionPolys.Length; x++)
                foreach (var poly in polys)
                    if (collisionPolys[x].Intersects (poly))
                        return true;
                    
            return false;
        }

        public void movePolygons (Vector2 newPosition)
        {
            Vector2 relative = newPosition - position;
            for (int x = 0; x < collisionPolys.Length; x++)
                collisionPolys[x].Move (relative);
        }
    }

    public enum LookDir
    {
        Left,
        Right
    }
}
