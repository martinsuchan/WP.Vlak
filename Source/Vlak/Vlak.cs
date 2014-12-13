using System;
using System.Diagnostics;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Vlak.Components;
using Vlak.Model;

namespace Vlak
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Vlak : Game, IGameView
    {
        private readonly GraphicsDeviceManager graphics;
        private GameController gameController;
        private InputComponent panelComponent;
        private ScoreComponent scoreComponent;
        private LevelComponent levelComponent;

        public Vlak()
        {
            // comment this in release!
            //Guide.SimulateTrialMode = true;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is slowed down to match the original Vlak game speed
            TargetElapsedTime = TimeSpan.FromTicks(1333333);

            graphics.IsFullScreen = true;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            Common.W = graphics.PreferredBackBufferWidth = 800;
            Common.H = graphics.PreferredBackBufferHeight = 480;

            Common.Window = Window;
            Common.TopMargin = (Common.H - Common.MapHeight*Common.TileHeight)/2;
            Common.LeftMarginThin = (Common.W - Common.MapWidth*Common.TileWidth)/3;
            Common.LeftMarginThick = 2*Common.LeftMarginThin;

            // Enable gestures
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Flick;

            // handle unknown exception
            Application.Current.UnhandledException += Application_UnhandledException;

            // game width 640 / 20 = 32PX
            // game height 400 / 12 + 16px score panel
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // create panel component
            panelComponent = new InputComponent(this, Content);
            Components.Add(panelComponent);

            // create score component
            scoreComponent = new ScoreComponent(this, Content);
            Components.Add(scoreComponent);

            // create the main game controller
            gameController = new GameController(this, Content);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                panelComponent.Back();
            }

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                switch (gesture.GestureType)
                {
                    case GestureType.Tap:
                        panelComponent.Tap((int)gesture.Position.X, (int)gesture.Position.Y, gameTime);
                        break;
                    case GestureType.Flick:
                        panelComponent.Flick((int)gesture.Delta.X, (int)gesture.Delta.Y, gameTime);
                        break;
                }
            }

            base.Update(gameTime);
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            Settings.Levels.Value = Settings.Levels.Value;
            base.OnDeactivated(sender, args);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // An unhandled exception has occurred; break into the debugger
            if (Debugger.IsAttached) Debugger.Break();
        }

        public void ShowLevel(Level level)
        {
            // create level component
            levelComponent = new LevelComponent(this, level);
            Components.Insert(0, levelComponent);
            ResetElapsedTime();
        }

        public void HideLevel()
        {
            Components.Remove(levelComponent);
            levelComponent = null;
        }

        void IGameView.Exit()
        {
            Exit();
        }
    }
}