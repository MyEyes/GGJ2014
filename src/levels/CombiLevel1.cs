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
        public CombiLevel1(ManualCamera2D cam):base(cam, new Branch1Evil(), new Branch1())
        {
            Light l = new Light();
            l.Radius = 2000;
            l.Mask = null;//G.Content.Load<Texture2D>("mask");
            l.Color = Color.White;
            l.Enabled = true;
            l.Position = new Vector2(1848, 83 + 200);
            Blending.AddLight(l);

            Trigger trig = Good.CTrigger(true, false, new Polygon(new Vector2[] { new Vector2(1898, 83), new Vector2(1848, 83), new Vector2(1848, 383), new Vector2(1898, 383) }));
            trig.Entered += delegate(GameObject obj) { SetTargetInsanity(1); };
            trig.Exited += delegate(GameObject obj) { SetTargetInsanity(0); };
        }
    }
}
