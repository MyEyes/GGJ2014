using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vest.levels
{
    public class TestLevel
        : VestLevel
    {
        ManualCamera2D cam;

        public override void Load(ManualCamera2D cam)
        {
            this.cam = cam;

            Texture2D lightMask = G.Content.Load<Texture2D> ("lamp_light");

            Lights = new LightOverlay (G.Gfx);
            Lights.AmbientColor = new Color (20, 20, 20);

            CLight (350, Color.Gray, lightMask, new Vector2 (150, -160) + new Vector2 (500, 500));
            CLight (350, Color.Gray, lightMask, new Vector2 (-170, -160) + new Vector2 (500, 500));

            CCollision (new Vector2 (-500, 0), new Vector2 (500, 0), new Vector2 (500, 10), new Vector2 (-500, 10));
            CCollision (new Vector2 (-500, 0), new Vector2 (-250, 0), new Vector2 (-250, 0), new Vector2 (-500, -100));
            CCollision (new Vector2 (-10, 0), new Vector2 (10, 0), new Vector2 (0, -9.8f));
            CCollision (new Vector2 (120, 0), new Vector2 (180, 0), new Vector2 (150, -15));
            CCollision (new Vector2 (120, -160), new Vector2 (180, -160), new Vector2 (150, -175));
            CCollision (new Vector2 (-200, -160), new Vector2 (-140, -160), new Vector2 (-170, -175));

            // Because the level data was created assuming it was relative to the center of the level
            Collision.ForEach (p => p.Move (new Vector2 (500, 500)));
        }

        public override void Update()
        {
            Lights.SetCam (cam);
        }
    }
}
