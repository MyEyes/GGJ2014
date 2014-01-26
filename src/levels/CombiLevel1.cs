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
            l.Radius = 400;
            //l.Mask = G.Content.Load<Texture2D>("branches/branch1/breakmask");
            //l.Mask = G.Content.Load<Texture2D>("breakMask3_Transition");
            l.Mask = G.Content.Load<Texture2D>("mask");
            l.Color = Color.White;
            l.Enabled = true;
            l.Position = new Vector2(1680, 228);
            Blending.AddLight(l);

            SetTransition(TransitionType.Threshold);

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
