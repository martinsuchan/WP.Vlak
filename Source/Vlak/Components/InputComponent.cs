using System;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Vlak.Model;
using Vlak.Model.Enums;
using Vlak.Resources;

namespace Vlak.Components
{
    /// <summary>
    /// Game component representing single level.
    /// </summary>
    public class InputComponent : DrawableGameComponent
    {
        private readonly ContentManager contentManager;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Texture2D dummyTexture1;
        private Texture2D dummyTexture2;

        private readonly Color overlay = new Color(32, 32, 32);
        private const float rot1 = (float) Math.PI/4;
        private const float rot2 = (float)(3 * Math.PI / 4);
        private const float rot3 = (float)(5 * Math.PI / 4);
        private const float rot4 = (float)(7 * Math.PI / 4);

        public InputComponent(Game game, ContentManager contentManager)
            : base(game)
        {
            DrawOrder = 1000;
            this.contentManager = contentManager;
        }

        protected override void LoadContent()
        {
            // load font and two dummy textures
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            dummyTexture1 = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture1.SetData(new[] { new Color(0, 255, 0, 32) });
            dummyTexture2 = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture2.SetData(new[] { new Color(255, 0, 0, 64) });
            font = contentManager.Load<SpriteFont>("fonts/score");
        }

        public override void Draw(GameTime gameTime)
        {
            Level level = GameController.Instance.Level;
            if (GameController.Instance.State == GameState.Game && level.Number == 1)
            {
                spriteBatch.Begin();
                int x0 = Common.LeftMargin + level.Train.X * Common.TileWidth + (int)(0.5 * Common.TileWidth);
                int y0 = Common.TopMargin + level.Train.Y * Common.TileHeight + (int)(0.5 * Common.TileHeight);
                        
                switch (GameController.Instance.Level.State)
                {
                    case LevelState.Ready:
                        spriteBatch.Draw(dummyTexture1, new Rectangle(x0, y0, 800, 800), null, overlay, rot1, new Vector2(0, 0), SpriteEffects.None, 0);
                        spriteBatch.Draw(dummyTexture2, new Rectangle(x0, y0, 800, 800), null, overlay, rot2, new Vector2(0, 0), SpriteEffects.None, 0);
                        spriteBatch.Draw(dummyTexture1, new Rectangle(x0, y0, 800, 800), null, overlay, rot3, new Vector2(0, 0), SpriteEffects.None, 0);
                        spriteBatch.Draw(dummyTexture2, new Rectangle(x0, y0, 800, 800), null, overlay, rot4, new Vector2(0, 0), SpriteEffects.None, 0);

                        spriteBatch.DrawAligned(font, AppResources.Up, new Vector2(x0, y0 - Common.TileHeight), Color.Gold, HorizontalAlign.Center, VerticalAlign.Bottom);
                        spriteBatch.DrawAligned(font, AppResources.Down, new Vector2(x0, y0 + Common.TileHeight), Color.Gold, HorizontalAlign.Center, VerticalAlign.Top);
                        spriteBatch.DrawAligned(font, AppResources.Left, new Vector2(x0 - Common.TileWidth, y0), Color.Gold, HorizontalAlign.Right, VerticalAlign.Center);
                        spriteBatch.DrawAligned(font, AppResources.Right, new Vector2(x0 + Common.TileWidth, y0), Color.Gold, HorizontalAlign.Left, VerticalAlign.Center);

                        spriteBatch.DrawString(font, AppResources.HowToPlayReady, new Vector2(Common.LeftMargin, Common.H - Common.TopMargin), Color.DarkSlateGray);
                        break;
                    case LevelState.Started:
                        Direction dir = level.Train.Direction;
                        switch (dir)
                        {
                            case Direction.Up:
                                spriteBatch.Draw(dummyTexture1, new Rectangle(0, 0, x0, Common.H), overlay);
                                spriteBatch.Draw(dummyTexture2, new Rectangle(x0, 0, Common.W - x0, Common.H), overlay);
                                spriteBatch.DrawAligned(font, AppResources.Left, new Vector2(x0 - Common.TileWidth, Common.H / 2), Color.Gold, HorizontalAlign.Right, VerticalAlign.Center);
                                spriteBatch.DrawAligned(font, AppResources.Right, new Vector2(x0 + Common.TileWidth, Common.H / 2), Color.Gold, HorizontalAlign.Left, VerticalAlign.Center);
                                break;
                            case Direction.Down:
                                spriteBatch.Draw(dummyTexture2, new Rectangle(0, 0, x0, Common.H), overlay);
                                spriteBatch.Draw(dummyTexture1, new Rectangle(x0, 0, Common.W - x0, Common.H), overlay);
                                spriteBatch.DrawAligned(font, AppResources.Left,new Vector2(x0 - Common.TileWidth, Common.H / 2), Color.Gold, HorizontalAlign.Right, VerticalAlign.Center);
                                spriteBatch.DrawAligned(font, AppResources.Right,new Vector2(x0 + Common.TileWidth, Common.H / 2), Color.Gold, HorizontalAlign.Left, VerticalAlign.Center);
                                break;
                            case Direction.Left:
                            case Direction.Right:
                                spriteBatch.Draw(dummyTexture2, new Rectangle(0, 0, Common.W, y0), overlay);
                                spriteBatch.Draw(dummyTexture1, new Rectangle(0, y0, Common.W, Common.H - y0), overlay);
                                spriteBatch.DrawAligned(font, AppResources.Up, new Vector2(Common.LeftMargin + 320, y0 - Common.TileHeight), Color.Gold, HorizontalAlign.Center, VerticalAlign.Bottom);
                                spriteBatch.DrawAligned(font, AppResources.Down, new Vector2(Common.LeftMargin + 320, y0 + Common.TileHeight), Color.Gold, HorizontalAlign.Center, VerticalAlign.Top);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        spriteBatch.DrawString(font, AppResources.HowToPlayStarted, new Vector2(Common.LeftMargin, Common.H - Common.TopMargin), Color.DarkSlateGray);
                        break;
                    case LevelState.Finished:
                        spriteBatch.DrawString(font, AppResources.HowToPlayFinished, new Vector2(Common.LeftMargin, Common.H - Common.TopMargin), Color.DarkSlateGray);
                        break;
                    case LevelState.Crashed:
                        spriteBatch.DrawString(font, AppResources.HowToPlayCrashed, new Vector2(Common.LeftMargin, Common.H - Common.TopMargin), Color.DarkSlateGray);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
               // spriteBatch.DrawString(font, AppResources.HowToPlayTap, new Vector2(Common.LeftMargin, Common.H - Common.TopMargin), Color.DarkSlateGray);
               // spriteBatch.DrawString(font, AppResources.HowToPlayFlick, new Vector2(Common.LeftMargin, Common.H - Common.TopMargin + 20), Color.DarkSlateGray);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public void Tap(int x, int y, GameTime gameTime)
        {
            switch (GameController.Instance.State)
            {
                case GameState.StartScreen:
                    // handle Level chooser tap
                    if (x <= Common.LeftMargin + 5 * Common.TileWidth + 5)
                    {
                        GameController.Instance.LoadLevelChooser();
                    }
                    // handle help tap
                    else if (x > Common.LeftMargin + 5 * Common.TileWidth + 5 && x <= Common.LeftMargin + 10 * Common.TileWidth + 5)
                    {
                        GameController.Instance.LoadHelp();
                    }
                    // handle sound on/or tap
                    else if (x > Common.LeftMargin + 10 * Common.TileWidth + 5 && x <= Common.LeftMargin + 15 * Common.TileWidth + 5)
                    {
                        GameController.Instance.SwitchSound();
                    }
                    // online stats 
                    else if (x > Common.LeftMargin + 15 * Common.TileWidth + 5)
                    {
                        Guide.BeginShowMessageBox(AppResources.Stats, AppResources.ComingSoon, new[] { AppResources.OK }, 0, MessageBoxIcon.None, null, null);
                    }
                    break;
                case GameState.LevelChooser:
                    // handle level selected tap
                    int w = (x - Common.LeftMargin - Common.HalfTileWidth)/(Common.TileWidth*2);
                    int h = (y - Common.TopMargin - Common.HalfTileHeight)/(Common.TileHeight*2);
                    int levelIndex = h*10 + w + 1;
                    if (levelIndex <= Settings.TopLevel.Value)
                    {
                        GameController.Instance.LoadLevel(levelIndex);
                    }
                    break;
                case GameState.Help:
                    if (x > Common.LeftMargin && x < Common.LeftMargin + Common.MapWidth*Common.TileWidth && y > Common.H - Common.TopMargin)
                    {
                        // show my apps
                        MarketplaceSearchTask v = new MarketplaceSearchTask { SearchTerms = "Martin Suchan" };
                        v.Show();
                        return;
                    }

                    // handle help sroll tap left/right
                    GameController.Instance.LoadStartScreen();
                    break;
                case GameState.Game:
                    if (InputHandled()) return;

                    Level level = GameController.Instance.Level;
                    Direction dir = level.Train.Direction;
                    int x0 = Common.LeftMargin + level.Train.X * Common.TileWidth + (int)(0.5 * Common.TileWidth);
                    int y0 = Common.TopMargin + level.Train.Y * Common.TileHeight + (int)(0.5 * Common.TileHeight);
                    int absX = Math.Abs(x - x0);
                    int absY = Math.Abs(y - y0);

                    // if train is moving up or down or is in initial position an tapped to left or right
                    if ((level.State == LevelState.Started && (dir == Direction.Up || dir == Direction.Down)) || (level.State == LevelState.Ready && absX > absY))
                    {
                        // move left or right
                        GameController.Instance.Move(x > x0 ? Direction.Right : Direction.Left);
                    }
                    // if train is moving left or right or is in initial position an tapped to up or down
                    else
                    {
                        // move up or down
                        GameController.Instance.Move(y > y0 ? Direction.Down : Direction.Up);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Handle flick gesture
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        /// <param name="gameTime"></param>
        public void Flick(int deltaX, int deltaY, GameTime gameTime)
        {
            switch (GameController.Instance.State)
            {
                case GameState.StartScreen:
                    // do nothing in start screen
                    break;
                case GameState.LevelChooser:
                    // do nothing in level chooser screen
                    break;
                case GameState.Help:
                    // do nothing in help screen
                    break;
                case GameState.Game:
                    if (InputHandled()) return;
                    
                    Level level = GameController.Instance.Level;
                    Direction dir = level.Train.Direction;
                    int absX = Math.Abs(deltaX);
                    int absY = Math.Abs(deltaY);

                    if (absX > absY)
                    {
                        if (level.State == LevelState.Ready || (level.State == LevelState.Started && (dir == Direction.Up || dir == Direction.Down)))
                        {
                            // move left or right
                            GameController.Instance.Move(deltaX > 0 ? Direction.Right : Direction.Left);
                        }
                    }
                    else
                    {
                        if (level.State == LevelState.Ready || (level.State == LevelState.Started && (dir == Direction.Left || dir == Direction.Right)))
                        {
                            // move up or down
                            GameController.Instance.Move(deltaY > 0 ? Direction.Down : Direction.Up);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Handle pressed back button.
        /// </summary>
        public void Back()
        {
            switch (GameController.Instance.State)
            {
                case GameState.StartScreen:
                    GameController.Instance.ExitGame();
                    break;
                case GameState.LevelChooser:
                case GameState.Help:
                case GameState.Game:
                    GameController.Instance.LoadStartScreen();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region Internals

        /// <summary>
        /// Handle special conditions, like restarting crashed level or switching to new level when finished.
        /// </summary>
        /// <returns></returns>
        private bool InputHandled()
        {
            Level level = GameController.Instance.Level;

            // in case level is null, should not happen
            if (level == null) return true;

            switch (level.State)
            {
                case LevelState.Ready:
                case LevelState.Started:
                    // default state, handle input gesture
                    return false;
                case LevelState.Finished:
                    // if finished, move to next level
                    GameController.Instance.FinishedLevel();
                    return true;
                case LevelState.Crashed:
                    // if crashed, flick to restart
                    GameController.Instance.LoadLevel();
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void MessageBoxEnd(IAsyncResult result)
        {

        }

        #endregion
    }
}
