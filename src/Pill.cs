using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vest
{
    public class Pill : SpineGameObject
    {
        Texture2D texture;
        bool edible = true;

        public Pill(Vector2 position)
            : base(position, new Polygon[] { new Polygon(new Vector2[] {new Vector2(-30,30),new Vector2(30,30), new Vector2(30,-30), new Vector2(-30,-30) }) })
        {
            texture = G.Content.Load<Texture2D>("objects/pill");
        }
        
        public void Activate(Player player)
        {
            if (edible)
            {
                Interact.Invoke(player);
            }
            edible = false;
        }

        public event Action<Player> Interact;

        public override void Draw(graphics.OSpriteBatch batch)
        {
            batch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
        }

        public override void Update(float dt)
        {
            
        }
    }
}
