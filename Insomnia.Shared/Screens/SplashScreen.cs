using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MoreOnCode.Xna.Framework.Input;
using MoreOnCode.Xna.Lib.Graphics;
using MoreOnCode.Xna.Lib.Util;

namespace Insomnia.Shared
{
	public class SplashScreen : GameScreen
	{
		public SplashScreen(Game parent) : base(parent) { }

		public override void Showing ()
		{
			BackgroundColor = Color.White;
		}

		public override void Hiding ()
		{
		}

		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);

			if (GamePadEx.WasJustPressed (PlayerIndex.One, Buttons.Start)) {
				ShowTitleScreen ();
			} else if (gameTime.TotalGameTime.TotalSeconds > 5.0) {
				ShowTitleScreen ();
			}
		}

		private bool doUpdates = true;
		private void ShowTitleScreen() {
			if (doUpdates) {
				doUpdates = false;
				ScreenUtil.Show (new TitleScreen (Parent));
			}
		}

		public override void Draw (GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw (gameTime, spriteBatch);
		}
	}
}

