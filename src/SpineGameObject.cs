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
using Otherworld.Graphics;
using Otherworld.Utilities;
using Spine;

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

        protected void LoadSkeleton(string skeletonPath, String decalsPath)
        {
            SkeletonJson json = new SkeletonJson (new DecalAttachmentLoader (decalsPath));
            SkeletonData skeletonData = json.ReadSkeletonData (skeletonPath);

            skeleton = new Skeleton (skeletonData);
            skeleton.SetSlotsToSetupPose ();

            skeletonRenderer = new SkeletonRenderer (Game1.Gfx);
            skeletonRenderer.PremultipliedAlpha = true;

            AnimationStateData animStateData = new AnimationStateData (skeletonData);
            AnimState = new AnimationState (animStateData);
        }
    }
}