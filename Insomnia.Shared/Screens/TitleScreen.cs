using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MoreOnCode.Xna.Framework.Input;
using MoreOnCode.Xna.Lib.Graphics;
using MoreOnCode.Xna.Lib.Util;

namespace Insomnia.Shared
{
	public class TitleScreen : GameScreen
	{
		public static GameScreen Instance;
		
		public TitleScreen(Game parent) : base(parent) { }

		public override void Showing ()
		{
			BackgroundColor = Color.Green;
			TitleScreen.Instance = this;
		}

		public override void Hiding ()
		{
		}

		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);

			if (GamePadEx.WasJustPressed(PlayerIndex.One, Buttons.Back)) {
				Parent.Exit ();
			}
			if (GamePadEx.WasJustPressed(PlayerIndex.One, Buttons.Start)) {
				ScreenUtil.Show(new TheGameScreen(Parent));
			}
		}

		public override void Draw (GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw (gameTime, spriteBatch);
		}
	}
}

