using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spine;

namespace Otherworld.Utilities
{
    public static class SpineHelper
    {
        public static Spine.Format ToSpineFormat (this SurfaceFormat format)
        {
            switch (format)
            {
                case SurfaceFormat.Color:       return Spine.Format.RGBA8888;
                case SurfaceFormat.Bgr565:      return Spine.Format.RGB565;
                case SurfaceFormat.Bgra4444:    return Spine.Format.RGBA4444;
                case SurfaceFormat.Alpha8:      return Spine.Format.Alpha;
            }

            throw new NotSupportedException (format.ToString());
        }

        /// <summary>
        /// Returns the world position of the bone, which does not take into
        /// consideration Skeleton.X, or Skeleton.Y so you can optionally pass
        /// in the WorldPosition of the skeleton to offset the bone's location.
        /// </summary>
        /// <param name="skeleton">The skeleton to find the bone of</param>
        /// <param name="boneName">The name of the bone to find the world postion of</param>
        /// <param name="worldOffset">The amount to offset the position of the bone in worlds coords</param>
        /// <returns>The position of the bone's origin in world coordinates</returns>
        public static Vector2 getBoneWorldPos (this Skeleton skeleton, String boneName,  Vector2 worldOffset)
        {
            Bone bone = skeleton.FindBone (boneName);
            return worldOffset + new Vector2 (bone.WorldX, bone.WorldY);
        }
    }
}
