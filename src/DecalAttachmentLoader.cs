using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Otherworld.Utilities;
using Spine;
using TextureFilter = Spine.TextureFilter;

namespace Vest
{
    public class DecalAttachmentLoader
        : AttachmentLoader
    {
        private readonly string root;

        public DecalAttachmentLoader (String root)
        {
            this.root = root;
        }

        public Attachment NewAttachment (Skin skin, AttachmentType type, string name)
        {
            switch (type)
            {
                case AttachmentType.region:         return createRegion (skin, name);
                case AttachmentType.boundingbox:    return createBox (skin, name);
            }

            throw new ArgumentOutOfRangeException ("Cannot create attachment with type " + type);
        }

        private RegionAttachment createRegion (Skin skin, string name)
        {
            var texture = getTexture (name);

            AtlasPage page = new AtlasPage();
            page.rendererObject = texture;
            page.width = texture.Width;
            page.height = texture.Height;
            page.format = texture.Format.ToSpineFormat();
            page.uWrap = TextureWrap.ClampToEdge;
            page.vWrap = TextureWrap.ClampToEdge;
            page.minFilter = TextureFilter.Linear;
            page.magFilter = TextureFilter.Linear;

            AtlasRegion region = new AtlasRegion();
            region.page = page;
            region.rotate = false;
            region.u = 0f;
            region.v = 0f;
            region.u2 = 1f;
            region.v2 = 1f;
            region.x = 0;
            region.y = 0;
            region.width = page.width;
            region.height = page.height;
            region.originalWidth = page.width;
            region.originalHeight = page.height;
            region.offsetX = 0;
            region.offsetY = 0;
            region.index = -1;

            RegionAttachment attachment = new RegionAttachment (name);
            attachment.RendererObject = region;
            attachment.SetUVs (region.u, region.v, region.u2, region.v2, region.rotate);
            attachment.RegionOffsetX = region.offsetX;
            attachment.RegionOffsetY = region.offsetY;
            attachment.RegionWidth = region.width;
            attachment.RegionHeight = region.height;
            attachment.RegionOriginalWidth = region.originalWidth;
            attachment.RegionOriginalHeight = region.originalHeight;

            return attachment;
        }

        private BoundingBoxAttachment createBox (Skin skin, string name)
        {
            return new BoundingBoxAttachment (name);
        }

        private Texture2D getTexture (String name)
        {
            return Game1.Content.Load<Texture2D> (Path.Combine (root, name));
        }
    }
}
