using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vest
{
    public class ManualCamera2D
    {
        public ManualCamera2D(int width, int height, GraphicsDevice graphicsDevice)
        {
            this.width = width;
            this.height = height;
            this.screenHalf = new Vector2 (width / 2f, height / 2f);
            this.updateTransform = true;
            this.graphicsDevice = graphicsDevice;
        }

        public Vector2 Origin
        {
            get { return Location + (screenHalf / Zoom); }
        }

        public Matrix Transformation
        {
            get
            {
                if (updateTransform)
                    generateTransformation ();

                return transformation;
            }
        }

        public Vector2 Location = new Vector2 (0, 0);
        public float Zoom = 1f;
        public bool UseBounds;
        public Rectangle Bounds;
        public Rectangle ViewFrustrum;

        public void MoveTo(Vector2 loc)
        {
            Location = loc;
        }

        public void CenterOnPoint(Vector2 loc)
        {
            CenterOnPoint (loc.X, loc.Y);
        }

        public void CenterOnPoint(float x, float y)
        {
            float newx = x - (screenHalf.X / this.Zoom);
            float newy = y - (screenHalf.Y / this.Zoom);

            if (UseBounds)
            {
                if (newx < 0) newx = 0;
                if (newy < 0) newy = 0;
                if (newx + width > Bounds.Right) newx = Bounds.Right - width;
                if (newy + height > Bounds.Bottom) newy = Bounds.Bottom - height;
            }

            Location = new Vector2 (newx, newy);
            updateTransform = true;
        }

        public void Resize(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.screenHalf = new Vector2 (width / 2f, height / 2f);
        }

        public Vector2 ScreenToWorld(Vector2 screenPos)
        {
            return new Vector2 (
                (screenPos.X / this.Zoom) + Location.X,
                (screenPos.Y / this.Zoom) + Location.Y);
        }

        public float GetYOrder(float y)
        {
            float depth = y;
            depth -= this.Location.Y;
            depth /= this.height;
            return depth;
        }

        public float GetObjectDepth(Vector2 objectLocation, float objectHeight = 0f, float objectScale = 1f)
        {
            //Get base of the object
            var objectBase = objectLocation + new Vector2 (0, objectHeight * objectScale);

            //Offset the objects position to camera position
            objectBase.X -= this.ViewFrustrum.Left;
            objectBase.Y -= this.ViewFrustrum.Top;

            //Get object depth
            float depth = (((objectBase.X / 260) + (objectBase.Y / 150))
                        / (ViewFrustrum.Width + ViewFrustrum.Height));

            depth = 1 - (objectBase.Y / ViewFrustrum.Height);

            return depth;
        }

        private int width;
        private int height;
        private Vector2 screenHalf;
        private bool updateTransform = false;
        private Matrix transformation;
        private GraphicsDevice graphicsDevice;

        private void generateTransformation()
        {
            transformation =
                Matrix.CreateTranslation (new Vector3 (-Location.X, -Location.Y, 0)) *
                Matrix.CreateScale (new Vector3 (this.Zoom, this.Zoom, 0));


            // Calculate the view frustrum
            var worldAtZero = ScreenToWorld (Vector2.Zero);
            var worldAtView = ScreenToWorld (new Vector2 (graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));

            ViewFrustrum = new Rectangle (
                (int)Math.Floor (worldAtZero.X),
                (int)Math.Floor (worldAtZero.Y),
                (int)Math.Ceiling (worldAtView.X - worldAtZero.X),
                (int)Math.Ceiling (worldAtView.Y - worldAtZero.Y));
        }
    }
}
