﻿using System;
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
        public CombiLevel1(ManualCamera2D cam, Player player)
            : base (player, cam, new Level1Good (), new Level1Evil ())
        {
            Light l = new Light();
            l.Radius = 200;
            l.Mask = G.Content.Load<Texture2D>("BreakMask Small 1");
            l.Color = Color.White;
            l.Enabled = true;
            l.Position = new Vector2(3320, 180);
            Blending.AddLight(l);

            Light l2 = new Light();
            l2.Radius = 130;
            l2.Mask = G.Content.Load<Texture2D>("BreakMask Small 2");
            l2.Color = Color.White;
            l2.Enabled = false;
            l2.Position = new Vector2(160, 320);
            Blending.AddLight(l2);
            
            Light l3 = new Light();
            l3.Radius = 800;
            l3.Mask = G.Content.Load<Texture2D>("pillmask");
            l3.Color = Color.White;
            l3.Enabled = false;
            l3.Position = new Vector2(560, 320);
            Blending.AddLight(l3);

            Light l4 = new Light();
            l4.Radius = 800;
            l4.Mask = G.Content.Load<Texture2D>("pillmask_green");
            l4.Color = Color.White;
            l4.Enabled = false;
            l4.Position = new Vector2(560, 320);
            Blending.AddLight(l4);

            Light l5 = new Light();
            l5.Radius = 800;
            l5.Mask = G.Content.Load<Texture2D>("pillmask3");
            l5.Color = Color.White;
            l5.Enabled = false;
            l5.Position = new Vector2(2160, 320);
            Blending.AddLight(l5);

            Light l6 = new Light();
            l6.Radius = 800;
            l6.Mask = G.Content.Load<Texture2D>("pillmask3_green");
            l6.Color = Color.White;
            l6.Enabled = false;
            l6.Position = new Vector2(2160, 320);
            Blending.AddLight(l6);

            Light l7 = new Light();
            l7.Radius = 800;
            l7.Mask = G.Content.Load<Texture2D>("elevatorMask");
            l7.Color = Color.White;
            l7.Enabled = false;
            l7.Position = new Vector2(3660, 320);
            Blending.AddLight(l7);

            Light l8 = new Light();
            l8.Radius = 350;
            l8.Mask = G.Content.Load<Texture2D>("BreakMask Small 4");
            l8.Color = Color.White;
            l8.Enabled = true;
            l8.Position = new Vector2(3050, 360);
            Blending.AddLight(l8);

            Level1Good l1g = Good as Level1Good;

            //Blending.AmbientColor = Color.White;

            SoundHelper.Preload("sounds/monster1");
            SoundHelper.Preload("sounds/ding");
            SoundHelper.Preload("sounds/elevator_openclose");
            SoundHelper.Preload("sounds/Parano_fiesta");
            SoundHelper.Preload("sounds/Parano_parano");
            SoundHelper.Preload("sounds/step1");
            SoundHelper.Preload("sounds/light_flick");
            SoundHelper.SetMusicChannel1("sounds/Parano_fiesta", 0.1f);
            SoundHelper.SetMusicChannel2("sounds/crowd_chatter", 0.1f);

            SetTransition(TransitionType.ThresholdRead);

            //Armchair room transition
            Trigger trig = Good.CTrigger(true, true, new Polygon(new Vector2[] {new Vector2(3170, 433),new Vector2(3176, 350),new Vector2(3271, 353),new Vector2(3259, 431)}));
            trig.Entered += delegate(GameObject obj) { SoundHelper.SetMusicVolume1(0.2f); SoundHelper.SetMusicVolume2(0.4f); SavePosition(); insanityChange = 0.25f / 1000f; SetTargetInsanity(1.1f); TaskHelper.SetDelay(5000, delegate { insanityChange = 0.8f / 1000f; SetTargetInsanity(-0.1f); }); TaskHelper.SetDelay(8000, delegate { l.Enabled = false; l8.Enabled = false; }); };

            //Lights flickering
            trig = Good.CTrigger(true, true, new Polygon(new Vector2[] { new Vector2(1447, 437), new Vector2(1367, 435), new Vector2(1365, 343), new Vector2(1443, 346) }));
            trig.Entered += delegate(GameObject obj)
            {
            SoundHelper.SetMusicVolume1(0.4f);
            SavePosition(); TaskHelper.SetDelay(100, delegate { SoundHelper.SetMusicVolume2(0.3f); l1g.light1.Enabled = false; l1g.light2.Enabled = false; }); TaskHelper.SetDelay(200, delegate { l1g.light1.Enabled = true; l1g.light2.Enabled = true; SoundHelper.PlaySound("sounds/light_flick", 0.2f); });
            TaskHelper.SetDelay(330, delegate { l1g.light1.Enabled = false; l1g.light2.Enabled = false; SoundHelper.PlaySound("sounds/light_flick", 0.2f); }); TaskHelper.SetDelay(470, delegate { l1g.light1.Enabled = true; l1g.light2.Enabled = true; SoundHelper.PlaySound("sounds/light_flick", 0.2f); });
            TaskHelper.SetDelay(800, delegate { l1g.light1.Enabled = false; l1g.light2.Enabled = false; SoundHelper.PlaySound("sounds/light_flick", 0.2f); }); TaskHelper.SetDelay(950, delegate { l1g.light1.Enabled = true; l1g.light2.Enabled = true; SoundHelper.PlaySound("sounds/light_flick", 0.2f); });
            };
            //Lights turning off
            trig = Good.CTrigger(true, true, new Polygon(new Vector2[] { new Vector2(1053, 346), new Vector2(1122, 343), new Vector2(1123, 430), new Vector2(1064, 430) }));
            trig.Entered += delegate(GameObject obj) { SoundHelper.SetMusicVolume1(0.5f); SoundHelper.SetMusicVolume2(0.2f); SavePosition(); l1g.light1.Enabled = false; l1g.light2.Enabled = false; SoundHelper.PlaySound("sounds/light_flick", 0.3f); };
            //Show up stuff around pill
            trig = Good.CTrigger (true, true, new Polygon (new Vector2[] { new Vector2 (653 + 300, 379), new Vector2 (739 + 300, 381), new Vector2 (738 + 300, 435), new Vector2 (662 + 300, 433) }));
            trig.Entered += delegate(GameObject obj) { SoundHelper.SetMusicVolume2(0.1f); SavePosition(); SetTransition(TransitionType.RepeatRead); insanityChange = 0.4f / 1000f; SetTargetInsanity(1.1f); l2.Enabled = true; };
            //Collect pill
            l1g.pill.Interact += delegate(Player obj) { SoundHelper.SetMusicVolume1(0.7f); SoundHelper.SetMusicChannel2("sounds/Parano_parano", 0.7f); SavePosition(); player.DisableInput++; insanity = 0;
            insanityChange = 0.3f / 1000f; l2.Enabled = false; SetTransition(TransitionType.ThresholdRead); SetTargetInsanity(0); player.Controller.ChangeState(PlayerState.Swallow);
            TaskHelper.SetDelay(2000, delegate { player.Controller.ChangeState(PlayerState.Freakout);  SetState(LevelState.Evil); insanityChange = 0.4f / 1000f; SetTargetInsanity(1.1f); l3.Enabled = true; l4.Enabled = true; });
            TaskHelper.SetDelay(4000, delegate { player.DisableInput--; player.Controller.ChangeState(PlayerState.Idle); });
            };

            trig = Good.CTrigger(true, false, new Polygon(new Vector2[] { new Vector2(135, 342), new Vector2(192, 342), new Vector2(183, 433), new Vector2(142, 433) }));
            trig.Entered += delegate(GameObject obj) { l1g.PillCue.visible = true; };
            trig.Exited += delegate(GameObject obj) { l1g.PillCue.visible = false; };

            trig = Bad.CTrigger(true, false, new Polygon(new Vector2[] { new Vector2(3943, 434), new Vector2(3944, 304), new Vector2(4044, 306), new Vector2(4043, 433) }));
            trig.Entered += delegate(GameObject obj) { l1g.ElevatorCue.visible = true; };
            trig.Exited += delegate(GameObject obj) { l1g.ElevatorCue.visible = false; };
            //Expand dark 1
            trig = Bad.CTrigger(true, true, new Polygon(new Vector2[] { new Vector2(1235, 432), new Vector2(1237, 338), new Vector2(1320, 343), new Vector2(1310, 431) }));
            trig.Entered += delegate(GameObject obj) { SavePosition(); insanity = 0.001f; l3.Enabled = false; l4.Enabled = true; l5.Enabled = true; SetTargetInsanity(1.1f); };
            //Expand Dark 2
            trig = Bad.CTrigger(true, true, new Polygon(new Vector2[] { new Vector2(2813, 301), new Vector2(2867, 301), new Vector2(2868, 350), new Vector2(2826, 351) }));
            trig.Entered += delegate(GameObject obj) { insanity = 0.001f; l5.Enabled = false; l6.Enabled = true; l7.Enabled = true; SetTargetInsanity(1.1f); };
            //Spike Kill trigger
            trig = Bad.CTrigger (true, false, new Polygon (new Vector2[] { new Vector2 (1898, 710), new Vector2 (2263, 591), new Vector2 (2259, 638), new Vector2 (1914, 748) }));
            trig.Entered += delegate(GameObject obj) { RestorePosition(); };
            
            //After Spike Save
            trig = Bad.CTrigger(true, true, new Polygon(new Vector2[] { new Vector2(2953, 341), new Vector2(2955, 215), new Vector2(3023, 223), new Vector2(3006, 375) }));
            trig.Entered += delegate(GameObject obj) { SavePosition(); };

            Trigger elevatorTrigger = Bad.CTrigger(true, true, new Polygon(new Vector2[]
            {
                new Vector2 (3921, 421),
                new Vector2 (3919, 236),
                new Vector2 (4071, 236),
                new Vector2 (4070, 418)
            }));
            elevatorTrigger.Entered += delegate(GameObject obj) { l1g.elevator.Enabled = true; };
        }
    }
}
