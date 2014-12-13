using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vlak.Model;
using Vlak.Model.Enums;
using Vlak.Resources;

namespace Vlak.Components
{
    /// <summary>
    /// Game component for showing Score and current Level.
    /// </summary>
    public class ScoreComponent : DrawableGameComponent
    {
        private readonly ContentManager contentManager;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private SpriteFont big;
        private SpriteFont levels;

        public ScoreComponent(Game game, ContentManager contentManager)
            : base(game)
        {
            this.contentManager = contentManager;
        }

        protected override void LoadContent()
        {
            // load each font from the content pipeline
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            font = contentManager.Load<SpriteFont>("fonts/score");
            big = contentManager.Load<SpriteFont>("fonts/big");
            levels = contentManager.Load<SpriteFont>("fonts/levels");
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            switch (GameController.Instance.State)
            {
                case GameState.StartScreen:
                    DrawStartScreen();
                    break;
                case GameState.LevelChooser:
                    DrawLevelChooser();
                    break;
                case GameState.Help:
                    DrawHelp();
                    break;
                case GameState.Game:
                    DrawGame();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawStartScreen()
        {
            // link to level chooser - play game
            spriteBatch.DrawString(big, AppResources.Play,
                new Vector2(Common.LeftMargin + 5, Common.TopMargin - Common.TileHeight), Color.DeepSkyBlue);

            // link to help screen
            spriteBatch.DrawString(big, AppResources.Help,
                new Vector2(Common.LeftMargin + 5 * Common.TileWidth + 5, Common.TopMargin - Common.TileHeight), Color.LimeGreen);

            // sound settings
            spriteBatch.DrawString(big, string.Format("{0}: [{1}]", AppResources.Sound, Settings.SoundEnabled.Value ? AppResources.On : AppResources.Off),
                new Vector2(Common.LeftMargin + 10 * Common.TileWidth + 5, Common.TopMargin - Common.TileHeight), Color.Gold);

            // sound settings
            spriteBatch.DrawString(big, AppResources.Stats,
                new Vector2(Common.LeftMargin + 15 * Common.TileWidth + 5, Common.TopMargin - Common.TileHeight), Color.Tomato);
        }

        private void DrawLevelChooser()
        {
            for (int i = 1; i <= 50; i++)
            {
                int w = (i - 1)%10;
                int h = (i - 1) / 10;
                Color color = i < Settings.TopLevel.Value
                    ? Color.Gold
                    : (i == Settings.TopLevel.Value
                        ? Color.DeepSkyBlue
                        : Color.DarkSlateGray);
                spriteBatch.DrawString(levels, i.ToString(),
                    new Vector2(Common.LeftMargin + Common.TileWidth * (2 * w + 1), Common.TopMargin  + Common.TileHeight * (2 * h + 1)), color);
            }
        }

        private void DrawHelp()
        {
            spriteBatch.DrawString(font, AppResources.ThisIsYou,
                new Vector2(Common.LeftMargin, Common.TopMargin), Color.DeepSkyBlue);

            spriteBatch.DrawString(font, AppResources.YourGoalIs,
                new Vector2(Common.LeftMargin, Common.TopMargin + Common.TileHeight*2), Color.DeepSkyBlue);

            spriteBatch.DrawString(font, AppResources.GoToGate,
                new Vector2(Common.LeftMargin, Common.TopMargin + Common.TileHeight*5), Color.DeepSkyBlue);

            spriteBatch.DrawString(font, AppResources.DoNotHitWall,
                new Vector2(Common.LeftMargin, Common.TopMargin + Common.TileHeight*7), Color.DeepSkyBlue);

            spriteBatch.DrawString(font, AppResources.DoNotHitCargo,
                new Vector2(Common.LeftMargin, Common.TopMargin + Common.TileHeight * 8), Color.DeepSkyBlue);

            spriteBatch.DrawString(font, AppResources.HowToPlayHint,
                new Vector2(Common.LeftMargin, Common.TopMargin + Common.TileHeight * 9), Color.DeepSkyBlue);

            spriteBatch.DrawString(font, AppResources.GoodLuck,
                new Vector2(Common.LeftMargin, Common.TopMargin + Common.TileHeight*10), Color.DeepSkyBlue);


            spriteBatch.DrawString(font, string.Format(AppResources.Author, Common.Version),
                new Vector2(Common.LeftMargin, Common.H - Common.TopMargin), Color.DarkSlateGray);
            spriteBatch.DrawString(font, AppResources.OriginalAuthor,
                new Vector2(Common.LeftMargin, Common.H - Common.TopMargin + 20), Color.DarkSlateGray);
        }

        private void DrawGame()
        {
            // draw steps board
            spriteBatch.DrawString(font, AppResources.Steps, new Vector2(Common.LeftMargin, Common.TopMargin - Common.TileHeight), Color.LimeGreen);
            if (GameController.Instance.Level != null)
            {
                int score = GameController.Instance.Steps;
                int bestScore = GameController.Instance.Info.MinSteps;
                Color color = score < bestScore
                    ? Color.Gold
                    : (score == bestScore
                        ? Color.DeepSkyBlue
                        : Color.Tomato);
                spriteBatch.DrawString(font, score + (bestScore < int.MaxValue ? (" / ") + bestScore : ""),
                    new Vector2(Common.LeftMargin + 100, Common.TopMargin - Common.TileHeight), color);
            }

            // draw remaining items
            spriteBatch.DrawString(font, AppResources.Items, new Vector2(Common.LeftMargin + 250, Common.TopMargin - Common.TileHeight), Color.LimeGreen);
            if (GameController.Instance.Level != null)
            {
                int score = GameController.Instance.RemainingCargo;
                spriteBatch.DrawString(font, score.ToString(),
                    new Vector2(Common.LeftMargin + 350, Common.TopMargin - Common.TileHeight), Color.Gold);
            }

            // draw level board
            spriteBatch.DrawString(font, AppResources.Level, new Vector2(Common.LeftMargin + 515, Common.TopMargin - Common.TileHeight), Color.LimeGreen);
            if (GameController.Instance.Level != null)
            {
                int number = GameController.Instance.CurrentLevel;
                spriteBatch.DrawString(font, number.ToString(),
                    new Vector2(Common.LeftMargin + 615, Common.TopMargin - Common.TileHeight), Color.Gold);
            }
        }
    }
}
