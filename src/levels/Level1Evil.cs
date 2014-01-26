using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vest.graphics;

namespace Vest.levels
{
    public class Level1Evil
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

            CCollision(new Vector2(33, 18), new Vector2(47, 18), new Vector2(47, 435), new Vector2(33, 437)); // WALL LEFT
            CCollision(new Vector2(4201, 104), new Vector2(4217, 104), new Vector2(4218, 446), new Vector2(4195, 445)); // WALL RIGHT

            CCollision (
                new Vector2 (48, 438),
                new Vector2 (326, 434),
                new Vector2 (321, 406),
                new Vector2 (391, 376),
                new Vector2 (374, 343),
                new Vector2 (494, 316),
                new Vector2 (528, 339),
                new Vector2 (586, 351),
                new Vector2 (698, 373),
                new Vector2 (704, 416),
                new Vector2 (934, 413),
                new Vector2 (969, 434),
                new Vector2 (1484, 431),
                new Vector2 (1521, 423),
                new Vector2 (1639, 381),
                new Vector2 (1743, 423),
                new Vector2 (1888, 434),
                new Vector2 (1981, 439),
                new Vector2 (1981, 459),
                new Vector2 (1953, 579),
                new Vector2 (1959, 644),
                new Vector2 (2249, 556),
                new Vector2 (2169, 456),
                new Vector2 (2363, 424),
                new Vector2 (2479, 386),
                new Vector2 (2499, 384),
                new Vector2 (2596, 334),
                new Vector2 (2721, 334),
                new Vector2 (2836, 356),
                new Vector2 (2933, 354),
                new Vector2 (3056, 439),
                new Vector2 (3043, 608),
                new Vector2 (43, 778));
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