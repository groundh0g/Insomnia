using System;
using System.Collections.Generic;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoreOnCode.Xna.Lib.Graphics
{
	public static class TextureAtlas
	{
		// load as Rectangles
		public static Dictionary<string, Rectangle> Load(string assetName) {
			var result = new Dictionary<string, Rectangle> ();
			var xml = new XmlDocument ();
			xml.Load(TitleContainer.OpenStream (assetName + ".xml"));
			foreach (XmlNode node in xml.SelectNodes("/TextureAtlas/sprite")) {
				var rect = Rectangle.Empty;
				rect.X = int.Parse (node.Attributes ["x"].Value);
				rect.Y = int.Parse (node.Attributes ["y"].Value);
				rect.Width = int.Parse (node.Attributes ["w"].Value);
				rect.Height = int.Parse (node.Attributes ["h"].Value);
				var name = node.Attributes ["n"].Value
					.Replace (".png", "")
					.Replace (".gif", "")
					.Replace (".jpg", "")
					.Replace (".tif", "")
					.Replace (".bmp", "")
					.Replace (".rle", "");
				result.Add(name, rect);
			}
			return result;
		}

		// load as GameSprites
		public static Dictionary<string, GameSprite> Load(string assetName, Texture2D texture) {
			var result = new Dictionary<string, GameSprite> ();
			var rectangles = Load (assetName);
			foreach (var key in rectangles.Keys) {
				result.Add(key, new GameSprite (texture, rectangles[key]));
			}
			return result;
		}
	}
}

