using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Vest
{
    public class PlayerController
    {
        private GamePadState currPadState;
        private GamePadState prevPadState;
        private readonly SpineGameObject player;
        private readonly Timer idleTimer;

        private const float MOVE_SPEED = 2;

        public PlayerController (SpineGameObject player)
        {
            this.player = player;
            LoadPlayerAnim();

            idleTimer = new Timer (new Random ().Next (3000, 8000));
            idleTimer.Elapsed += (s, a) =>
            {
                player.AnimState.AddAnimation (0, "idle2", false, 0);
                player.AnimState.AddAnimation (0, "idle", true, 0);
                idleTimer.Interval = new Random ().Next (3000, 8000);
            };
        }

        public void Update()
        {
            currPadState = GamePad.GetState (PlayerIndex.One);

            Vector2 moveVector = Vector2.Zero;
            if (isButtonDown (currPadState.DPad.Up)) moveVector.Y = -1;
            if (isButtonDown (currPadState.DPad.Down)) moveVector.Y = 1;
            if (isButtonDown (currPadState.DPad.Left)) moveVector.X = -1;
            if (isButtonDown (currPadState.DPad.Right)) moveVector.X = 1;

            if (moveVector != Vector2.Zero)
            {
                player.Move (moveVector * MOVE_SPEED);
            }

            prevPadState = currPadState;
        }

        private void LoadPlayerAnim()
        {
            player.LoadSkeleton ("content/player/vest.json", "player/");
            player.position = new Vector2 (400, 400);
            player.AnimState.SetAnimation (0, "idle", true);
            player.AnimData.SetMix ("idle", "run", 0.3f);
            player.AnimData.SetMix ("idle2", "run", 0.3f);
            player.AnimData.SetMix ("run", "idle", 0.3f);
            player.AnimData.SetMix ("run", "idle2", 0.3f);
        }

        private bool isButtonDown (ButtonState bState)
        {
            return bState == ButtonState.Pressed;
        }
    }
}
