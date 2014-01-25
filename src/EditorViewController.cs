using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Vest
{
    public class EditorViewController
    {
        public float OriginX = 0f;
        public float OriginY = 0f;
        public float Zoom = 1f;

        public EditorViewController (IntPtr windowHandle)
        {
            this.windowHandle = windowHandle;
        }

        public void SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Update()
        {
            handleMouseInput ();
            handleKeyboardInput ();
        }

        private const float ZOOM_MIN = 1.8f;
        private const float ZOOM_MAX = 0.02f;
        private const float SCROLL_INTERVAL = 120f;
        private const float SCROLL_DAMPENING = 0.025f;
        private const float MOVE_SPEED = 1;

        private readonly IntPtr windowHandle;
        private MouseState oldMouse;
        private KeyboardState oldKeyboard;
        private Boolean isGrabbed;
        private float width;
        private float height;

        private void handleMouseInput()
        {
            Mouse.WindowHandle = windowHandle;
            MouseState mouse = Mouse.GetState ();

            isGrabbed = wasGrabbed (mouse, oldMouse, isGrabbed);

            if (isGrabbed)
            {
                float deltaMouseX = oldMouse.X - mouse.X;
                float deltaMouseY = oldMouse.Y - mouse.Y;

                OriginX += deltaMouseX / Zoom;
                OriginY += deltaMouseY / Zoom;
            }

            if (mouse.ScrollWheelValue != oldMouse.ScrollWheelValue)
            {
                float scrollDelta = mouse.ScrollWheelValue - oldMouse.ScrollWheelValue;
                float zoomDelta = (scrollDelta / SCROLL_INTERVAL) * SCROLL_DAMPENING;

                Zoom = MathHelper.Clamp (Zoom + zoomDelta, ZOOM_MAX, ZOOM_MIN);
            }

            oldMouse = mouse;
        }

        private void handleKeyboardInput()
        {
            KeyboardState keyboard = Keyboard.GetState ();

            if (wasKeyPressed (keyboard, Keys.A))
            {
                OriginX += MOVE_SPEED;
            }
            if (wasKeyPressed (keyboard, Keys.D))
            {
                OriginX += -MOVE_SPEED;
            }
            if (wasKeyPressed (keyboard, Keys.S))
            {
                OriginY += -MOVE_SPEED;
            }
            if (wasKeyPressed (keyboard, Keys.W))
            {
                OriginY += MOVE_SPEED;
            }

            oldKeyboard = keyboard;
        }

        private bool wasKeyPressed(KeyboardState keyboard, Keys key)
        {
            return keyboard.IsKeyDown (key) && oldKeyboard.IsKeyUp (key);
        }

        private bool isMouseInView(int mouseX, int mouseY)
        {
            return
                mouseX >= 0 &&
                mouseY >= 0 &&
                mouseX <= width &&
                mouseY <= height;
        }

        private bool wasGrabbed(MouseState newMouse, MouseState oldMouse, bool currentIsGrabbed)
        {
            var rightPressed =
                newMouse.RightButton == ButtonState.Pressed &&
                oldMouse.RightButton == ButtonState.Released;

            if (rightPressed && isMouseInView (newMouse.X, newMouse.Y))
            {
                return true;
            }
            else if (newMouse.RightButton == ButtonState.Released)
            {
                return false;
            }
            else
            {
                return currentIsGrabbed;
            }
        }
    }
}
