using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Vlak.Model;

namespace Vlak.Components
{
    public class GateItemComponent : ItemComponent<GateItem>
    {
        public GateItemComponent(Game game, ContentManager contentManager, GateItem item)
            : base(game, contentManager, item) {}

        public override void Update(GameTime gameTime)
        {
            if (Item.State >= GateItem.Open && Item.State < GateItem.FullOpen)
            {
                Item.State++;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Item.State == GateItem.Hidden) return;

            int x = Common.LeftMargin + Item.X * Common.TileWidth;
            int y = Common.TopMargin + Item.Y * Common.TileHeight;

            int index = Item.State + 12;
            Rectangle sourceRect = new Rectangle(index * Common.HalfTileWidth, 7 * Common.HalfTileHeight, Common.HalfTileWidth, Common.HalfTileHeight);

            // draw the tile
            SpriteBatch.Draw(Texture, new Rectangle(x, y, Common.TileWidth, Common.TileHeight), sourceRect, Color.White);
        }
    }
}