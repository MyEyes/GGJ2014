using System;
using System.Net.Mime;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Otherworld;
using Spine;
using SpriteBatcher = Vest.graphics.SpriteBatcher;
using SpriteBatchItem = Vest.graphics.SpriteBatchItem;

namespace Vest.graphics
{
    public class OSpriteBatch
    {
        private SpriteSortMode sortMode;
        private SpriteBatcher batcher;
        private GraphicsDevice device;
        private BasicEffect effect;
        private RasterizerState rasterState;
        private BlendState blendMode;
        
        // Skeleton Renderer
        float[] vertices = new float[8];

        public OSpriteBatch (GraphicsDevice graphicsDevice)
        {
            Otherworld.Assert.IsNotNull (graphicsDevice);

            device = graphicsDevice;
            batcher = new SpriteBatcher();

            effect = new BasicEffect (device);
            effect.World = Matrix.Identity;
            effect.View = Matrix.CreateLookAt (new Vector3 (0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up);
            effect.TextureEnabled = true;
            effect.VertexColorEnabled = true;
            
            rasterState = new RasterizerState();
            rasterState.CullMode = CullMode.None;
        }

        public void Begin (SpriteSortMode sortMode, BlendState blendMode, Matrix worldMatrix)
        {
            this.sortMode = sortMode;
            this.blendMode = blendMode;

            effect.Projection = Matrix.CreateOrthographicOffCenter (0, device.Viewport.Width, device.Viewport.Height, 0, 1, 0);
            effect.World = worldMatrix;
        }

        public void End()
        {
            device.BlendState = blendMode;
            device.RasterizerState = rasterState;
            
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply ();
                batcher.DrawBatch (sortMode, device);
            }
        }

        public void Draw (Skeleton skeleton, float depth)
        {
            List<Slot> drawOrder = skeleton.DrawOrder;
            float x = skeleton.X, y = skeleton.Y;
            float skeletonR = skeleton.R, skeletonG = skeleton.G, skeletonB = skeleton.B, skeletonA = skeleton.A;
            for (int i = 0, n = drawOrder.Count; i < n; i++)
            {
                Slot slot = drawOrder[i];
                RegionAttachment regionAttachment = slot.Attachment as RegionAttachment;
                if (regionAttachment != null)
                {
                    BlendState blend = slot.Data.AdditiveBlending ? BlendState.Additive : device.BlendState;
                    if (device.BlendState != blend)
                    {
                        End ();
                        device.BlendState = blend;
                    }

                    SpriteBatchItem item = batcher.CreateBatchItem();
                    AtlasRegion region = (AtlasRegion)regionAttachment.RendererObject;

                    item.Depth = depth;
                    item.Texture = (Texture2D)region.page.rendererObject;

                    Color color;
                    float a = skeletonA * slot.A;
                    
                    if (device.BlendState == BlendState.AlphaBlend)
                        color = new Color (skeletonR * slot.R * a, skeletonG * slot.G * a, skeletonB * slot.B * a, a); 
                    else
                        color = new Color (skeletonR * slot.R, skeletonG * slot.G, skeletonB * slot.B, a);

                    item.vertexTL.Color = color;
                    item.vertexBL.Color = color;
                    item.vertexBR.Color = color;
                    item.vertexTR.Color = color;

                    float[] vertices = this.vertices;
                    regionAttachment.ComputeWorldVertices (x, y, slot.Bone, vertices);
                    item.vertexTL.Position.X = vertices[RegionAttachment.X1];
                    item.vertexTL.Position.Y = vertices[RegionAttachment.Y1];
                    item.vertexTL.Position.Z = 0;
                    item.vertexBL.Position.X = vertices[RegionAttachment.X2];
                    item.vertexBL.Position.Y = vertices[RegionAttachment.Y2];
                    item.vertexBL.Position.Z = 0;
                    item.vertexBR.Position.X = vertices[RegionAttachment.X3];
                    item.vertexBR.Position.Y = vertices[RegionAttachment.Y3];
                    item.vertexBR.Position.Z = 0;
                    item.vertexTR.Position.X = vertices[RegionAttachment.X4];
                    item.vertexTR.Position.Y = vertices[RegionAttachment.Y4];
                    item.vertexTR.Position.Z = 0;

                    float[] uvs = regionAttachment.UVs;
                    item.vertexTL.TextureCoordinate.X = uvs[RegionAttachment.X1];
                    item.vertexTL.TextureCoordinate.Y = uvs[RegionAttachment.Y1];
                    item.vertexBL.TextureCoordinate.X = uvs[RegionAttachment.X2];
                    item.vertexBL.TextureCoordinate.Y = uvs[RegionAttachment.Y2];
                    item.vertexBR.TextureCoordinate.X = uvs[RegionAttachment.X3];
                    item.vertexBR.TextureCoordinate.Y = uvs[RegionAttachment.Y3];
                    item.vertexTR.TextureCoordinate.X = uvs[RegionAttachment.X4];
                    item.vertexTR.TextureCoordinate.Y = uvs[RegionAttachment.Y4];
                }
            }
        }

