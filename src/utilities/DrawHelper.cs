using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Vest.utilities
{
    public class DrawHelper
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
            be.Projection = Matrix.CreateTranslation(-device.Viewport.Width / 2 - 0.5f, -device.Viewport.Height / 2 - 0.5f, 0) * Matrix.CreateScale(2 / ((float)device.Viewport.Width), -2 / ((float)device.Viewport.Height), 1); ;
        }

        public void DrawPolys(List<Polygon> polygons, Matrix View, Color color)
        {
            DrawPolys (polygons.ToArray(), View, color);
        }

        public void DrawPolys(Polygon[] polygons, Matrix View, Color color)
        {
            int len = 0;
            for (int x = 0; x < polygons.Length; x++)
            {
                len += polygons[x].Edges.Length * 2;
            }
            be.View = View;
            be.CurrentTechnique.Passes[0].Apply();
            VertexPositionColor[] vertices = new VertexPositionColor[len];
            int count = 0;
            for (int x = 0; x < polygons.Length; x++)
            {
                for (int y = 0; y < polygons[x].Edges.Length; y++)
                {
                    vertices[count] = new VertexPositionColor(new Vector3(polygons[x].Edges[y].Start, 0), color);
                    vertices[count + 1] = new VertexPositionColor(new Vector3(polygons[x].Edges[y].End, 0), color);
                    count += 2;
                }
            }
            if (len < 2)
                return;
            device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, len/2, VertexPositionColor.VertexDeclaration);
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
