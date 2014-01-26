using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vest.levels
{
    class CombiLevel1:CombiLevel
    {
        public CombiLevel1(ManualCamera2D cam)
            : base (cam, new Level1Good (), new Level1Evil ())
        {
            Light l = new Light();
            l.Radius = 2000;
            l.Mask = null;//G.Content.Load<Texture2D>("mask");
            l.Color = Color.White;
            l.Enabled = true;
            l.Position = new Vector2(1848, 83 + 200);
            Blending.AddLight(l);

            Trigger trig = Good.CTrigger(true, false, new Polygon(new Vector2[] {
                new Vector2(1423, 183),
                new Vector2(1536, 41),
                new Vector2(2480, 47),
                new Vector2(2559, 308),
                new Vector2(2565, 441),
                new Vector2(1416, 440)
            }));
            trig.Entered += delegate(GameObject obj) { SetTargetInsanity(1); };
            trig.Exited += delegate(GameObject obj) { SetTargetInsanity(0); };
        }
    }
}
