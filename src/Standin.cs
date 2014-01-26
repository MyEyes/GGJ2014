using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vest.graphics;

namespace Vest
{
    public class Standin
        : GameObject
    {   
        private Texture2D texture;
        private bool flipX;

        public Standin(Vector2 position, String texturePath, bool flipX)
            : base (position, new Polygon[0])
        {
            this.texture = G.Content.Load<Texture2D> (texturePath);
            this.flipX = flipX;
        }

        public override void Update(float dt)
        {
        }

        public override void Draw(OSpriteBatch batch)
        {
            Vector2 pos = position + new Vector2 (0, - texture.Height);
            SpriteEffects effects = flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            
            batch.Draw (texture, pos, null, Color.White, 0f, Vector2.Zero, 1f, effects, 0.6f);
        }
    }
}
