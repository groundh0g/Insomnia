using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MoreOnCode.Xna.Lib.Graphics;
using MoreOnCode.Xna.Lib.Util;
using MoreOnCode.Xna.Framework.Input;

namespace Insomnia.Shared
{
	public class TheGameScreen : GameScreen
	{
		public TheGameScreen(Game parent) : base(parent) { }

		public override void Showing ()
		{
			BackgroundColor = Color.Red;
		}

		public override void Hiding ()
		{
		}

		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);

			if (GamePadEx.WasJustPressed (PlayerIndex.One, Buttons.Back)) {
				ScreenUtil.Show(TitleScreen.Instance);
			}
			if (GamePadEx.WasJustPressed (PlayerIndex.One, Buttons.Start)) {
				ScreenUtil.Show(new CreditsScreen(Parent));
			}
		}

		public override void Draw (GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw (gameTime, spriteBatch);
		}
	}
}

