using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MoreOnCode.Xna.Framework.Input;

namespace Insomnia.Shared
{
	public class Player : Actor
	{
		public PlayerIndex PlayerIndex { get; set; }
		public Vector2 MoveSpeed { get; set; }

		public Player () : base()
		{
			PlayerIndex = PlayerIndex.One;
		}

		public override void Update (GameTime gameTime)
		{
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

			var gamepad = GamePadEx.GetState (PlayerIndex);
			if (gamepad.IsButtonDown (Buttons.DPadLeft)) {
				Location = new Vector2(Location.X - MoveSpeed.X * elapsed, Location.Y);
			}
			if (gamepad.IsButtonDown (Buttons.DPadRight)) {
				Location = new Vector2(Location.X + MoveSpeed.X * elapsed, Location.Y);
			}
			if (gamepad.IsButtonDown (Buttons.DPadUp)) {
				Location = new Vector2(Location.X, Location.Y - MoveSpeed.Y * elapsed);
			}
			if (gamepad.IsButtonDown (Buttons.DPadRight)) {
				Location = new Vector2(Location.X, Location.Y + MoveSpeed.Y * elapsed);
			}

			base.Update (gameTime);
		}

		public override void Draw (GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw (gameTime, spriteBatch);
		}
	}
}

