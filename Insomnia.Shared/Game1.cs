using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

using MoreOnCode.Xna.Lib.Util;

namespace Insomnia.Shared
{
	public class Game1 : Game
	{
		protected GraphicsDeviceManager graphics;
		protected SpriteBatch spriteBatch;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";

			graphics.PreferredBackBufferWidth = 1024;
			graphics.PreferredBackBufferHeight = 768;
//			graphics.IsFullScreen = true;
			graphics.ApplyChanges();
		}

		protected override void Initialize ()
		{
			MoreOnCode.Xna.Framework.Input.GamePadEx.KeyboardPlayerIndex = PlayerIndex.One;
			base.Initialize ();
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);
			ScreenUtil.Show(new SplashScreen(this));
		}

		protected override void Update (GameTime gameTime)
		{
			ScreenUtil.Update(gameTime);
			base.Update (gameTime);
		}

		protected override void Draw (GameTime gameTime)
		{
			spriteBatch.Begin ();
			ScreenUtil.Draw (gameTime, spriteBatch);
			spriteBatch.End ();
			base.Draw (gameTime);
		}
	}
}