        public void Draw (
            Texture2D texture,
            Vector2 position,
            Nullable<Rectangle> sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effect,
            float depth)
        {
            Otherworld.Assert.IsNotNull (texture);

            SpriteBatchItem item = batcher.CreateBatchItem();

            item.Depth = depth;
            item.Texture = texture;

            Rectangle rect;
            if (sourceRectangle.HasValue)
                rect = sourceRectangle.Value;
            else
                rect = new Rectangle (0, 0, texture.Width, texture.Height);

            /*public Vector2 GetTextureCoord ( int x, int y )
            {
                textureWidth = (int)(texture.PixelsWide/scale);
                textureHeight = (int)(texture.PixelsHigh/scale);
                texWidthRatio = 1.0f / (float)textureWidth;
                texHeightRatio = 1.0f / (float)textureHeight;

             * u = x * 1/width/1;
                return new Vector2 (
             *          x * (1 / (width / 1),
             *          y * (1 / (height / 1));
            }*/

            Vector2 texCoordTL = GetTextureCoord (texture, rect.X, rect.Y);
            Vector2 texCoordBR = GetTextureCoord (texture, rect.X + rect.Width, rect.Y + rect.Height);

            if (effect == SpriteEffects.FlipVertically)
            {
                float temp = texCoordBR.Y;
                texCoordBR.Y = texCoordTL.Y;
                texCoordTL.Y = temp;
            }
            else if (effect == SpriteEffects.FlipHorizontally)
            {
                float temp = texCoordBR.X;
                texCoordBR.X = texCoordTL.X;
                texCoordTL.X = temp;
            }

            item.Set (
                position.X,
                position.Y,
                -origin.X * scale.X,
                -origin.Y * scale.Y,
                rect.Width * scale.X,
                rect.Height * scale.Y,
                (float)Math.Sin (rotation),
                (float)Math.Cos (rotation),
                color,
                texCoordTL,
                texCoordBR);
        }

        public void Draw (
            Texture2D texture,
            Vector2 position,
            Nullable<Rectangle> sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effect,
            float depth)
        {
            Otherworld.Assert.IsNotNull (texture);

            SpriteBatchItem item = batcher.CreateBatchItem();

            item.Depth = depth;
            item.Texture = texture;

            Rectangle rect;
            if (sourceRectangle.HasValue)
                rect = sourceRectangle.Value;
            else
                rect = new Rectangle (0, 0, texture.Width, texture.Height);

            Vector2 texCoordTL = GetTextureCoord (texture, rect.X, rect.Y);
            Vector2 texCoordBR = GetTextureCoord (texture, rect.X + rect.Width, rect.Y + rect.Height);

            if (effect == SpriteEffects.FlipVertically)
            {
                float temp = texCoordBR.Y;
                texCoordBR.Y = texCoordTL.Y;
                texCoordTL.Y = temp;
            }
            else if (effect == SpriteEffects.FlipHorizontally)
            {
                float temp = texCoordBR.X;
                texCoordBR.X = texCoordTL.X;
                texCoordTL.X = temp;
            }

            item.Set (
                position.X,
                position.Y,
                -origin.X * scale,
                -origin.Y * scale,
                rect.Width * scale,
                rect.Height * scale,
                (float)Math.Sin (rotation),
                (float)Math.Cos (rotation),
                color,
                texCoordTL,
                texCoordBR);
        }

