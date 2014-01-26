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
            l.Radius = 100;
            //l.Mask = G.Content.Load<Texture2D>("branches/branch1/breakmask");
            l.Mask = G.Content.Load<Texture2D>("default");
            l.Color = Color.White;
            l.Enabled = true;
            l.Position = new Vector2(2080, 428);
            Blending.AddLight(l);

            SetTransition(TransitionType.ThresholdRead);

            Trigger trig = Good.CTrigger(true, false, new Polygon(new Vector2[] { new Vector2(1898, 83), new Vector2(1848, 83), new Vector2(1848, 383), new Vector2(1898, 383) }));
            trig.Entered += delegate(GameObject obj) { SetTargetInsanity(1); };
            trig.Exited += delegate(GameObject obj) { SetTargetInsanity(0); };
        }
    }
}
