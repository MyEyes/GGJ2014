using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Vest.levels;
using Vest.state;

namespace Vest
{
    public class PhysicsSpineGameObject
        : SpineGameObject
    {
        const float GRAVITY = 300f;
        const float JUMP_STRENGTH = -0.6f * GRAVITY;

        public bool OnGround { get; private set; }
        public CombiLevel level { get; private set; }
    
        Vector2 toMove = Vector2.Zero;
        float ySpeed = 0;

        public PhysicsSpineGameObject(Vector2 position, Polygon[] polys)
            : base(position, polys)
        {

        }

        public void SetLevel (CombiLevel level)
        {
            this.level = level;
        }

        public override void Move(Vector2 diff)
        {
            toMove += diff;
        }

        public void Jump()
        {
            ySpeed = JUMP_STRENGTH;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            dt /= 1000f;

            //Move player controlled
            base.Move(toMove);

            //Move gravity
            if (level.IsColliding (this))
            {
                base.Move(new Vector2(0, -Math.Abs(toMove.X)));
                if (level.IsColliding (this))
                    base.Move(-toMove + new Vector2(0, Math.Abs(toMove.X)));
            }
            ySpeed += GRAVITY * dt;
            OnGround = false;
            base.Move(new Vector2(0, ySpeed*dt));
            if (level.IsColliding (this))
            {
                base.Move(new Vector2(0, -ySpeed * dt));
                if (ySpeed > 0)
                    OnGround = true;
                ySpeed = 0;
            }
            toMove = Vector2.Zero;
        }
    }
}
