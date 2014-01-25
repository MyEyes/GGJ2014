using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vest.utilities
{
    class DrawHelper
    {
        BasicEffect be;
        GraphicsDevice device;

        public DrawHelper(GraphicsDevice device)
        {
            this.device = device;
            be = new BasicEffect(device);
            be.VertexColorEnabled = true;
            be.TextureEnabled = false;
            be.World = Matrix.Identity;
            be.Projection = Matrix.Identity;
        }

        public void DrawLines(Vector2[] positions, Vector2 relative, Color color)
        {
            be.View = Matrix.CreateTranslation(-relative.X, -relative.Y, 0);
            be.CurrentTechnique.Passes[0].Apply();
            VertexPositionColor[] vertices = new VertexPositionColor[positions.Length];
            for (int x = 0; x < positions.Length; x++)
            {
                vertices[x] = new VertexPositionColor(new Vector3(positions[x], 0), color);
            }
            device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, positions.Length, VertexPositionColor.VertexDeclaration);
        }
    }
}
