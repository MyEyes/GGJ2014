using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Vest.graphics;

namespace Vest
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

        public Trigger(bool playerOnly, bool triggerOnce, Polygon[] collision)
            : base (Vector2.Zero, collision)
        {
            this.hasTriggered = false;
            this.triggerOnce = triggerOnce;
            this.collision = collision;
        }

        public void TryTrigger(GameObject o)
        {
            // Player is inside the volume
            if (o.Collides (this))
            {
                bool canTrigger = !triggerOnce || !hasTriggered;

                if (!inside.Contains (o) && canTrigger)
                    onEntered (o);
            }
            else
            {
                if (inside.Contains (o))
                    onExited (o);
            }
        }

        public bool IsObjectIn (GameObject o)
        {
            return inside.Contains (o);
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

        public override void Update(float dt) { throw new NotImplementedException (); }
        public override void Draw(OSpriteBatch batch) { throw new NotImplementedException (); }
    }
}
