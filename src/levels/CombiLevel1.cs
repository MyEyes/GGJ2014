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
            l.Radius = 400;
            //l.Mask = G.Content.Load<Texture2D>("branches/branch1/breakmask");
            l.Mask = G.Content.Load<Texture2D>("breakMask4_Transition");
            //l.Mask = G.Content.Load<Texture2D>("StrangeMask2");
            l.Color = Color.White;
            l.Enabled = true;
            l.Position = new Vector2(1680, 228);
            Blending.AddLight(l);
            //Blending.AmbientColor = Color.White;

            SetTransition(TransitionType.ThresholdRead);

            Trigger trig = Good.CTrigger(true, false, new Polygon(new Vector2[] {
                new Vector2(1423, 183),
                new Vector2(1536, 41),
                new Vector2(2480, 47),
                new Vector2(2559, 308),
                new Vector2(2565, 441),
                new Vector2(1416, 440)
            }));
            trig.Entered += delegate(GameObject obj) { SetTargetInsanity(1.1f); };
            trig.Exited += delegate(GameObject obj) { SetTargetInsanity(-0.1f); };
            
            trig = Good.CTrigger(true, false, new Polygon(new Vector2[]{
                new Vector2(1311, 436),
                new Vector2(1311, 336),
                new Vector2(1414, 337),
                new Vector2(1407, 434)}));
            trig.Exited += delegate(GameObject obj) { SetState(LevelState.Evil); SetTargetInsanity(1.1f); };

            trig = Bad.CTrigger(true, false, new Polygon(new Vector2[]{
                new Vector2(1242, 437),
                new Vector2(1239, 303),
                new Vector2(1103, 296),
                new Vector2(1102, 436)}));
            trig.Exited += delegate(GameObject obj) { SetState(LevelState.Good); SetTargetInsanity(-0.1f); };
             
        }
    }
}
