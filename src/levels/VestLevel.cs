using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vest.graphics;
using Vest.utilities;

namespace Vest.levels
{
    public abstract class VestLevel
    {
        public List<GameObject> GameObjects = new List<GameObject> ();
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
        
        public virtual void Draw (OSpriteBatch batch)
        {
            foreach (var g in GameObjects)
                g.Draw (batch);
        }

        public virtual void DrawDebug (OSpriteBatch batch, ManualCamera2D cam, DrawHelper helper)
        {
            foreach (var g in GameObjects)
                helper.DrawPolys (g.collisionPolys, cam.Transformation, Color.White);
        }

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

        public Table CTable (Vector2 pos, bool isGood)
        {
            var newTable = new Table (pos, new Polygon[0], isGood);
            GameObjects.Add (newTable);
            return newTable;
        }

        public IEnumerable<T> GetObjects<T>()
            where T : GameObject
        {
            return GameObjects
                .Where (o => o is T)
                .Cast<T>();
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
