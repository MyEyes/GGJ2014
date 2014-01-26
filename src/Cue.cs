using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vest
{
    public class Cue : SpineGameObject
    {
        Texture2D texture;
        public bool visible = false;

        public Cue(Vector2 position)
            : base(position, new Polygon[0])
        {
            texture = G.Content.Load<Texture2D>("X");
        }

        public override void Draw(graphics.OSpriteBatch batch)
        {
            if (visible)
                batch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }

        public override void Update(float dt)
        {
            
        }
    }
}
