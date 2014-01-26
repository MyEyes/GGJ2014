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
        Trigger elevatorTrigger;

        public override void Load(ManualCamera2D cam)
        {
            this.cam = cam;

            normalBG1 = G.Content.Load<Texture2D>("branches/branch1/normalBG1");
            normalBG2 = G.Content.Load<Texture2D>("branches/branch1/normalBG2");

            Lights = new LightOverlay(G.Gfx);
            Lights.AmbientColor = new Color(175, 175, 175);

            CCollision(new Vector2(49, 440), new Vector2(4202, 440), new Vector2(4202, 450), new Vector2(47, 450)); // FLOOR
            CCollision(new Vector2(33, 18), new Vector2(47, 18), new Vector2(47, 435), new Vector2(33, 437)); // WALL LEFT
            CCollision(new Vector2(4201, 104), new Vector2(4217, 104), new Vector2(4218, 446), new Vector2(4195, 445)); // WALL RIGHT

            // Chandoliers
            CLight(200, Color.White, G.Content.Load<Texture2D>("branches/branch1/l_round"), new Vector2(246, 205));
            CLight(200, Color.White, G.Content.Load<Texture2D>("branches/branch1/l_round"), new Vector2(836, 196));
            // Hallway Lights
            CLight (91, Color.White, G.Content.Load<Texture2D> ("branches/branch1/l_roundUp"), new Vector2 (3646, 276 - 40));
            CLight (91, Color.White, G.Content.Load<Texture2D> ("branches/branch1/l_roundUp"), new Vector2 (3483, 276 - 40));
            CLight (91, Color.White, G.Content.Load<Texture2D> ("branches/branch1/l_roundUp"), new Vector2 (2781, 281 - 40));
            CLight (91, Color.White, G.Content.Load<Texture2D> ("branches/branch1/l_roundUp"), new Vector2 (2625, 274 - 40));
            CLight (91, Color.White, G.Content.Load<Texture2D> ("branches/branch1/l_roundUp"), new Vector2 (1342, 277 - 40));
            CLight (91, Color.White, G.Content.Load<Texture2D> ("branches/branch1/l_roundUp"), new Vector2 (1185, 277 - 40));
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
        
            elevatorTrigger = CTrigger(true, false, new Polygon(new Vector2[]
            {
                new Vector2 (3921, 421),
                new Vector2 (3919, 236),
                new Vector2 (4071, 236),
                new Vector2 (4070, 418)
            }));

            var elevator = new Elevator(new Vector2(3922, 236), new Polygon[0]);
        }

        public override void Update(GameObject player)
        {
            base.Update(player);
        }

        public override void Update(float dt)
        {
            Lights.SetCam(cam);
        }

        public override void Draw(OSpriteBatch batch)
        {
            batch.Draw(normalBG1, Vector2.Zero, Color.White);
            batch.Draw(normalBG2, new Vector2(normalBG1.Width, 0), Color.White);
        }
    }
}

