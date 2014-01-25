using System;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Otherworld.Utilities;
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

        ManualCamera2D cam;
        PhysicsSpineGameObject player;
        PlayerController playerController;
        OSpriteBatch batch;
        DrawHelper helper;

        GameObject CollisionTest;
        
        public override void Load()
        {
            batch = new OSpriteBatch (G.Gfx);

            cam = new ManualCamera2D (G.SCREEN_WIDTH, G.SCREEN_HEIGHT, G.Gfx);
            player = new PhysicsSpineGameObject(Vector2.Zero, new Polygon[] { new Polygon(new Vector2[] { new Vector2(20, 0), new Vector2(-20, 0), new Vector2(-20, -140), new Vector2(20, -140) }) });
            playerController = new PlayerController (player);
            helper = new DrawHelper(G.Gfx);

            cam.Zoom = 1f;
            cam.CenterOnPoint (player.position - new Vector2(0, 50));

            CollisionTest = new SpineGameObject(Vector2.Zero, new Polygon[]{
                new Polygon(new Vector2[]{new Vector2(-500,0),new Vector2(500,0), new Vector2(500,10), new Vector2(-500,10)}),
                new Polygon(new Vector2[]{new Vector2(-500,0),new Vector2(-250,0), new Vector2(-250,0), new Vector2(-500,-100)}),
                new Polygon(new Vector2[]{new Vector2(-500,0),new Vector2(-490,0), new Vector2(-490,-200), new Vector2(-500,-200)}),
                new Polygon(new Vector2[]{new Vector2(500,0),new Vector2(490,0), new Vector2(490,-200), new Vector2(500,-200)}),
                new Polygon(new Vector2[]{new Vector2(-10,0),new Vector2(10,0), new Vector2(0,-15)}),
                new Polygon(new Vector2[]{new Vector2(120,0),new Vector2(180,0), new Vector2(150,-15)}),
                new Polygon(new Vector2[]{new Vector2(120,-160),new Vector2(180,-160), new Vector2(150,-175)})
            });

            CollisionTest.Move(new Vector2(500, 500));

            //TODO: THIS IS A HACK UNTIL LEVEL CLASS EXISTS, See PhysicsSpineGameObject.cs
            player.collisions = new GameObject[] { CollisionTest };
        }

        public override void Activate()
        {
        }

        public override void Update (float dt)
        {
            playerController.Update();
            player.Update (dt);
        }

        public override void Draw()
        {
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, cam.Transformation);
            player.Draw(batch);
            batch.End();
            helper.DrawPolys(player.collisionPolys, cam.Transformation, Color.Red);
            helper.DrawPolys(CollisionTest.collisionPolys, cam.Transformation, Color.Red);
        }
    }
}
