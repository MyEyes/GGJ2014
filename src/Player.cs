using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vest
{
    public class Player
        : PhysicsSpineGameObject
    {

        public int DisableInput = 0;
        public LookDir LookDir = LookDir.Right;

        public Player (Vector2 position, Polygon[] polygons)
            : base(position, polygons)
        {
        }
    }
}
