using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Otherworld.Utilities
{
    public class VarTracker
    {
        private const int PADDING = 5;
        private readonly List<Tuple<String, Func<String>>> tracked
            = new List<Tuple<string, Func<string>>> ();

        public void Track(String name, Func<String> value)
        {
            tracked.Add (new Tuple<string, Func<string>> (name, value));
        }

        public void Untrack(String name)
        {
            tracked.RemoveAll (v => v.Item1 == name);
        }

        public void Draw(SpriteBatch batch, SpriteFont font)
        {
            Vector2 textPos = new Vector2 (PADDING, PADDING);

            batch.Begin ();
            foreach (var var in tracked)
            {
                String text = String.Format ("{0}: {1}", var.Item1, var.Item2 ());
                batch.DrawString (font, text, textPos, Color.Yellow);
                textPos.Y += PADDING + font.LineSpacing;
            }
            batch.End ();
        }
    }
}
