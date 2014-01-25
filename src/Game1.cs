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
using Otherworld.State;

namespace Vest
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDevice Gfx;
        public static StateManager State;
        public static ContentManager Content;

        private GraphicsDeviceManager graphicsDeviceManager;

        public Game1()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
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
                {"test", new TestState()}
            };

            Game1.State = new StateManager (states);
            Game1.Gfx = graphicsDeviceManager.GraphicsDevice;
            Game1.Content = base.Content;

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
