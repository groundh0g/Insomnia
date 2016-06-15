using System;
using System.Collections.Generic;

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
		public List<Actor> Baddies { get; set; }
		public Vector2 locWorld = Vector2.Zero;

		public Player () : base()
		{
			PlayerIndex = PlayerIndex.One;
		}

		public override void Update (GameTime gameTime)
		{
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			var delta = Vector2.Zero;

			var gamepad = GamePadEx.GetState (PlayerIndex);
			if (gamepad.IsButtonDown (Buttons.DPadLeft)) {
				delta.X = -MoveSpeed.X * elapsed;
			}
			if (gamepad.IsButtonDown (Buttons.DPadRight)) {
				delta.X = MoveSpeed.X * elapsed;
			}
			if (gamepad.IsButtonDown (Buttons.DPadUp)) {
				delta.Y = -MoveSpeed.Y * elapsed;
			}
			if (gamepad.IsButtonDown (Buttons.DPadRight)) {
				delta.Y = MoveSpeed.Y * elapsed;
			}

			//Location += delta;
			updateBaddiePosition (-delta);
			locWorld -= delta;

			checkForCollissions ();

			base.Update (gameTime);
		}

		private void checkForCollissions() {
			var rect1 = this.Sprites [CurrentFrame].TextureRect;
			rect1.X = (int)this.Location.X;
			rect1.Y = (int)this.Location.Y;
			foreach (Actor baddie in Baddies) {
				var rect2 = baddie.Sprites [baddie.CurrentFrame].TextureRect;
				rect2.X = (int)baddie.Location.X;
				rect2.Y = (int)baddie.Location.Y;
				if (rect1.Intersects (rect2) && baddie.IsActive) {
					this.Health -= baddie.Attack;
					if (Health > 3) {
						Health = 3;
					}
					if (Health < 0) {
						Health = 0;
					}
					baddie.IsActive = false;
					break;
				}
			}
		}

		private void updateBaddiePosition(Vector2 delta) {
			foreach (Actor baddie in Baddies) {
				if (baddie.IsActive) {
					baddie.Location += delta;
				}
			}
		}

		public override void Draw (GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw (gameTime, spriteBatch);
		}
	}
}

