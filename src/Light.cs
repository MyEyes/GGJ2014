using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vest
{
    struct Light
    {
        public Vector2 Position;
        public float Radius;
        public Color Color;
        public Texture2D Mask;
    }
}
