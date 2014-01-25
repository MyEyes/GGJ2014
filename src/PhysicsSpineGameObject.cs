using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vest
{
    class PhysicsSpineGameObject:SpineGameObject
    {
        const float gravity = 300f;
        const float JumpStrength = -0.6f*gravity;
        float ySpeed = 0;
        //TODO: Hack, should be a reference to the level instead
        public GameObject[] collisions;
        Vector2 toMove = Vector2.Zero;
        public bool _onGround = false;

        public PhysicsSpineGameObject(Vector2 position, Polygon[] polys)
            : base(position, polys)
        {

        }

        public override void Move(Vector2 diff)
        {
            toMove += diff;
        }

        public void Jump()
        {
            ySpeed = JumpStrength;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            dt /= 1000f;
            //Move player controlled
            base.Move(toMove);
            //Move gravity
            if (Collides())
            {
                base.Move(new Vector2(0, -Math.Abs(toMove.X)));
                if (Collides())
                    base.Move(-toMove + new Vector2(0, Math.Abs(toMove.X)));
            }
            ySpeed += gravity * dt;
            _onGround = false;
            base.Move(new Vector2(0, ySpeed*dt));
            if (Collides())
            {
                base.Move(new Vector2(0, -ySpeed * dt));
                if (ySpeed > 0)
                    _onGround = true;
                ySpeed = 0;
            }
            toMove = Vector2.Zero;
        }

        private bool Collides()
        {
            for (int x = 0; x < collisions.Length; x++)
            {
                if (Collides(collisions[x]))
                    return true;
            }
            return false;
        }

        public bool OnGround
        {
            get { return _onGround; }
        }
    }
}
