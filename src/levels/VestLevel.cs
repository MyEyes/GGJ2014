using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vest.levels
{
    public abstract class VestLevel
    {
        public List<GameObject> GameObjects;
        public LightOverlay Lights;
        public List<Polygon> Collision = new List<Polygon> ();

        public Light CLight(float radius, Color color, Texture2D mask, Vector2 position)
        {
            Light light = new Light
            {
                Mask = mask,
                Color = color,
                Position = position,
                Radius = radius
            };
            Lights.AddLight (light);
            return light;
        }

        public void CreateTrigger()
        {
        }

        public void CCollision(params Vector2[] args)
        {
            Collision.Add (new Polygon (args));
        }

        public bool IsColliding(GameObject o)
        {
            return o.Collides (Collision);
        }
    }
}
