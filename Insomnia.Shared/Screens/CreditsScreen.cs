using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MoreOnCode.Xna.Framework.Input;
using MoreOnCode.Xna.Lib.Graphics;
using MoreOnCode.Xna.Lib.Util;

namespace Insomnia.Shared
{
	public class CreditsScreen : GameScreen
	{
		public CreditsScreen(Game parent) : base(parent) { }

		public override void Showing ()
		{
			BackgroundColor = Color.Blue;
		}

		public override void Hiding ()
		{
		}

		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);

			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back)) {
				ScreenUtil.Show (TitleScreen.Instance);
			}
		}

		public override void Draw (GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw (gameTime, spriteBatch);
		}
	}
}

