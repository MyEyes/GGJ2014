using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Vest.graphics;
using Vest.levels;
using Vest.levels.Vest.levels;
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

            VestLevel level;
            OSpriteBatch batch;
            SpriteBatch uiBatch;
            DrawHelper helper;
            SpriteFont font;

            private MouseState currMouse;
            private MouseState prevMouse;
            Vector2 mousePos;
            Vector2 mouseWorldPos;

            public List<Vector2> polyBuffer;
            public Polygon bufferedPoly;

            public override void Load()
            {
                VestLevel LEVEL_TO_EDIT = new Branch1();

                batch = new OSpriteBatch (G.Gfx);
                uiBatch = new SpriteBatch (G.Gfx);
                helper = new DrawHelper (G.Gfx);

                view = new EditorViewController (G.Window);
                view.SetSize (G.SCREEN_WIDTH, G.SCREEN_HEIGHT);
                cam = new ManualCamera2D (G.SCREEN_WIDTH, G.SCREEN_HEIGHT, G.Gfx);

                level = LEVEL_TO_EDIT;
                level.Load (cam);

                font = G.Content.Load<SpriteFont> ("font");
            }

            public override void Activate()
            {
            }

            public override void Update(float dt)
            {
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

                prevMouse = currMouse;
            }

            public override void Draw()
            {
                G.Gfx.Clear (Color.CornflowerBlue);
                batch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend, cam.Transformation);
                level.Draw(batch);
                batch.End ();

                helper.DrawPolys (level.Collision, cam.Transformation, Color.Red);
                level.Triggers.ForEach (t => {
                    helper.DrawPolys (t.collisionPolys, cam.Transformation, Color.Green);
                });

                if (bufferedPoly != null)
                    helper.DrawPolys (new [] {bufferedPoly}, cam.Transformation, Color.Green);
                
                uiBatch.Begin();
                uiBatch.DrawString (font, String.Format ("{0}, {1}", mouseWorldPos.X, mouseWorldPos.Y), Vector2.Zero, Color.Yellow);
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
                
                for (var i = 0; i < polyBuffer.Count; i++)
                {
                    var vert = polyBuffer[i];
                    Console.Write("new Vector2({0:0}, {1:0})", vert.X, vert.Y);
                    
                    if (i < polyBuffer.Count-1)
                        Console.WriteLine (",");
                }

                Console.WriteLine ("\n============");

                bufferedPoly = null;
                polyBuffer = null;
            }
        }
    }

}
