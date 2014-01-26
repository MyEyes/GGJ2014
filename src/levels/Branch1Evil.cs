using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vest.graphics;

namespace Vest.levels
{
    public class Branch1Evil
        : VestLevel
    {
        ManualCamera2D cam;
        Texture2D evilBG1;
        Texture2D evilBG2;

        public override void Load(ManualCamera2D cam)
        {
            this.cam = cam;

            evilBG1 = G.Content.Load<Texture2D>("branches/branch1/evilBG1");
            evilBG2 = G.Content.Load<Texture2D>("branches/branch1/evilBG2");

            Lights = new LightOverlay(G.Gfx);
            Lights.AmbientColor = new Color(255, 255, 255);

            CCollision(new Vector2(49, 422), new Vector2(4202, 423), new Vector2(4202, 434), new Vector2(47, 437)); // FLOOR
            CCollision(new Vector2(33, 18), new Vector2(47, 18), new Vector2(47, 435), new Vector2(33, 437)); // WALL LEFT
            CCollision(new Vector2(4201, 104), new Vector2(4217, 104), new Vector2(4218, 446), new Vector2(4195, 445)); // WALL RIGHT
        }

        public override void Update(GameObject player)
        {
            base.Update(player);
        }

        public override void Update(float dt)
        {
            Lights.SetCam(cam);
        }

        public override void Draw(OSpriteBatch batch)
        {
            batch.Draw(evilBG1, Vector2.Zero, Color.White);
            batch.Draw(evilBG2, new Vector2(evilBG1.Width, 0), Color.White);
        }
    }
}