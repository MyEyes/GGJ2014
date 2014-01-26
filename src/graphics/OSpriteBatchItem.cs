using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Otherworld.Utilities;

namespace Vest.graphics
{
    public class SpriteBatchItem
        : BatchableItem
    {
        public Texture2D Texture { get; set; }
        public float Depth { get; set; }

        public VertexPositionColorTexture vertexTL;
        public VertexPositionColorTexture vertexTR;
        public VertexPositionColorTexture vertexBL;
        public VertexPositionColorTexture vertexBR;

        public void Set (float x, float y, float w, float h, Color color, Vector2 texCoordTL, Vector2 texCoordBR)
        {
            vertexTL.Position = new Vector2 (x, y).ToVector3();
            vertexTL.Color = color;
            vertexTL.TextureCoordinate = texCoordTL;

            vertexTR.Position = new Vector2 (x + w, y).ToVector3();
            vertexTR.Color = color;
            vertexTR.TextureCoordinate = new Vector2 (texCoordBR.X, texCoordTL.Y);

            vertexBL.Position = new Vector2 (x, y + h).ToVector3();
            vertexBL.Color = color;
            vertexBL.TextureCoordinate = new Vector2 (texCoordTL.X, texCoordBR.Y);

            vertexBR.Position = new Vector2 (x + w, y + h).ToVector3();
            vertexBR.Color = color;
            vertexBR.TextureCoordinate = texCoordBR;
        }

        public void Set (float x, float y, float dx, float dy, float w, float h, float sin, float cos, Color color, Vector2 texCoordTL, Vector2 texCoordBR)
        {
            vertexTL.Position = new Vector2 (x + dx * cos - dy * sin, y + dx * sin + dy * cos).ToVector3();
            vertexTL.Color = color;
            vertexTL.TextureCoordinate = texCoordTL;

            vertexTR.Position = new Vector2 (x + (dx + w) * cos - dy * sin, y + (dx + w) * sin + dy * cos).ToVector3();
            vertexTR.Color = color;
            vertexTR.TextureCoordinate = new Vector2 (texCoordBR.X, texCoordTL.Y);

            vertexBL.Position = new Vector2 (x + dx * cos - (dy + h) * sin, y + dx * sin + (dy + h) * cos).ToVector3();
            vertexBL.Color = color;
            vertexBL.TextureCoordinate = new Vector2 (texCoordTL.X, texCoordBR.Y);

            vertexBR.Position = new Vector2 (x + (dx + w) * cos - (dy + h) * sin, y + (dx + w) * sin + (dy + h) * cos).ToVector3();
            vertexBR.Color = color;
            vertexBR.TextureCoordinate = texCoordBR;
        }
    }
}
