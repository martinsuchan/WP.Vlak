using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Vlak.Model;

namespace Vlak.Components
{
    public class CargoItemComponent : ItemComponent<CargoItem>
    {
        public CargoItemComponent(Game game, ContentManager contentManager, CargoItem item)
            : base(game, contentManager, item) {}

        public override void Update(GameTime gameTime)
        {
            if (!Item.InTrain)
            {
                Item.State = (Item.State + 1)%3;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            int x = Common.LeftMargin + Item.X*Common.TileWidth;
            int y = Common.TopMargin + Item.Y*Common.TileHeight;

            int index = Item.State + (int) Item.Direction;
            Rectangle sourceRect =
                new Rectangle((int) Item.CargoType*Common.HalfTileWidth, index*Common.HalfTileHeight, Common.HalfTileWidth, Common.HalfTileHeight);

            // draw the tile
            SpriteBatch.Draw(Texture, new Rectangle(x, y, Common.TileWidth, Common.TileHeight), sourceRect, Color.White);
        }
    }
}