using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vest.graphics;

namespace Vest.levels
{
    public class Level1Evil
        : VestLevel
    {
        ManualCamera2D cam;
        Texture2D evilBG1;
        Texture2D evilBG2;

        public override void Load(ManualCamera2D cam, CombiLevel parent)
        {
            this.cam = cam;
            this.parent = parent;

            evilBG1 = G.Content.Load<Texture2D>("branches/branch1/evilBG1");
            evilBG2 = G.Content.Load<Texture2D>("branches/branch1/evilBG2");

            Lights = new LightOverlay(G.Gfx);
            Lights.AmbientColor = new Color(255, 255, 255);

            CTable (new Vector2 (1191, 385), false);
            CTable (new Vector2 (1710, 364), false);
            CTable (new Vector2 (105, 385), false);
            CTable (new Vector2 (3067, 384), false);
            CTable (new Vector2 (3297, 384), false);

            CMonster (new Vector2 (1616, 344), 1503, 1931);
            CMonster (new Vector2 (3508, 400), 3189, 3659);

            CCollision(new Vector2(1981, 450),
            new Vector2(1975, 440),
            new Vector2(34, 440),
            new Vector2(34, 450)); // FLOOR LEFT

            CCollision(new Vector2(2165, 459),
            new Vector2(4085, 436),
            new Vector2(2169, 436)); // FLOOR Right

            CCollision(new Vector2(33, 18), new Vector2(47, 18), new Vector2(47, 435), new Vector2(33, 437)); // WALL LEFT
            CCollision(new Vector2(4201, 104), new Vector2(4217, 104), new Vector2(4218, 446), new Vector2(4195, 445)); // WALL RIGHT
            CCollision(new Vector2(324, 402),
            new Vector2(454, 364),
            new Vector2(481, 434),
            new Vector2(339, 436)); //First stoneblock

            CCollision(new Vector2(381, 345),
            new Vector2(498, 316),
            new Vector2(527, 337),
            new Vector2(517, 348),
            new Vector2(394, 375)); //Second stoneblock

            CCollision(new Vector2(494, 353),
            new Vector2(508, 349),
            new Vector2(518, 412),
            new Vector2(495, 403)); //Third small stoneblock

            CCollision(new Vector2(589, 386), new Vector2(581, 377), new Vector2(520, 411), new Vector2(560, 408)); //Fourth small stoneblock

            CCollision(new Vector2(633, 391), new Vector2(618, 415), new Vector2(589, 352), new Vector2(606, 347)); //Fifth small stoneblock

            CCollision(new Vector2(610, 395), new Vector2(694, 386), new Vector2(706, 420), new Vector2(618, 415)); //Sixth small stoneblock

            CCollision(new Vector2(644, 360),
            new Vector2(655, 389),
            new Vector2(665, 388),
            new Vector2(654, 358)); //Seventh small stoneblock

            CCollision(new Vector2(707, 420),
            new Vector2(707, 436),
            new Vector2(864, 437),
            new Vector2(859, 419)); //Eigth small stoneblock

            CCollision(new Vector2(879, 387),
            new Vector2(893, 384),
            new Vector2(954, 434),
            new Vector2(892, 407)); //Ninth small stoneblock

            CCollision(new Vector2(859, 419),
            new Vector2(943, 390),
            new Vector2(951, 401),
            new Vector2(864, 436)); //Tenth small stoneblock
            //ROOM 1 DONE

            CCollision(new Vector2(1456, 440),
            new Vector2(1525, 422),
            new Vector2(1529, 440)); //Slope 1

            CCollision(new Vector2(1531, 421),
            new Vector2(1545, 407),
            new Vector2(1641, 379),
            new Vector2(1701, 396),
            new Vector2(1737, 418)); //Slope 2

            CCollision(new Vector2(1740, 419),
            new Vector2(1846, 410),
            new Vector2(1885, 440),
            new Vector2(1743, 440)); //Slope 3

            CCollision(new Vector2(2295, 436),
            new Vector2(2416, 409),
            new Vector2(2416, 436)); //Slope 4

            CCollision(new Vector2(2416, 409),
            new Vector2(2464, 387),
            new Vector2(2499, 386),
            new Vector2(2497, 406)); //Slope 5

            CCollision(new Vector2(2502, 387),
            new Vector2(2595, 336),
            new Vector2(2635, 339),
            new Vector2(2630, 384)); //Slope 6

            CCollision (new Vector2 (2634, 339), new Vector2 (2834, 361), new Vector2 (2929, 394), new Vector2 (2630, 384)); // Slope 7-8
            CCollision (new Vector2 (2497, 141), new Vector2 (2905, 143), new Vector2 (2911, 211), new Vector2 (2850, 295), new Vector2 (2598, 229), new Vector2 (2495, 159)); // Ceiling over Slope 7-8

            CCollision(new Vector2(2834, 362),
            new Vector2(2939, 355),
            new Vector2(3060, 436)); //Slope 9

            // STUPID TABLE SLOAP
            /*CCollision(new Vector2(3092, 436),
            new Vector2(3126, 421),
            new Vector2(3159, 421),
            new Vector2(3212, 436)); //Slope 10
            */

            CCollision(new Vector2(3515, 436),
            new Vector2(3542, 416),
            new Vector2(3568, 428)); //Slope 11

            CCollision(new Vector2(3568, 427),
            new Vector2(3597, 423),
            new Vector2(3622, 436),
            new Vector2(3568, 432)); //Slope 12

            CCollision(new Vector2(4085, 433),
            new Vector2(4085, 420),
            new Vector2(4120, 407),
            new Vector2(4123, 430)); //Endblock1

            CCollision(new Vector2(4123, 405),
            new Vector2(4123, 393),
            new Vector2(4157, 375),
            new Vector2(4161, 399)); //Endblock2

            CCollision(new Vector2(4157, 374),
            new Vector2(4150, 343),
            new Vector2(4172, 340),
            new Vector2(4178, 369)); //Endblock3

            CCollision(new Vector2(4173, 336),
            new Vector2(4148, 188),
            new Vector2(4198, 169),
            new Vector2(4195, 334)); //Endblock4

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
            batch.Draw(evilBG1, Vector2.Zero, Color.White);
            batch.Draw(evilBG2, new Vector2(evilBG1.Width, 0), Color.White);

            base.Draw (batch);
        }
    }
}