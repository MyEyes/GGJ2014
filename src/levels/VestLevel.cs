using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vest.graphics;

namespace Vest.levels
{
    public abstract class VestLevel
    {
        public List<GameObject> GameObjects;
        public List<Polygon> Collision = new List<Polygon>();
        public List<Trigger> Triggers = new List<Trigger>();
        public LightOverlay Lights;

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

        public abstract void Load (ManualCamera2D cam);
        public virtual void Draw (OSpriteBatch batch) { }

        public virtual void Update(float dt)
        {

        }

        public virtual void Update (GameObject player)
        {
            foreach (var t in Triggers)
                t.TryTrigger (player);
        }

        public Trigger CTrigger (bool playerOnly, bool triggerOnce, Polygon poly)
        {
            Trigger newTrigger = new Trigger (playerOnly, triggerOnce, new[] { poly });
            Triggers.Add (newTrigger);
            return newTrigger;
        }

        public void CCollision(params Vector2[] args)
        {
            Collision.Add (new Polygon (args));
        }

        public bool IsColliding(GameObject o)
        {
            return o.Collides (Collision);
        }

        public void BindToggleLight(Trigger t, Light l)
        {
            t.Entered += o => l.Enabled = true;
            t.Exited += o => l.Enabled = false;
        }
    }
}
