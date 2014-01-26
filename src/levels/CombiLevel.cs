using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vest.graphics;
using Vest.utilities;

namespace Vest.levels
{
    public enum LevelState
    {
        Good,
        Evil
    }

    public enum TransitionType
    {
        Smooth,
        Threshold,
        ThresholdRead,
        Thresholdsmooth
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
        SpriteBatch internalBatch;
        public float t;
        public float insanity;
        public float targetInsanity;
        const float insanityChange = 0.5f / 1000f;

        public CombiLevel (ManualCamera2D cam, VestLevel Good, VestLevel Bad)
        {
            this.Good = Good;
            Good.Load(cam);
            this.Bad = Bad;
            Bad.Load(cam);
            State = LevelState.Good;
            GoodTarget = new RenderTarget2D(G.Gfx, G.Gfx.Viewport.Width, G.Gfx.Viewport.Height, false, SurfaceFormat.Rgba1010102, DepthFormat.Depth16);
            BadTarget = new RenderTarget2D(G.Gfx, G.Gfx.Viewport.Width, G.Gfx.Viewport.Height, false, SurfaceFormat.Rgba1010102, DepthFormat.Depth16);
            Blending = new LightOverlay(G.Gfx);
            BlendEffect = G.Content.Load<Effect>("Blend");
            SetTransition(TransitionType.Smooth);
            internalBatch = new SpriteBatch(G.Gfx);
            Blending.SetBlendMode();
        }

        public void SetTransition(TransitionType t)
        {
            switch (t)
            {
                case TransitionType.Smooth: BlendEffect.CurrentTechnique = BlendEffect.Techniques["SmoothBlend"]; break;
                case TransitionType.Threshold: BlendEffect.CurrentTechnique = BlendEffect.Techniques["ActivateBlend"]; break;
                case TransitionType.ThresholdRead: BlendEffect.CurrentTechnique = BlendEffect.Techniques["ActivateReadBlend"]; break;
                case TransitionType.Thresholdsmooth: BlendEffect.CurrentTechnique = BlendEffect.Techniques["SmoothActivateBlend"]; break;
            }
        }

        public void SetState(LevelState state)
        {
            State = state;
        }

        public void SetTargetInsanity(float target)
        {
            targetInsanity = target;
        }

        public bool IsColliding(GameObject o)
        {
            return State == LevelState.Good ? Good.IsColliding(o) : Bad.IsColliding(o);
        }

        public void Update(float dt)
        {
            t += dt / 1000;
            Good.Update(dt);
            Bad.Update(dt);
            if (targetInsanity > insanity)
                insanity += dt * insanityChange;
            else if(targetInsanity<insanity)
                insanity -= dt * insanityChange;
        }

        public void Update(GameObject player)
        {
            if (State == LevelState.Good)
                Good.Update(player);
            else
                Bad.Update(player);
        }

        public void Draw (OSpriteBatch batch, ManualCamera2D cam, Player player)
        {
            Blending.SetCam(cam);

            Good.Lights.DrawLights();
            G.Gfx.SetRenderTarget(GoodTarget);
            G.Gfx.Clear(Color.Black);
            batch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, cam.Transformation);
            Good.Draw(batch);
            if (player != null) player.Draw(batch);
            batch.End();
            Good.Lights.Apply(batch);
            G.Gfx.SetRenderTarget(null);
            
            Bad.Lights.DrawLights();
            G.Gfx.SetRenderTarget(BadTarget);
            G.Gfx.Clear(Color.Black);
            batch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, cam.Transformation);
            Bad.Draw(batch);
            if (player != null) player.Draw (batch);
            batch.End();
            Bad.Lights.Apply(batch);
            G.Gfx.SetRenderTarget(null);

            Blending.DrawLights();
            BlendEffect.Parameters["time"].SetValue(t);
            BlendEffect.Parameters["strength"].SetValue(insanity);
            BlendEffect.Parameters["darkWorld"].SetValue(GoodTarget);
            BlendEffect.Parameters["mask"].SetValue(Blending.Target);

            G.Gfx.Clear(Color.Black);
            internalBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, BlendEffect, Matrix.Identity);
            internalBatch.Draw(BadTarget, Vector2.Zero, Color.White);
            internalBatch.End();
        }

        public void DrawDebug(OSpriteBatch batch, ManualCamera2D cam, DrawHelper helper)
        {
            if (insanity <= 0.5)
            {
                helper.DrawPolys (Good.Collision, cam.Transformation, Color.Green);
                Good.Triggers.ForEach (t => helper.DrawPolys (t.collisionPolys, cam.Transformation, Color.Blue));
            }

            else if (insanity > 0.5)
            {
                helper.DrawPolys (Bad.Collision, cam.Transformation, Color.Red);
                Bad.Triggers.ForEach (t => helper.DrawPolys (t.collisionPolys, cam.Transformation, Color.Blue));
            }
        }
    }
}
