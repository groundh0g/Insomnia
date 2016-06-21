using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MoreOnCode.Xna.Framework.Input;
using MoreOnCode.Xna.Lib.Graphics;

namespace Insomnia.Shared
{
	public class Player : Actor
	{
		public enum PlayerState { IDLE, RUN, JUMP };

		public PlayerState State { get; set; }
		public Dictionary<PlayerState, List<GameSprite>> PlayerSprites { get; set; }
		public PlayerIndex PlayerIndex { get; set; }
		public Vector2 MoveSpeed { get; set; }
		public List<Actor> Baddies { get; set; }
		public Vector2 locWorld = Vector2.Zero;

		public Player () : base()
		{
			PlayerIndex = PlayerIndex.One;
			State = PlayerState.IDLE;
		}

		public override void Update (GameTime gameTime, Game Parent)
		{
			if (GamePad.GetState (PlayerIndex).IsConnected == false) {
				IsActive = false;
			}

			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			var delta = Vector2.Zero;

            var gamepad = GamePadEx.GetState(PlayerIndex);
			if(gamepad.IsButtonDown(Buttons.DPadRight)) {
				delta.X = MoveSpeed.X * elapsed * gamepad.ThumbSticks.Left.X * 2;
			}
				
            if (gamepad.ThumbSticks.Left.X < -0.1f)
            {
                if (Location.X > 0)
                {
                    delta.X = MoveSpeed.X * elapsed * gamepad.ThumbSticks.Left.X * 2;
                }
            }
            if (gamepad.ThumbSticks.Left.X > 0.1f)
            {
                if (Location.X < 960)
                {
                    delta.X = MoveSpeed.X * elapsed * gamepad.ThumbSticks.Left.X * 2;
                }
            }

			if (delta != Vector2.Zero) {
				if (State != PlayerState.RUN) {
					State = PlayerState.RUN;
					CurrentFrame = 0;
				}
			} else {
				if (State != PlayerState.IDLE) {
					State = PlayerState.IDLE;
					CurrentFrame = 0;
				}
			}

            if (PlayerIndex == 0)
            {
                updateBaddiePosition(-delta);
            }
            if (PlayerIndex == 0)
            {
                locWorld -= delta;
            }
            else
            {
                Location += delta;
            }

			//base.Update (gameTime, Parent);

            timeOnCurrentFrame -= elapsed;
			if (timeOnCurrentFrame < 0.0) {
				CurrentFrame = (CurrentFrame + 1) % PlayerSprites [State].Count;
				timeOnCurrentFrame = TimePerFrame;
			}

			checkForCollissions ();

		}

		private void checkForCollissions() {
			var rect1 = this.PlayerSprites[State] [CurrentFrame].TextureRect;
			rect1.X = (int)this.Location.X;
			rect1.Y = (int)this.Location.Y;
			foreach (Actor baddie in Baddies) {
				var rect2 = baddie.Sprites [baddie.CurrentFrame].TextureRect;
				rect2.X = (int)baddie.Location.X;
				rect2.Y = (int)baddie.Location.Y;
				if (rect1.Intersects (rect2) && baddie.IsActive) {

					if (Health == 3 && baddie.Attack == -1) {
						// if cookie and full, do nothing
					} else {

						this.Health -= baddie.Attack;
						if (Health > 3) {
							Health = 3;
						} else {
							if (Health <= 0) {
								Health = 0;
								TheGameScreen.sounds ["girl-death"].Play ();
								IsActive = false;
							} else if(baddie.Attack > 0){
								TheGameScreen.sounds ["girl-hit"].Play ();
							}
						}
						break;

					}
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
            if (GamePad.GetState(PlayerIndex).IsConnected)
            {
                PlayerSprites[State][CurrentFrame].Draw(spriteBatch, Location, Tint);
            }
        }
	}
}

