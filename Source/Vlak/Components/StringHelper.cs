using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vlak.Components
{
    public static class StringHelper
    {
        public static void DrawAligned(this SpriteBatch batch, SpriteFont font, string text, Vector2 position, Color color, HorizontalAlign hor, VerticalAlign ver)
        {
            Vector2 textSize = font.MeasureString(text);
            float hAlign;
            switch (hor)
            {
                case HorizontalAlign.Left:
                    hAlign = 0;
                    break;
                case HorizontalAlign.Center:
                    hAlign = textSize.X/2;
                    break;
                case HorizontalAlign.Right:
                    hAlign = textSize.X;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("hor");
            }
            float vAlign;
            switch (ver)
            {
                case VerticalAlign.Top:
                    vAlign = 0;
                    break;
                case VerticalAlign.Center:
                    vAlign = textSize.Y/2;
                    break;
                case VerticalAlign.Bottom:
                    vAlign = textSize.Y;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ver");
            }
            Vector2 newPos = new Vector2(position.X - hAlign, position.Y - vAlign);

            batch.DrawString(font, text, newPos, color);
        }
    }
}