using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vest.graphics;
using Otherworld.Utilities;

namespace Vest
{
    public class Elevator
        : SpineGameObject
    {
        public bool Enabled;
        public bool MovingIn = false;
        public bool Opening = false;
        public bool Closing = false;
        public bool MovingOut = false;
        public float StopY = 440;
        public float Speed = 2;
        public Vector2 doorPosition;
        Texture2D backGround;
        Texture2D frame;
        Player player;

        public Elevator (Player player, Vector2 position,Vector2 doorPosition, Polygon[] polygons)
            : base (position, polygons)
        {
            LoadSkeleton("Content/objects/elevator/elevator.json", "objects/elevator");
            backGround = G.Content.Load<Texture2D>("objects/elevator/elements/elevInside");
            frame = G.Content.Load<Texture2D>("objects/elevator/elements/elevFrame");
            SetAnim("closed", LookDir.Right, false);
            Depth = 0.4f;
            this.doorPosition = doorPosition;
            MovingIn = true;
            player.DisableInput++;
            this.player = player;
            player.Depth = 0.35f;
            TaskHelper.SetDelay(4500,
                delegate { SoundHelper.PlaySound("sounds/ding"); });
            TaskHelper.SetDelay(5000,
                delegate { Opening = true; SoundHelper.PlaySound("sounds/elevator_openclose"); });
            TaskHelper.SetDelay(6400,
                delegate { Closing = true; Opening = false; player.Depth = 0.9f; });
            TaskHelper.SetDelay(7400,
                delegate { Closing = false;  player.DisableInput--; });
        }

        public void Interact (Player player)
        {
            if (!Enabled)
                return;
            this.player = player;
            player.DisableInput++;
            player.Controller.Script = true;
            player.Controller.MoveToX = position;
            Enabled = false;
            TaskHelper.SetDelay(550,
                delegate { player.Controller.ChangeState(PlayerState.Back); player.Controller.Script = false; });
            TaskHelper.SetDelay(1250,
                delegate { SoundHelper.PlaySound("sounds/ding"); });
            TaskHelper.SetDelay(2000,
                delegate { Opening = true; SoundHelper.PlaySound("sounds/elevator_openclose"); });
            TaskHelper.SetDelay(3400,
                delegate { player.Depth = 0.35f; Opening = false; Closing = true; });
            TaskHelper.SetDelay(5400,
                delegate { Closing = false; player.Controller.ChangeState(PlayerState.Idle); MovingOut = true; });
        }

        public override void Update (float dt)
        {
            if (MovingIn)
            {
                if (position.Y < StopY)
                {
                    Move(new Vector2(0, Speed));
                    player.position = this.position;
                }
            }
            if (Opening)
            {
                AddAnim("open", LookDir.Right, false);
                Opening = false;
            }
            if (Closing)
            {
                AddAnim("closing", LookDir.Right, false);
            }
            if (MovingOut)
            {
                Move(new Vector2(0, Speed));
                player.position = this.position;
            }

            AnimState.Update(dt / 1000);
            AnimState.Apply(skeleton);
            skeleton.X = doorPosition.X;
            skeleton.Y = doorPosition.Y;
            skeleton.UpdateWorldTransform();
        }

        public override void Draw (OSpriteBatch batch)
        {
            batch.Draw(backGround, position + new Vector2(-60, -175), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.3f);
            batch.Draw(frame, doorPosition+ new Vector2(-79, -192), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.6f);
            base.Draw(batch);
        }
    }
}
