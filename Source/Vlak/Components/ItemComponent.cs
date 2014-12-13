using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vlak.Model;

namespace Vlak.Components
{
    public abstract class ItemComponent : DrawableGameComponent
    {
        protected Item BaseItem;
        public SpriteBatch SpriteBatch;

        protected ItemComponent(Game game)
            : base(game) {}
    }

    public abstract class ItemComponent<T> : ItemComponent
        where T : Item
    {
        protected readonly ContentManager ContentManager;
        protected static Texture2D Texture;

        protected T Item;

        protected ItemComponent(Game game, ContentManager contentManager, T item)
            : base(game)
        {
            ContentManager = contentManager;
            BaseItem = Item = item;
        }

        protected override void LoadContent()
        {
            if (Texture == null)
            {
                Texture = ContentManager.Load<Texture2D>(Common.ImageAsset);
            }
        }
    }
}