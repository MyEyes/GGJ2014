using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vest.graphics;

namespace Vest
{
    public class LightOverlay
    {
        public RenderTarget2D Target;
        public List<Light> Lights;
        public Color AmbientColor;
        Effect _lightEffect;
        Texture2D defaultTex;
        BlendState multiplicativeBlend;
        BlendState drawBlend;

        public LightOverlay(GraphicsDevice device)
        {
            Target = new RenderTarget2D(device, device.Viewport.Width, device.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            Lights = new List<Light>();
            _lightEffect = G.Content.Load<Effect>("Light");
            _lightEffect.Parameters["Projection"].SetValue(Matrix.CreateTranslation(-device.Viewport.Width / 2 - 0.5f, -device.Viewport.Height / 2 - 0.5f, 0) * Matrix.CreateScale(2 / ((float)device.Viewport.Width), -2 / ((float)device.Viewport.Height), 1));
            _lightEffect.Parameters["World"].SetValue(Matrix.Identity);
            defaultTex = G.Content.Load<Texture2D>("default");

            multiplicativeBlend = new BlendState();
            multiplicativeBlend.ColorWriteChannels = ColorWriteChannels.All;
            multiplicativeBlend.ColorBlendFunction = BlendFunction.Add;
            multiplicativeBlend.ColorDestinationBlend = Blend.SourceColor;
            multiplicativeBlend.ColorSourceBlend = Blend.Zero;
            drawBlend = BlendState.Additive;
        }

        public void SetCam(ManualCamera2D cam)
        {
            _lightEffect.Parameters["View"].SetValue(cam.Transformation);
        }

        public void SetBlendMode()
        {
            _lightEffect.CurrentTechnique = _lightEffect.Techniques["BlendMask"];
        }

        public void AddLight(Light light)
        {
            for (int x = 0; x < Lights.Count; x++)
            {
                if (Lights[x].Mask == light.Mask)
                {
                    Lights.Insert(x, light);
                    return;
                }
            }
            Lights.Add(light);
        }

        public void DrawLights() 
        {
            var enabledLights = Lights
                .Where(l => l.Enabled)
                .ToList();

            G.Gfx.SetRenderTarget(Target);
            G.Gfx.Clear(AmbientColor);
            G.Gfx.BlendState = drawBlend;
            G.Gfx.RasterizerState = RasterizerState.CullNone;

            if (enabledLights.Count < 1)
            {
                G.Gfx.SetRenderTarget(null);
                return;
            }

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[enabledLights.Count * 6];
            for (int x = 0, v = 0; x < enabledLights.Count; x++, v += 6)
            {
                vertices[v]  =  new VertexPositionColorTexture(new Vector3(enabledLights[x].Position.X - enabledLights[x].Radius, enabledLights[x].Position.Y - enabledLights[x].Radius, enabledLights[x].Radius), enabledLights[x].Color, new Vector2(0, 0));
                vertices[v+1] = new VertexPositionColorTexture(new Vector3(enabledLights[x].Position.X - enabledLights[x].Radius, enabledLights[x].Position.Y + enabledLights[x].Radius, enabledLights[x].Radius), enabledLights[x].Color, new Vector2(0, 1));
                vertices[v+2] = new VertexPositionColorTexture(new Vector3(enabledLights[x].Position.X + enabledLights[x].Radius, enabledLights[x].Position.Y + enabledLights[x].Radius, enabledLights[x].Radius), enabledLights[x].Color, new Vector2(1, 1));

                vertices[v+3] = new VertexPositionColorTexture(new Vector3(enabledLights[x].Position.X - enabledLights[x].Radius, enabledLights[x].Position.Y - enabledLights[x].Radius, enabledLights[x].Radius), enabledLights[x].Color, new Vector2(0, 0));
                vertices[v+4] = new VertexPositionColorTexture(new Vector3(enabledLights[x].Position.X + enabledLights[x].Radius, enabledLights[x].Position.Y + enabledLights[x].Radius, enabledLights[x].Radius), enabledLights[x].Color, new Vector2(1, 1));
                vertices[v+5] = new VertexPositionColorTexture(new Vector3(enabledLights[x].Position.X + enabledLights[x].Radius, enabledLights[x].Position.Y - enabledLights[x].Radius, enabledLights[x].Radius), enabledLights[x].Color, new Vector2(1, 0));
            }
            int count=0;
            int start=0;
            Texture2D lastTex = enabledLights[0].Mask;

            for(int x=0; x < enabledLights.Count; x++)
            {
                if (enabledLights[x].Mask != lastTex)
                {
                    _lightEffect.Parameters["Texture"].SetValue (lastTex ?? defaultTex);
                    _lightEffect.CurrentTechnique.Passes[0].Apply();
                    G.Gfx.DrawUserPrimitives<VertexPositionColorTexture> (PrimitiveType.TriangleList, vertices, start, count);
                    start = 6 * x;
                    count = 0;
                    lastTex = enabledLights[x].Mask;
                }
                
                count += 2;

                if (x == enabledLights.Count - 1)
                {
                    _lightEffect.Parameters["Texture"].SetValue(enabledLights[x].Mask ?? defaultTex);
                    _lightEffect.CurrentTechnique.Passes[0].Apply();
                    G.Gfx.DrawUserPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertices, start, count);
                }
            }
            G.Gfx.SetRenderTarget(null);
        }

        public void Apply(OSpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Immediate, multiplicativeBlend, Matrix.Identity);
            batch.Draw(Target, Vector2.Zero, Color.White);
            batch.End();
        }
    }
}
