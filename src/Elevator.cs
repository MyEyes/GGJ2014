using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Vest.graphics;

namespace Vest
{
    public class Elevator
        : GameObject
    {
        public Elevator (Vector2 position, Polygon[] polygons)
            : base (position, polygons)
        {
        }

        public void Interact (Player player)
        {
            
        }

        public override void Update (float dt)
        {    
        }

        public override void Draw (OSpriteBatch batch)
        {
            // Draw background
            // Draw player
            // Draw spine doors
        }
    }
}
