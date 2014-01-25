using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Vest.state;
using Vest.State;

namespace Vest
{
    public class G : Microsoft.Xna.Framework.Game
    {
        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;
        
        public static GraphicsDevice Gfx;
        public static StateManager State;
        public static ContentManager Content;

        private GraphicsDeviceManager graphicsDeviceManager;

        public G()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphicsDeviceManager.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphicsDeviceManager.ApplyChanges();

            this.IsMouseVisible = true;
            
            base.Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            var states = new Dictionary<string, BaseGameState>
            {
                {"test", new TestState()},
                {"jason", new JasonState()}
            };

            G.State = new StateManager (states);
            G.Gfx = graphicsDeviceManager.GraphicsDevice;
            G.Content = base.Content;

            // Used by Spine to know that positive y goes vertically down on the screen
            Spine.Bone.yDown = true;

            State.Load ();
            State.Set ("test");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            State.Update ((float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        protected override void Draw(GameTime gameTime)
        {
            State.Draw();
        }
    }
}
