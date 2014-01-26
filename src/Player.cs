using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace Vest
{
    public class Player
        : PhysicsSpineGameObject
    {
        public const float PLAYER_HIDE_DEPTH = 0.7f;
        public const float PLAYER_DEPTH = 0.9f;

        public int DisableInput = 0;
        public bool IsHiding = false;
        public LookDir LookDir = LookDir.Right;
        public PlayerController Controller;

        public Player (Vector2 position, Polygon[] polygons)
            : base(position, polygons)
        {
        }

        public static Polygon[] CROUCH_COLLISION
        {
            get {
                return new Polygon[] {new Polygon (new Vector2[] {
                    new Vector2(20, 0),
                    new Vector2 (-20, 0),
                    new Vector2 (-20, -50),
                    new Vector2(20, -50)
            })};}
        }

        public static Polygon[] STAND_COLLISION
        {
            get {
                return new Polygon[] {new Polygon (new Vector2[] {
                    new Vector2 (20, 0),
                    new Vector2 (-20, 0),
                    new Vector2 (-20, -140),
                    new Vector2 (20, -140)
            })};}
        }

        public void SetCollision (Polygon[] poly)
        {
            poly[0].Move (position);
            collisionPolys = poly;
        }
    }
}
