using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vest.State
{
    public class TestState
        : BaseGameState
    {
        SpriteBatch spriteBatch;
        Texture2D worldTex;
        Texture2D darkTex;
        Texture2D maskTex;

        Effect distortionEffect;
        float strength = 70;
        float strengthparam = 0;
        float time = 0;
        float masked = 0;
        Vector2 cam;
        Vector2 position;
        SpriteFont font;
        MouseState oldMouse;

        public TestState()
            : base (false, true)
        {
        }

        public override void Activate()
        {
            spriteBatch = new SpriteBatch(G.Gfx);
            font = G.Content.Load<SpriteFont>("font");
            distortionEffect = G.Content.Load<Effect>("Blend");
            darkTex = G.Content.Load<Texture2D>("Paranoid World");
            worldTex = G.Content.Load<Texture2D>("Normal World");
            maskTex = G.Content.Load<Texture2D>("mask");
        }

        public override void Update (float dt)
        {
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                strengthparam += 0.1f * (strength - strengthparam);
            }
            else
            {
                strengthparam += 0.01f * (0 - strengthparam);
            }
            position.X += 0.01f * (mouse.X - position.X);
            position.Y += 0.01f * (mouse.Y - position.Y);
            time += dt / 1000f;
            if (mouse.RightButton == ButtonState.Pressed && oldMouse.RightButton == ButtonState.Pressed)
            {
                cam.X -= mouse.X - oldMouse.X;
                cam.Y -= mouse.Y - oldMouse.Y;
            }

            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.W))
                masked += 0.1f;
            if (keyboard.IsKeyDown(Keys.S))
                masked -= 0.1f;
            if (keyboard.IsKeyDown(Keys.D1))
                distortionEffect.CurrentTechnique = distortionEffect.Techniques["Technique1"];
            if (keyboard.IsKeyDown(Keys.D2))
                distortionEffect.CurrentTechnique = distortionEffect.Techniques["Technique2"];
            if (keyboard.IsKeyDown(Keys.D3))
                distortionEffect.CurrentTechnique = distortionEffect.Techniques["Technique3"];
            if (keyboard.IsKeyDown(Keys.D4))
                distortionEffect.CurrentTechnique = distortionEffect.Techniques["Technique4"];
            oldMouse = mouse;
        }

        public override void Draw()
        {
            distortionEffect.Parameters["time"].SetValue(time);
            distortionEffect.Parameters["strength"].SetValue(strengthparam);
            distortionEffect.Parameters["darkWorld"].SetValue(darkTex);
            distortionEffect.Parameters["mask"].SetValue(maskTex);
            distortionEffect.Parameters["masked"].SetValue(masked);
            distortionEffect.Parameters["position"].SetValue(cam + position);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, distortionEffect, Matrix.CreateTranslation(-cam.X, -cam.Y, 0));
            spriteBatch.Draw(worldTex, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Strength: " + strength.ToString(), Vector2.Zero, Color.White);
            spriteBatch.End();
        }
    }
}
