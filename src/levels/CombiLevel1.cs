using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Otherworld.Utilities;

namespace Vest.levels
{
    class CombiLevel1:CombiLevel
    {
        public Light l;
        public CombiLevel1(ManualCamera2D cam)
            : base (cam, new Level1Good (), new Level1Evil ())
        {
            l = new Light();
            l.Radius = 200;
            l.Mask = G.Content.Load<Texture2D>("BreakMask Small 1");
            l.Color = Color.White;
            l.Enabled = true;
            l.Position = new Vector2(3320, 180);
            Blending.AddLight(l);
            Level1Good l1g = Good as Level1Good;
            Blending.AmbientColor = Color.White;

            SetTransition(TransitionType.ThresholdRead);

            Trigger trig = Good.CTrigger(true, true, new Polygon(new Vector2[] {new Vector2(3170, 433),new Vector2(3176, 350),new Vector2(3271, 353),new Vector2(3259, 431)}));
            trig.Entered += delegate(GameObject obj) { SetTargetInsanity(1.1f);};// TaskHelper.SetDelay(3000, delegate { insanityChange = 0.8f / 1000f; SetTargetInsanity(-0.1f); }); TaskHelper.SetDelay(5000, delegate { l.Enabled = false; }); };

            trig = Good.CTrigger(true, true, new Polygon(new Vector2[] { new Vector2(1447, 437), new Vector2(1367, 435), new Vector2(1365, 343), new Vector2(1443, 346) }));
            trig.Entered += delegate(GameObject obj) { TaskHelper.SetDelay(100, delegate { l1g.light1.Enabled = false; l1g.light2.Enabled = false; }); TaskHelper.SetDelay(200, delegate { l1g.light1.Enabled = true; l1g.light2.Enabled = true; });
            TaskHelper.SetDelay(330, delegate { l1g.light1.Enabled = false; l1g.light2.Enabled = false; }); TaskHelper.SetDelay(470, delegate { l1g.light1.Enabled = true; l1g.light2.Enabled = true; });
            TaskHelper.SetDelay(800, delegate { l1g.light1.Enabled = false; l1g.light2.Enabled = false; }); TaskHelper.SetDelay(950, delegate { l1g.light1.Enabled = true; l1g.light2.Enabled = true; });
            };

            trig = Good.CTrigger(true, true, new Polygon(new Vector2[] { new Vector2(1053, 346), new Vector2(1122, 343), new Vector2(1123, 430), new Vector2(1064, 430) }));
            trig.Entered += delegate(GameObject obj) { l1g.light1.Enabled = false; l1g.light2.Enabled = false; l1g.Lights.AmbientColor = new Color(50, 50, 50); TaskHelper.SetDelay(2000, delegate { l1g.Lights.AmbientColor = new Color(170, 170, 170); }); };
        }
    }
}
