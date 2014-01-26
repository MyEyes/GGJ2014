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

        public override void Load (Player player, ManualCamera2D cam)
        {
            this.cam = cam;

            Texture2D lightMask = G.Content.Load<Texture2D> ("lamp_light");

            Lights = new LightOverlay (G.Gfx);
            Lights.AmbientColor = new Color (20, 20, 20);

            var light1 = CLight (350, Color.Gray, lightMask, new Vector2 (-170, -160) + new Vector2 (500, 500));
            var light2 = CLight (349, Color.Gray, lightMask, new Vector2 (150, -160) + new Vector2 (500, 500));

            CCollision (new Vector2 (-500, 0), new Vector2 (500, 0), new Vector2 (500, 10), new Vector2 (-500, 10));
            CCollision (new Vector2 (-500, 0), new Vector2 (-250, 0), new Vector2 (-250, 0), new Vector2 (-500, -100));
            CCollision (new Vector2 (-10, 0), new Vector2 (10, 0), new Vector2 (0, -9.8f));
            CCollision (new Vector2 (120, 0), new Vector2 (180, 0), new Vector2 (150, -15));
            CCollision (new Vector2 (120, -160), new Vector2 (180, -160), new Vector2 (150, -175));
            CCollision (new Vector2 (-200, -160), new Vector2 (-140, -160), new Vector2 (-170, -175));

            // Because the level data was created assuming it was relative to the center of the level
            Collision.ForEach (p => p.Move (new Vector2 (500, 500)));

            var light1Trigger = CTrigger (true, false, new Polygon (new Vector2[]
            {
               new Vector2(299, 340),
                new Vector2(358, 341),
                new Vector2(414, 496),
                new Vector2(266, 494)
            }));
            
            var light2Trigger = CTrigger (true, false, new Polygon (new Vector2[]
            {
                new Vector2(620.8718f, 339.5641f),
                new Vector2(679.3333f, 342.641f),
                new Vector2(749.0769f, 496.4872f),
                new Vector2(580.8718f, 495.4615f)
            }));
            
            BindToggleLight (light1Trigger, light1);
            BindToggleLight (light2Trigger, light2);
        }

        private void BindToggleLight (Trigger t, Light l)
        {
            t.Entered += o => l.Enabled = true;
            t.Exited += o => l.Enabled = false;
        }

        public override void Update(GameObject player)
        {
            base.Update (player);
            Lights.SetCam (cam);
        }
    }
}
