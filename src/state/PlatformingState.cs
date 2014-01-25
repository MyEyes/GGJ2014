using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Otherworld.Utilities;
using Vest.levels;
using Vest.utilities;
using Vest.graphics;
using Vest.State;

namespace Vest.state
{
    public class PlatformingState
        : BaseGameState
    {
        public PlatformingState()
            : base (false, true)
        {
        }

        OSpriteBatch batch;
        DrawHelper helper;
        ManualCamera2D cam;

        PhysicsSpineGameObject player;
        PlayerController playerController;
        
        VestLevel level;
        Light playerLight;
        
        public override void Load()
        {
            batch = new OSpriteBatch (G.Gfx);
            helper = new DrawHelper (G.Gfx);
            
            cam = new ManualCamera2D (G.SCREEN_WIDTH, G.SCREEN_HEIGHT, G.Gfx);

            level = new TestLevel ();
            level.Load (cam);

            player = new PhysicsSpineGameObject (Vector2.Zero, new Polygon[] {new Polygon (new Vector2[] {
                new Vector2(20, 0),
                new Vector2 (-20, 0),
                new Vector2 (-20, -140),
                new Vector2(20, -140)
            })});

            player.SetLevel (level);
            playerController = new PlayerController (player);
            playerLight = level.CLight (350, Color.Gray, null, new Vector2 (400, 400));            

            cam.Zoom = 1f;
            cam.CenterOnPoint (player.position - new Vector2(0, 50));
        }

        public override void Activate()
        {
        }

        public override void Update (float dt)
        {
            // Move player light to the player
            playerLight.Position = player.position + new Vector2(0, -50);
            
            level.Update();
            playerController.Update();
            player.Update (dt);
        }

        public override void Draw()
        {
            level.Lights.DrawLights();

            G.Gfx.Clear(Color.CornflowerBlue);
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, cam.Transformation);
            player.Draw(batch);
            batch.End();

            level.Lights.Apply (batch);

            helper.DrawPolys(player.collisionPolys, cam.Transformation, Color.Green);
            helper.DrawPolys (level.Collision, cam.Transformation, Color.Red);
        }
    }
}
