using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Vest
{
    public struct Edge
    {
        public Vector2 Start;
        public Vector2 End;
        public Vector2 Normal;
        public Vector2 Dir;
        public float Length;

        public Edge(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
            Dir = End - Start;
            Length = Dir.Length();
            Normal.X = Dir.Y;
            Normal.Y = -Dir.X;
        }
    }
}
