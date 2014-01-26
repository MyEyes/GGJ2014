using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Otherworld.Utilities;
using Spine;
using Vest.graphics;

namespace Vest
{
    public class SpineGameObject
        : GameObject
    {
        public SpineGameObject(Vector2 position, Polygon[] polygons)
            : base(position, polygons)
        {
        }

        public AnimationState AnimState { get; private set; }
        public AnimationStateData AnimData { get; private set; }
        public bool FlipX { set { skeleton.FlipX = value; }}

        private ManualCamera2D cam;
        private Skeleton skeleton;
        private SkeletonRenderer skeletonRenderer;
        private String currentAnim;

        public override void Update(float dt)
        {
            AnimState.Update (dt / 1000);
            AnimState.Apply (skeleton);

            skeleton.X = position.X;
            skeleton.Y = position.Y;
            skeleton.UpdateWorldTransform();
        }

        public override void Draw (OSpriteBatch batch)
        {
            batch.Draw (skeleton, 1);
        }

        public void Load (ManualCamera2D cam)
        {
            this.cam = cam;
        }

        public void UpdateAnim (String newAnim, LookDir dir, bool loop=true)
        {
            Boolean flipAnim = dir == LookDir.Left;

            if (newAnim != currentAnim || flipAnim != skeleton.FlipX)
            {
                //Console.WriteLine ("Switching to anim={0}, flipX={1}", newAnim, skeleton.FlipX);
                AnimState.SetAnimation (0, newAnim, loop);
                skeleton.FlipX = flipAnim;
                currentAnim = newAnim;
            }
        }

        public void LoadSkeleton(string skeletonPath, String decalsPath)
        {
            SkeletonJson json = new SkeletonJson (new DecalAttachmentLoader (decalsPath));
            SkeletonData skeletonData = json.ReadSkeletonData (skeletonPath);

            skeleton = new Skeleton (skeletonData);
            skeleton.SetSlotsToSetupPose ();

            skeletonRenderer = new SkeletonRenderer (G.Gfx);
            skeletonRenderer.PremultipliedAlpha = true;

            AnimData = new AnimationStateData (skeletonData);
            AnimState = new AnimationState (AnimData);
        }
    }
}