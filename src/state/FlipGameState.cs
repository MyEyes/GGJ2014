﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
﻿using Microsoft.Xna.Framework;
﻿using Microsoft.Xna.Framework.Graphics;
﻿using spine_csharp_xna.src;
﻿using Vest.graphics;
using Vest.levels;
using Vest.State;
using Vest.utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Vest.state
{
    public class FlipGameState
        : BaseGameState
    {
        private const bool DRAW_DEBUG = true;

        OSpriteBatch batch;
        SpriteBatch uiBatch;
        DrawHelper helper;
        VarTracker tracker;
        SpriteFont font;
        ManualCamera2D cam;
        Vector2 camPos;

        CombiLevel currentLevel;
        int transitionLight = 255;

        Player player;
        PlayerController playerController;

        public FlipGameState()
            : base (false, true)
        {
        }

        public override void Load()
        {
            batch = new OSpriteBatch (G.Gfx);
            uiBatch = new SpriteBatch (G.Gfx);
            helper = new DrawHelper (G.Gfx);
            font = G.Content.Load<SpriteFont> ("font");

            cam = new ManualCamera2D (G.SCREEN_WIDTH, G.SCREEN_HEIGHT, G.Gfx);


            player = new Player (Vector2.Zero, new Polygon[0]);
            currentLevel = new CombiLevel1(cam, player);
            player.SetLevel (currentLevel);
            player.position = new Vector2(1048, 400);
            playerController = new PlayerController (player);


            tracker = new VarTracker();
            tracker.Track ("Anim", () => player.AnimState.GetTrackPlayingString (0));

            cam.Zoom = 1f;
            cam.CenterOnPoint (camPos = player.position);
        }

        public override void Update(float dt)
        {
            currentLevel.Update(dt);
            currentLevel.Update (player);
            playerController.Update ();
            player.Update (dt);

            KeyboardState state = Keyboard.GetState();
            GamePadState padState = GamePad.GetState (PlayerIndex.One);

            if (state.IsKeyDown(Keys.W))
                transitionLight++;
            if (state.IsKeyDown(Keys.S))
                transitionLight--;

            if (padState.DPad.Up == ButtonState.Pressed)
            {
                currentLevel.SetState (LevelState.Evil);
                currentLevel.SetTargetInsanity (1.1f);
            }

            var targetPos = player.position - new Vector2 (0, 200);
            camPos = Vector2.Lerp (camPos, targetPos, 0.2f);
            cam.CenterOnPoint (camPos);
        }

        public override void Draw()
        {
            G.Gfx.Clear (Color.Black);
            currentLevel.Draw (batch, cam);

            if (DRAW_DEBUG)
            {
                currentLevel.DrawDebug (batch, cam, helper);
                helper.DrawPolys (player.collisionPolys, cam.Transformation, Color.Pink);
                tracker.Draw (uiBatch, font);
            }
        }
    }
}

