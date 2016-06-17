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
		Texture2D bg;
		public float d = 0;

		public CreditsScreen(Game parent) : base(parent) { }

		public override void Showing ()
		{
			BackgroundColor = Color.Blue;
			bg = Content.Load<Texture2D>("credits");
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
			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start)) {
				ScreenUtil.Show (TitleScreen.Instance);
			}
		}

		public override void Draw (GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw (gameTime, spriteBatch);
			d -= 0.75f;
			if (GamePadEx.GetState(0).ThumbSticks.Left.Y < 0.0f)
			{
				d += GamePadEx.GetState(0).ThumbSticks.Left.Y * 4;
			}
			spriteBatch.Draw(bg, new Vector2(0, d), Color.White);
//			System.Diagnostics.Debug.WriteLine(d);
			if (d < -2225)
			{
				d = 0; // Exit();
			}
		}
	}
}