        public void Draw (
            Texture2D texture,
            Rectangle destinationRectangle,
            Nullable<Rectangle> sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            SpriteEffects effect,
            float depth)
        {
            Otherworld.Assert.IsNotNull (texture);

            SpriteBatchItem item = batcher.CreateBatchItem();

            item.Depth = depth;
            item.Texture = texture;

            Rectangle rect;
            if (sourceRectangle.HasValue)
                rect = sourceRectangle.Value;
            else
                rect = new Rectangle (0, 0, texture.Width, texture.Height);

            Vector2 texCoordTL = GetTextureCoord (texture, rect.X, rect.Y);
            Vector2 texCoordBR = GetTextureCoord (texture, rect.X + rect.Width, rect.Y + rect.Height);

            if (effect == SpriteEffects.FlipVertically)
            {
                float temp = texCoordBR.Y;
                texCoordBR.Y = texCoordTL.Y;
                texCoordTL.Y = temp;
            }
            else if (effect == SpriteEffects.FlipHorizontally)
            {
                float temp = texCoordBR.X;
                texCoordBR.X = texCoordTL.X;
                texCoordTL.X = temp;
            }

            item.Set (
                destinationRectangle.X,
                destinationRectangle.Y,
                -origin.X,
                -origin.Y,
                destinationRectangle.Width,
                destinationRectangle.Height,
                (float)Math.Sin (rotation),
                (float)Math.Cos (rotation),
                color,
                texCoordTL,
                texCoordBR);
        }

        public void Draw (Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            Otherworld.Assert.IsNotNull (texture);

            SpriteBatchItem item = batcher.CreateBatchItem();

            item.Depth = 0.0f;
            item.Texture = texture;

            Rectangle rect;
            if (sourceRectangle.HasValue)
                rect = sourceRectangle.Value;
            else
                rect = new Rectangle (0, 0, texture.Width, texture.Height);

            Vector2 texCoordTL = GetTextureCoord (texture, rect.X, rect.Y);
            Vector2 texCoordBR = GetTextureCoord (texture, rect.X + rect.Width, rect.Y + rect.Height);

            item.Set (
                position.X,
                position.Y,
                rect.Width,
                rect.Height,
                color,
                texCoordTL,
                texCoordBR);
        }

        public void Draw (Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            Otherworld.Assert.IsNotNull (texture);

            SpriteBatchItem item = batcher.CreateBatchItem();

            item.Depth = 0.0f;
            item.Texture = texture;

            Rectangle rect;
            if (sourceRectangle.HasValue)
                rect = sourceRectangle.Value;
            else
                rect = new Rectangle (0, 0, texture.Width, texture.Height);

            Vector2 texCoordTL = GetTextureCoord (texture, rect.X, rect.Y);
            Vector2 texCoordBR = GetTextureCoord (texture, rect.X + rect.Width, rect.Y + rect.Height);

            item.Set (
                destinationRectangle.X,
                destinationRectangle.Y,
                destinationRectangle.Width,
                destinationRectangle.Height,
                color,
                texCoordTL,
                texCoordBR);
        }

        public void Draw (
            Texture2D texture,
            Vector2 position,
            Color color)
        {
            Otherworld.Assert.IsNotNull (texture);

            SpriteBatchItem item = batcher.CreateBatchItem();

            item.Depth = 0;
            item.Texture = texture;

            Rectangle rect = new Rectangle (0, 0, texture.Width, texture.Height);

            Vector2 texCoordTL = GetTextureCoord (texture, rect.X, rect.Y);
            Vector2 texCoordBR = GetTextureCoord (texture, rect.X + rect.Width, rect.Y + rect.Height);

            item.Set (
                position.X,
                position.Y,
                rect.Width,
                rect.Height,
                color,
                texCoordTL,
                texCoordBR);
        }

        public void Draw (Texture2D texture, Rectangle rectangle, Color color)
        {
            Otherworld.Assert.IsNotNull (texture);

            SpriteBatchItem item = batcher.CreateBatchItem();

            item.Depth = 0;
            item.Texture = texture;

            Vector2 texCoordTL = GetTextureCoord (texture, 0, 0);
            Vector2 texCoordBR = GetTextureCoord (texture, texture.Width, texture.Height);

            item.Set (
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height,
                color,
                texCoordTL,
                texCoordBR);
        }

        private Vector2 GetTextureCoord (Texture2D tex, int x, int y)
        {
            return new Vector2 (x / tex.Width, y / tex.Height);
        }
    }
}
