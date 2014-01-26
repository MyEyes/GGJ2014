using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vest.graphics;

namespace Vest.levels
{
    public class Level1Good
        : VestLevel
    {
        ManualCamera2D cam;
        Texture2D normalBG1;
        Texture2D normalBG2;
        public Light light1;
        public Light light2;
        public Table table;
        public Elevator elevator;
        public Pill pill;
        public Player player;
        public Cue ElevatorCue;
        public Cue PillCue;

        public override void Load(Player player, ManualCamera2D cam, CombiLevel parent)
        {
            this.cam = cam;
            this.parent = parent;

            normalBG1 = G.Content.Load<Texture2D>("branches/branch1/normalBG1");
            normalBG2 = G.Content.Load<Texture2D>("branches/branch1/normalBG2");

            Lights = new LightOverlay(G.Gfx);
            Lights.AmbientColor = new Color(175, 175, 175);

            CCollision(new Vector2(49, 440), new Vector2(4202, 440), new Vector2(4202, 450), new Vector2(47, 450)); // FLOOR
            CCollision(new Vector2(33, 18), new Vector2(47, 18), new Vector2(47, 435), new Vector2(33, 437)); // WALL LEFT
            CCollision(new Vector2(4201, 104), new Vector2(4217, 104), new Vector2(4218, 446), new Vector2(4195, 445)); // WALL RIGHT 

            CTable (new Vector2 (101, 384), true);
            CTable (new Vector2 (336, 384), true);
            CTable (new Vector2 (567, 385), true);
            CTable (new Vector2 (798, 385), true);
            CTable (new Vector2 (1191, 385), true);
            CTable (new Vector2 (3067, 384), true);

            // First room
            CStandin (new Vector2 (3451, 436), "chars/altperson04");
            CStandin (new Vector2 (3490, 436), "chars/girl_02");
            CStandin (new Vector2 (3536, 436), "chars/altperson02", true);
            
            // First lounge room
            CStandin (new Vector2 (3000, 436), "chars/altperson_sit_01");
            CStandin (new Vector2 (3215, 436), "chars/altperson_sit_02", true);

            // Dining room
            CStandin (new Vector2 (296, 436), "chars/altperson_sit_01");
            CStandin (new Vector2 (453, 436), "chars/altperson_sit_02", true);
            CStandin (new Vector2 (756, 436), "chars/altperson_sit_02");
            CStandin (new Vector2 (919, 436), "chars/altperson_sit_01", true);
            
            // Party table room
            CStandin (new Vector2 (1131, 436), "chars/girl_01");
            CStandin (new Vector2 (1283, 436), "chars/altperson03", true);
            
            // Dancing room
            CStandin (new Vector2 (2380 - 100, 436), "chars/altperson01", true);
            CStandin (new Vector2 (2321 - 100, 436), "chars/memory");
            CStandin (new Vector2 (1846 - 65, 436), "chars/jason");
            CStandin (new Vector2 (1906 - 80, 436), "chars/sean");
            CStandin (new Vector2 (1971 - 90, 436), "chars/kevin");
            CStandin (new Vector2 (2061 - 137, 436), "chars/nils");

            // Chandoliers
            CLight(200, Color.White, G.Content.Load<Texture2D>("branches/branch1/l_round"), new Vector2(246, 205));
            CLight(200, Color.White, G.Content.Load<Texture2D>("branches/branch1/l_round"), new Vector2(836, 196));
            // Hallway Lights
            CLight (91, Color.White, G.Content.Load<Texture2D> ("branches/branch1/l_roundUp"), new Vector2 (3646, 276 - 40));
            CLight(91, Color.White, G.Content.Load<Texture2D>("branches/branch1/l_roundUp"), new Vector2(3483, 276 - 40));
            CLight (91, Color.White, G.Content.Load<Texture2D> ("branches/branch1/l_roundUp"), new Vector2 (2781, 281 - 40));
            CLight (91, Color.White, G.Content.Load<Texture2D> ("branches/branch1/l_roundUp"), new Vector2 (2625, 274 - 40));
            light1 = CLight (91, Color.White, G.Content.Load<Texture2D> ("branches/branch1/l_roundUp"), new Vector2 (1342, 277 - 40));
            light2 = CLight(91, Color.White, G.Content.Load<Texture2D>("branches/branch1/l_roundUp"), new Vector2(1185, 277 - 40));
            // Party Lights (Down)
            CLight (200, Color.Orange, G.Content.Load<Texture2D> ("branches/branch1/l_dir"), new Vector2 (1848, 83 + 200));
            CLight (200, Color.Red, G.Content.Load<Texture2D> ("branches/branch1/l_dir"), new Vector2 (1957, 83 + 200));
            CLight (200, Color.Red, G.Content.Load<Texture2D> ("branches/branch1/l_dir"), new Vector2 (2058, 83 + 200));
            CLight (200, Color.Orange, G.Content.Load<Texture2D> ("branches/branch1/l_dir"), new Vector2 (2111, 83 + 200));
            // Party Lights (Forward)
            CLight (50, Color.Red, G.Content.Load<Texture2D> ("branches/branch1/l_roundDown"), new Vector2 (1906, 103 + 20));
            CLight (50, Color.Orange, G.Content.Load<Texture2D> ("branches/branch1/l_roundDown"), new Vector2 (2005 + 3, 103 + 20));
            // Lamp
            CLight (90, Color.White, G.Content.Load<Texture2D> ("branches/branch1/l_roundDown"), new Vector2 (3130, 390));
            // Elevator Lights
            CLight (91, Color.White, G.Content.Load<Texture2D> ("branches/branch1/l_roundUp"), new Vector2(3845, 262-68));
            CLight (91, Color.White, G.Content.Load<Texture2D> ("branches/branch1/l_roundUp"), new Vector2 (4146, 262-68));

            GameObjects.Add(elevator = new Elevator(player,new Vector2(3995, -100), new Vector2(3995, 436), new Polygon[] { new Polygon(new Vector2[] { new Vector2(-50, -100), new Vector2(-50, 200), new Vector2(50, 200), new Vector2(50, -100) }) }));
            GameObjects.Add(pill = new Pill(new Vector2(160, 373)));
            GameObjects.Add(ElevatorCue = new Cue(new Vector2(3985, 270)));
            GameObjects.Add(PillCue = new Cue(new Vector2(155, 352)));
        }

        public override void Update(GameObject player)
        {
            base.Update(player);
        }

        public override void Update(float dt)
        {
            Lights.SetCam(cam);
            base.Update (dt);
        }

        public override void Draw(OSpriteBatch batch)
        {
            batch.Draw(normalBG1, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            batch.Draw(normalBG2, new Vector2(normalBG1.Width, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

            base.Draw (batch);
        }
    }
}

