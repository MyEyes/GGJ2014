using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vest.graphics;

namespace Vest.levels
{
    public enum LevelState
    {
        Good,
        Evil
    }

    public class CombiLevel
    {
        public VestLevel Good;
        public VestLevel Bad;
        public LevelState State;
        public RenderTarget2D GoodTarget;
        public RenderTarget2D BadTarget;
        public LightOverlay Blending;
        public Effect BlendEffect;
        public float t;
        public float insanity;

        public CombiLevel(VestLevel Good, VestLevel Bad)
        {
            this.Good = Good;
            this.Bad = Bad;
            State = LevelState.Good;
            GoodTarget = new RenderTarget2D(G.Gfx, G.Gfx.Viewport.Width, G.Gfx.Viewport.Height, false, SurfaceFormat.Rgba1010102, DepthFormat.Depth16);
            BadTarget = new RenderTarget2D(G.Gfx, G.Gfx.Viewport.Width, G.Gfx.Viewport.Height, false, SurfaceFormat.Rgba1010102, DepthFormat.Depth16);
            Blending = new LightOverlay(G.Gfx);
            BlendEffect = G.Content.Load<Effect>("Blend");
        }

        public void SetState(LevelState state)
        {
            State = state;
        }

        public bool IsColliding(GameObject o)
        {
            return State == LevelState.Good ? Good.IsColliding(o) : Bad.IsColliding(o);
        }

        public void Update(float dt)
        {
            t += dt / 1000;
        }

        public void Update(GameObject player)
        {
            if (State == LevelState.Good)
                Good.Update(player);
            else
                Bad.Update(player);
        }

        public void Draw(OSpriteBatch batch, ManualCamera2D cam, Player player)
        {
            Good.Lights.DrawLights();
            G.Gfx.SetRenderTarget(GoodTarget);
            Good.Draw(batch);
            player.Draw (batch);
            Good.Lights.Apply(batch);

            Bad.Lights.DrawLights();
            G.Gfx.SetRenderTarget(BadTarget);
            Bad.Draw(batch);
            player.Draw (batch);
            Bad.Lights.Apply(batch);

            Blending.DrawLights();
            BlendEffect.Parameters["time"].SetValue(t);
            BlendEffect.Parameters["strength"].SetValue(insanity);
            BlendEffect.Parameters["darkWorld"].SetValue(BadTarget);
            BlendEffect.Parameters["mask"].SetValue(Blending.Target);

            batch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, cam.Transformation);
            BlendEffect.CurrentTechnique.Passes[0].Apply();
            batch.Draw(GoodTarget, Vector2.Zero, Color.White);
            batch.End();
        }
    }
}
