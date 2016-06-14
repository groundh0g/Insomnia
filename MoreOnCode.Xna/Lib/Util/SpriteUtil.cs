using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MoreOnCode.Xna.Lib.Graphics;

namespace MoreOnCode.Xna.Lib.Util
{
	public static class SpriteUtil
	{
		public enum VerticalAlign 
		{
			Top,
			Middle,
			Bottom,
		}

		public enum HorizontalAlign 
		{
			Left,
			Center,
			Right,
		}

		public static void DrawInRect(
			SpriteBatch spriteBatch, 
			GameSprite sprite, 
			Rectangle bounds, 
			HorizontalAlign alignHorizontal = HorizontalAlign.Center,
			VerticalAlign alignVertical = VerticalAlign.Middle
		) 
		{
			Vector2 location = Vector2.Zero;
			Vector2 origin = Vector2.Zero;

			switch (alignHorizontal) {
			case HorizontalAlign.Left:
				location.X = bounds.X;
				origin.X = 0;
				break;
			case HorizontalAlign.Center:
				location.X = bounds.X + bounds.Width / 2;
				origin.X = sprite.TextureRect.Width / 2;
				break;
			case HorizontalAlign.Right:
				location.X = bounds.X + bounds.Width;
				origin.X = sprite.TextureRect.Width;
				break;
			}

			switch (alignVertical) {
			case VerticalAlign.Top:
				location.Y = bounds.Y;
				origin.Y = 0;
				break;
			case VerticalAlign.Middle:
				location.Y = bounds.Y + bounds.Height / 2;
				origin.Y = sprite.TextureRect.Height / 2;
				break;
			case VerticalAlign.Bottom:
				location.Y = bounds.Y + bounds.Height;
				origin.Y = sprite.TextureRect.Height;
				break;
			}

			sprite.Draw (spriteBatch, location, origin: origin);
		}
	}
}

