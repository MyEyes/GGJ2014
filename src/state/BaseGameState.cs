using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vest.State
{
    public abstract class BaseGameState
    {
        public BaseGameState (bool isOverlay, bool acceptsInput)
        {
            this.IsOverlay = isOverlay;
            this.AcceptsInput = acceptsInput;
        }

        public readonly bool IsOverlay;
        public readonly bool AcceptsInput;

        public abstract void Update (float dt);
        public abstract void Draw();

        public virtual void Load()
        {
        }

        public virtual void Activate()
        {
        }

        public virtual void Deactivate()
        {
        }

        public virtual bool HandleInput (float dt)
        {
            return true;
        }
    }
}
