using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public class FlipGameState
            : BaseGameState
        {
            OSpriteBatch batch;
            DrawHelper helper;
            ManualCamera2D cam;
            Vector2 camPos;

            Player player;
            PlayerController playerController;

            CombiLevel currentLevel;
            //VestLevel currentLevel;

            public FlipGameState()
                : base (false, true)
            {
            }

            public override void Load()
            {
                batch = new OSpriteBatch (G.Gfx);
                helper = new DrawHelper (G.Gfx);

                cam = new ManualCamera2D (G.SCREEN_WIDTH, G.SCREEN_HEIGHT, G.Gfx);

                VestLevel GoodLevel = new Branch1();
                GoodLevel.Load (cam);

                VestLevel BadLevel = new Branch1();
                BadLevel.Load(cam);

                currentLevel = new CombiLevel(GoodLevel, BadLevel);

                player = new Player (Vector2.Zero, new Polygon[] {new Polygon (new Vector2[] {
                    new Vector2(20, 0),
                    new Vector2 (-20, 0),
                    new Vector2 (-20, -140),
                    new Vector2(20, -140)
                })});

                player.SetLevel (currentLevel);
                player.position = new Vector2 (3922, 400);
                playerController = new PlayerController (player);

                cam.Zoom = 1f;
                cam.CenterOnPoint (camPos = player.position);
            }

            public override void Activate()
            {
            }

            public override void Update(float dt)
            {
                currentLevel.Update (player);
                playerController.Update ();
                player.Update (dt);

                var targetPos = player.position - new Vector2 (0, 200);
                camPos = Vector2.Lerp (camPos, targetPos, 0.2f);
                cam.CenterOnPoint (camPos);
            }

            public override void Draw()
            {
                G.Gfx.Clear (Color.Black);
                currentLevel.Draw (batch, cam, player);
            }
        }
    }

}
