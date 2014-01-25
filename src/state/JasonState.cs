using System;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Otherworld.Utilities;
using Vest.graphics;
using Vest.State;

namespace Vest.state
{
    public class JasonState
        : BaseGameState
    {
        public JasonState()
            : base (false, true)
        {
        }

        ManualCamera2D cam;
        SpineGameObject player;
        PlayerController playerController;
        OSpriteBatch batch;
        
        public override void Load()
        {
            batch = new OSpriteBatch (G.Gfx);

            cam = new ManualCamera2D (G.SCREEN_WIDTH, G.SCREEN_HEIGHT, G.Gfx);
            player = new SpineGameObject (Vector2.Zero, new Polygon[0]);
            playerController = new PlayerController (player);

            cam.Zoom = 1f;
            cam.CenterOnPoint (player.position - new Vector2(0, 50));
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
            batch.Begin (SpriteSortMode.Immediate, BlendState.AlphaBlend, cam.Transformation);
            player.Draw (batch);
            batch.End();
        }
    }
}
