﻿using System;
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
        Thresholdsmooth,
        RepeatRead
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
        public float insanityChange = 0.4f / 1000f;
        public float repeatSpeed = 1;
        public float repeatPeriod = 2 * 3.14f;
        public Player player;

        public Vector2 lastPosition;
        public bool crouching;

        public CombiLevel (Player player, ManualCamera2D cam, VestLevel Good, VestLevel Bad)
        {
            this.player = player;
            if (player != null)
                lastPosition = player.position;
            this.Good = Good;
            Good.Load(player,cam, this);
            this.Bad = Bad;
            Bad.Load(player, cam, this);
            State = LevelState.Good;
            GoodTarget = new RenderTarget2D(G.Gfx, G.Gfx.Viewport.Width, G.Gfx.Viewport.Height, false, SurfaceFormat.Rgba1010102, DepthFormat.Depth16);
            BadTarget = new RenderTarget2D(G.Gfx, G.Gfx.Viewport.Width, G.Gfx.Viewport.Height, false, SurfaceFormat.Rgba1010102, DepthFormat.Depth16);
            Blending = new LightOverlay(G.Gfx);
            BlendEffect = G.Content.Load<Effect>("Blend");
            SetTransition(TransitionType.Smooth);
            internalBatch = new SpriteBatch(G.Gfx);
            Blending.SetBlendMode();

            //TODO: REMOVE
            //Blending.AmbientColor = Color.White;
        }

        public void SetTransition(TransitionType t)
        {
            switch (t)
            {
                case TransitionType.Smooth: BlendEffect.CurrentTechnique = BlendEffect.Techniques["SmoothBlend"]; break;
                case TransitionType.Threshold: BlendEffect.CurrentTechnique = BlendEffect.Techniques["ActivateBlend"]; break;
                case TransitionType.ThresholdRead: BlendEffect.CurrentTechnique = BlendEffect.Techniques["ActivateReadBlend"]; break;
                case TransitionType.Thresholdsmooth: BlendEffect.CurrentTechnique = BlendEffect.Techniques["SmoothActivateBlend"]; break;
                case TransitionType.RepeatRead: BlendEffect.CurrentTechnique = BlendEffect.Techniques["RepeatReadBlend"]; break;
            }
        }

        public void SavePosition()
        {
            lastPosition = player.position;
        }

        public void RestorePosition()
        {
            player.Move(lastPosition - player.position);
            
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
            if (o is Monster)
                return Bad.IsColliding(o);
            
            return State == LevelState.Good
                ? Good.IsColliding(o)
                : Bad.IsColliding(o);
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

        public float T
        {
            get { return t; }
            set { t = value; }
        }

        public IEnumerable<T> GetObjects<T>()
            where T : GameObject
        {
            //HACK to make elevator work
            if(typeof(T) == typeof(Elevator))
                return Good.GetObjects<T>();
            switch (State)
            {
                case LevelState.Good:   return Good.GetObjects<T>();
                case LevelState.Evil:   return Bad.GetObjects<T>();
            }
            throw new Exception();
        }

        public void Draw (OSpriteBatch batch, ManualCamera2D cam)
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
            BlendEffect.Parameters["period"].SetValue(repeatPeriod);
            BlendEffect.Parameters["speed"].SetValue(repeatSpeed);

            G.Gfx.Clear(Color.Black);
            internalBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, null, null, BlendEffect, Matrix.Identity);
            internalBatch.Draw(BadTarget, Vector2.Zero, Color.White);
            internalBatch.End();
        }

        public void DrawDebug(OSpriteBatch batch, ManualCamera2D cam, DrawHelper helper)
        {
            if (State == LevelState.Good)
            {
                helper.DrawPolys (Good.Collision, cam.Transformation, Color.Green);
                Good.Triggers.ForEach (t => helper.DrawPolys (t.collisionPolys, cam.Transformation, Color.Blue));
                Good.DrawDebug (batch, cam, helper);
            }
            else if (State == LevelState.Evil)
            {
                helper.DrawPolys (Bad.Collision, cam.Transformation, Color.Red);
                Bad.Triggers.ForEach (t => helper.DrawPolys (t.collisionPolys, cam.Transformation, Color.Blue));
                Bad.DrawDebug (batch, cam, helper);
            }
        }
    }
}
