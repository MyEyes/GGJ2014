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
using Vest.state.Vest.state;

namespace Vest
{
    public class G : Microsoft.Xna.Framework.Game
    {
        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;
        
        public static GraphicsDevice Gfx;
        public static StateManager State;
        public static ContentManager Content;
        public static IntPtr Window;

        private GraphicsDeviceManager graphicsDeviceManager;

        public G()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphicsDeviceManager.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphicsDeviceManager.IsFullScreen = false;
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
                {"jason", new JasonState()},
                {"platform", new PlatformingState()},
                {"edit", new EditState()},
                {"game", new GameState()},
                {"flipgame", new FlipGameState()}
            };

            G.State = new StateManager (states);
            G.Gfx = graphicsDeviceManager.GraphicsDevice;
            G.Content = base.Content;
            G.Window = base.Window.Handle;

            // Used by Spine to know that positive y goes vertically down on the screen
            Spine.Bone.yDown = true;

            State.Load();
            //State.Set("edit");
            State.Set("flipgame");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState (PlayerIndex.One).IsButtonDown (Buttons.Back))
                Environment.Exit(1);

            State.Update ((float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        protected override void Draw(GameTime gameTime)
        {
            State.Draw();
        }
    }
}
