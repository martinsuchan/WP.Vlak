using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Vlak.Model;

namespace Vlak.Components
{
    public class WallItemComponent : ItemComponent<WallItem>
    {
        public WallItemComponent(Game game, ContentManager contentManager, WallItem item)
            : base(game, contentManager, item) {}

        public override void Draw(GameTime gameTime)
        {
            int x = Common.LeftMargin + Item.X*Common.TileWidth;
            int y = Common.TopMargin + Item.Y*Common.TileHeight;
            Rectangle sourceRect = new Rectangle(18*Common.HalfTileWidth, 7*Common.HalfTileHeight, Common.HalfTileWidth, Common.HalfTileHeight);
            SpriteBatch.Draw(Texture, new Rectangle(x, y, Common.TileWidth, Common.TileHeight), sourceRect, Color.White);
        }
    }
}