using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vest.graphics;

namespace Vest
{
    public class Table
        : GameObject
    {
        private Texture2D texture;

        public Table (Vector2 position, Polygon[] polygons, bool IsGood)
            : base (position, polygons)
        {
            texture = IsGood
                ? G.Content.Load<Texture2D> ("objects/table_c")
                : G.Content.Load<Texture2D> ("objects/table_pc");

            var poly = PolyFactory.Rect (
                texture.Width / 3,
                0,
                42,
                texture.Height);
            
            poly.Move (position);
            collisionPolys = new[] {poly};
        }

        public override void Update (float dt)
        {
        }

        public override void Draw (OSpriteBatch batch)
        {
            batch.Draw (texture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
        }

        public bool IsCovering (Player player)
        {
            return this.Collides (player);
        }
    }
}
