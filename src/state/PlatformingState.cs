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

        LightOverlay light;
        
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
                new Polygon(new Vector2[]{new Vector2(-10,0),new Vector2(10,0), new Vector2(0,-9.8f)}),
                new Polygon(new Vector2[]{new Vector2(120,0),new Vector2(180,0), new Vector2(150,-15)}),
                new Polygon(new Vector2[]{new Vector2(120,-160),new Vector2(180,-160), new Vector2(150,-175)}),
                new Polygon(new Vector2[]{new Vector2(-200,-160),new Vector2(-140,-160), new Vector2(-170,-175)})
                    
            });

            CollisionTest.Move(new Vector2(500, 500));
            light = new LightOverlay(G.Gfx);
            light.AmbientColor = new Color(20, 20, 20);
            Texture2D mask = G.Content.Load<Texture2D>("lamp_light");

            Light light1;
            light1.Mask = mask;
            light1.Position = new Vector2(150, -160) + new Vector2(500, 500);
            light1.Radius = 350;
            light1.Color = Color.Gray;
            light.AddLight(light1);

            Light light2;
            light2.Mask = null;
            light2.Position=new Vector2(400,400);
            light2.Radius=350;
            light2.Color=Color.Gray;
            light.AddLight(light2);

            Light light3;
            light3.Mask = mask;
            light3.Position = new Vector2(-170, -160) + new Vector2(500, 500);
            light3.Radius = 350;
            light3.Color = Color.Gray;
            light.AddLight(light3);

            //TODO: THIS IS A HACK UNTIL LEVEL CLASS EXISTS, See PhysicsSpineGameObject.cs
            player.collisions = new GameObject[] { CollisionTest };
        }

        public override void Activate()
        {
        }

        public override void Update (float dt)
        {
            Light l1 = light.Lights[2];
            l1.Position = player.position + new Vector2(0, -50);
            light.Lights[2] = l1;

            light.SetCam(cam);
            playerController.Update();
            player.Update (dt);
        }

        public override void Draw()
        {
            light.DrawLights();

            G.Gfx.Clear(Color.CornflowerBlue);
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, cam.Transformation);
            player.Draw(batch);
            batch.End();

            light.Apply(batch);

            helper.DrawPolys(player.collisionPolys, cam.Transformation, Color.Red);
            helper.DrawPolys(CollisionTest.collisionPolys, cam.Transformation, Color.Red);
        }
    }
}
