using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vest.graphics;

namespace Vest.levels
{
    public class Trigger
        : GameObject
    {
        public event Action<GameObject> Entered;
        public event Action<GameObject> Exited;

        private Polygon[] collision;
        private bool hasTriggered;
        private bool triggerOnce;
        private bool playerOnly;

        private List<GameObject> inside = new List<GameObject> ();

        public Trigger (bool playerOnly, bool triggerOnce, Polygon[] collision)
            : base (Vector2.Zero, collision)
        {
            this.hasTriggered = false;
            this.triggerOnce = triggerOnce;
            this.collision = collision;
        }

        public void TryTrigger(GameObject player)
        {
            // Player is inside the volume
            if (player.Collides (this))
            {
                bool canTrigger = !triggerOnce || !hasTriggered;

                if (!inside.Contains (player) && canTrigger)
                    onEntered (player);
            }
            else
            {
                if (inside.Contains (player))
                    onExited(player);
            }
        }

        private void onEntered(GameObject target)
        {
            hasTriggered = true;
            inside.Add (target);

            if (Entered != null)
                Entered (target);
        }

        private void onExited(GameObject target)
        {
            inside.Remove (target);
            if (Exited != null)
                Exited (target);
        }

        public override void Update (float dt) { throw new NotImplementedException();}
        public override void Draw (OSpriteBatch batch) { throw new NotImplementedException(); }
    }

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
    }
}
