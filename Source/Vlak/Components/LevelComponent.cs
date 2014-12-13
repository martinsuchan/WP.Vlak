using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vlak.Model;
using Vlak.Model.Enums;

namespace Vlak.Components
{
    /// <summary>
    /// Game component representing single level.
    /// </summary>
    public class LevelComponent : DrawableGameComponent
    {
        private readonly ContentManager contentManager;

        public readonly Level Level;
        private SpriteBatch spriteBatch;

        private readonly List<ItemComponent> Items;

        public LevelComponent(Game game, Level level)
            : base(game)
        {
            contentManager = new ContentManager(game.Services)
            {
                RootDirectory = "Content"
            };
            Level = level;
            Items = new List<ItemComponent>();
        }

        public override void Initialize()
        {
            ItemComponent itemComponent = new TrainItemComponent(Game, contentManager, Level.Train);
            Items.Add(itemComponent);
            itemComponent.Initialize();
            // Initialize all items
            foreach (Item item in Level.Map)
            {
                if (item is WallItem)
                {
                    itemComponent = new WallItemComponent(Game, contentManager, ((WallItem)item));
                }
                else if (item is GateItem)
                {
                    itemComponent = new GateItemComponent(Game, contentManager, ((GateItem)item));
                }
                else if (item is CargoItem)
                {
                    itemComponent = new CargoItemComponent(Game, contentManager, ((CargoItem)item));
                }
                else continue;

                Items.Add(itemComponent);
                itemComponent.Initialize();
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            foreach (ItemComponent item in Items)
            {
                item.SpriteBatch = spriteBatch;
            }
        }

        protected override void UnloadContent()
        {
            contentManager.Unload();
        }

        private int nextTurnCount;
        public override void Update(GameTime gameTime)
        {
            foreach (ItemComponent item in Items)
            {
                item.Update(gameTime);
            }
            if (nextTurnCount == Common.UpdatesPerTurn)
            {
                nextTurnCount = 0;
                TryAutoMove();
                GameController.Instance.Turn();
            }
            nextTurnCount++;
            base.Update(gameTime);
        }

        private void TryAutoMove()
        {
            if (string.IsNullOrEmpty(Level.AutoTrail)) return;

            char dir = Level.AutoTrail[0];
            Level.AutoTrail = Level.AutoTrail.Substring(1);
            switch (dir)
            {
                case 'u':
                    GameController.Instance.Move(Direction.Up);
                    break;
                case 'd':
                    GameController.Instance.Move(Direction.Down);
                    break;
                case 'l':
                    GameController.Instance.Move(Direction.Left);
                    break;
                case 'r':
                    GameController.Instance.Move(Direction.Right);
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            foreach (ItemComponent item in Items)
            {
                item.Draw(gameTime);
            }

            spriteBatch.End();
        }
    }
}
