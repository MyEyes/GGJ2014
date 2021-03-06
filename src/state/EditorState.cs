﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Vest.graphics;
using Vest.levels;
using Vest.State;
using Vest.utilities;

namespace Vest.state
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Otherworld.Utilities;

    namespace Vest.state
    {
        public class EditState
            : BaseGameState
        {
            public EditState()
                : base (false, true)
            {
            }

            EditorViewController view;
            ManualCamera2D cam;

            CombiLevel level;
            OSpriteBatch batch;
            SpriteBatch uiBatch;
            DrawHelper helper;
            SpriteFont font;

            private MouseState currMouse;
            private MouseState prevMouse;
            Vector2 mousePos;
            Vector2 mouseWorldPos;

            Player player;
            PlayerController pController;

            public List<Vector2> polyBuffer;
            public Polygon bufferedPoly;

            public override void Load()
            {
                batch = new OSpriteBatch (G.Gfx);
                uiBatch = new SpriteBatch (G.Gfx);
                helper = new DrawHelper (G.Gfx);

                view = new EditorViewController (G.Window);
                view.SetSize (G.SCREEN_WIDTH, G.SCREEN_HEIGHT);
                view.OriginX = 2000;
                view.OriginY = 200;
                view.Zoom = 0.8f;

                cam = new ManualCamera2D (G.SCREEN_WIDTH, G.SCREEN_HEIGHT, G.Gfx);
                
                player = new Player (Vector2.Zero, new Polygon[0]);
                player.Load (cam);
                pController = new PlayerController (player);

                level = new CombiLevel1 (cam, player);
                font = G.Content.Load<SpriteFont> ("font");

                player.SetLevel (level);
            }

            public override void Activate()
            {
            }

            public override void Update(float dt)
            {
                level.Update (dt);

                currMouse = Mouse.GetState();

                view.Update();
                cam.Zoom = view.Zoom;
                cam.CenterOnPoint (view.OriginX, view.OriginY);

                var mouseState = Mouse.GetState();
                mousePos = new Vector2 (mouseState.X, mouseState.Y);
                mouseWorldPos = cam.ScreenToWorld (mousePos);

                if (prevMouse.LeftButton == ButtonState.Released && currMouse.LeftButton == ButtonState.Pressed)
                    AddVertexToBuffer(mouseWorldPos);

                if (prevMouse.MiddleButton == ButtonState.Released && currMouse.MiddleButton == ButtonState.Pressed)
                    FlushPolyBuffer();

                var padState = GamePad.GetState(PlayerIndex.One);
                if (padState.DPad.Up == ButtonState.Pressed)
                {
                    level.SetTargetInsanity (2f);
                    level.SetState (LevelState.Evil);
                }

                if (padState.DPad.Down == ButtonState.Pressed)
                {
                    level.SetTargetInsanity (0);
                    level.SetState (LevelState.Good);
                }

                prevMouse = currMouse;
            }

            public override void Draw()
            {
                G.Gfx.Clear (Color.CornflowerBlue);
                level.Draw (batch, cam);
                level.DrawDebug (batch, cam, helper);

                if (bufferedPoly != null)
                    helper.DrawPolys (new [] {bufferedPoly}, cam.Transformation, Color.Green);
                
                uiBatch.Begin();
                uiBatch.DrawString (font, String.Format ("{0:0}, {1:0}", mouseWorldPos.X, mouseWorldPos.Y), Vector2.Zero, Color.Yellow);
                uiBatch.End();
            }

            private void AddVertexToBuffer (Vector2 worldPos)
            {
                if (polyBuffer == null)
                    polyBuffer = new List<Vector2>();
                
                polyBuffer.Add (worldPos);

                if (polyBuffer.Count >= 3)
                    bufferedPoly = new Polygon (polyBuffer.ToArray());
            }

            private void FlushPolyBuffer()
            {
                if (polyBuffer == null)
                    return;

                Console.WriteLine ("\n============");
                //Console.WriteLine (String.Join (",\n", polyBuffer.Select (v => String.Format ("new Vector2({0:0}, {1:0})", v.X, v.Y))));
                Console.WriteLine ("CCollision (" + String.Join (",", polyBuffer.Select (v => String.Format ("new Vector2({0:0}, {1:0})", v.X, v.Y))) + ");");
                Console.WriteLine ("\n============");

                if (bufferedPoly != null && !bufferedPoly.IsConvex)
                    Console.WriteLine ("WARNING: NOT CONVEX");

                bufferedPoly = null;
                polyBuffer = null;
            }
        }
    }

}
