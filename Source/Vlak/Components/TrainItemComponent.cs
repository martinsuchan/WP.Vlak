using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Vlak.Model;
using Vlak.Model.Enums;

namespace Vlak.Components
{
    public class TrainItemComponent : ItemComponent<TrainItem>
    {
        public TrainItemComponent(Game game, ContentManager contentManager, TrainItem item)
            : base(game, contentManager, item) {}

        public override void Update(GameTime gameTime)
        {
            Item.State = (Item.State + 1)%3;
            // go further in explosion state 
            if (Item.State == 0 && Item.TrainState >= TrainState.StartExplosion && Item.TrainState < TrainState.Wreckage)
            {
                Item.TrainState++;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            int x = Common.LeftMargin + Item.X*Common.TileWidth;
            int y = Common.TopMargin + Item.Y*Common.TileHeight;

            int index;
            Rectangle sourceRect;
            switch (Item.TrainState)
            {
                case TrainState.Running:
                    index = Item.State*4 + (int)Item.Direction;
                    sourceRect = new Rectangle(index*Common.HalfTileWidth, 7*Common.HalfTileHeight, Common.HalfTileWidth, Common.HalfTileHeight);
                    break;
                case TrainState.StartExplosion:
                case TrainState.EndExplosion:
                case TrainState.StartWreckage:
                    index = Item.State + ((int)Item.TrainState*3);
                    sourceRect = new Rectangle(index*Common.HalfTileWidth, 8*Common.HalfTileHeight, Common.HalfTileWidth, Common.HalfTileHeight);
                    break;
                case TrainState.Wreckage:
                    index = Item.State + 7;
                    sourceRect = new Rectangle(index*Common.HalfTileWidth, 8*Common.HalfTileHeight, Common.HalfTileWidth, Common.HalfTileHeight);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // draw the tile
            SpriteBatch.Draw(Texture, new Rectangle(x, y, Common.TileWidth, Common.TileHeight), sourceRect, Color.White);
        }
    }
}
