using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vest.graphics;

namespace Vest.levels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    namespace Vest.levels
    {
        public class Branch1
            : VestLevel
        {
            ManualCamera2D cam;
            Texture2D normalBG1;
            Texture2D normalBG2;
            Trigger elevatorTrigger;

            public override void Load (ManualCamera2D cam)
            {
                this.cam = cam;

                normalBG1 = G.Content.Load<Texture2D> ("branches/branch1/normalBG1");
                normalBG2 = G.Content.Load<Texture2D> ("branches/branch1/normalBG2");
                
                Lights = new LightOverlay (G.Gfx);
                Lights.AmbientColor = new Color (255, 255, 255);

                CCollision (new Vector2 (49, 422), new Vector2 (4202, 423), new Vector2 (4202, 434), new Vector2 (47, 437)); // FLOOR
                CCollision (new Vector2(33, 18), new Vector2(47, 18), new Vector2(47, 435), new Vector2(33, 437)); // WALL LEFT
                CCollision (new Vector2(4201, 104), new Vector2(4217, 104), new Vector2(4218, 446), new Vector2(4195, 445)); // WALL RIGHT

                elevatorTrigger = CTrigger (true, false, new Polygon (new Vector2[]
                {
                    new Vector2 (3921, 421),
                    new Vector2 (3919, 236),
                    new Vector2 (4071, 236),
                    new Vector2 (4070, 418)
                }));

                

                var elevator = new Elevator (new Vector2 (3922, 236), new Polygon[0]);


            }

            public override void Update(GameObject player)
            {
                base.Update (player);
            }

            public override void DrawGood (OSpriteBatch batch)
            {
                batch.Draw (normalBG1, Vector2.Zero, Color.White);
                batch.Draw (normalBG2, new Vector2(normalBG1.Width, 0), Color.White);
            }

            public override void DrawEvil (OSpriteBatch batch)
            {
            }
        }
    }

}
