using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MoreOnCode.Xna.Framework.Input;

namespace Insomnia.Shared
{
	public class PlayerHelper : Actor
	{
		public enum HelperState { HOVER, CHARGE, ATTACK, RETURN };

		public HelperState State { get; set; }
		public Player TrackActor { get; set; }
		public List<Actor> Baddies { get; set; }

		public Vector2 deltaLocation = Vector2.Zero;
		Random rand = new Random();
		float charge = 0;

		public PlayerHelper () : base()
		{
			State = HelperState.HOVER;
		}

		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);

			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (TrackActor != null) {
				var gamepad = GamePad.GetState (TrackActor.PlayerIndex);
				var isCharging = gamepad.IsButtonDown (Buttons.A);

				if (isCharging && (State == HelperState.HOVER || State == HelperState.CHARGE)) {
					State = HelperState.CHARGE;
					charge += 15 * elapsed;
					if (charge > this.Attack) {
						charge = this.Attack;
					}
				} else {
					if (charge > 0) {
						State = HelperState.ATTACK;
						Location = new Vector2 (Location.X + 400 * elapsed, Location.Y);
						if (Location.X > 1024) {
							charge = 0;
						} else if(Baddies != null && Baddies.Count > 0) {
							var rect1 = this.Sprites [CurrentFrame].TextureRect;
							rect1.X = (int)this.Location.X;
							rect1.Y = (int)this.Location.Y;
							foreach (Actor baddie in Baddies) {
								var rect2 = baddie.Sprites [baddie.CurrentFrame].TextureRect;
								rect2.X = (int)baddie.Location.X;
								rect2.Y = (int)baddie.Location.Y;
								if (rect1.Intersects (rect2) && baddie.IsActive && baddie.Attack > 0) {
									if (baddie.Health <= charge) {
										baddie.IsActive = false;
										charge -= baddie.Health;
										if (baddie.DeathMp3 != null) {
											TheGameScreen.sounds [baddie.DeathMp3].Play();
										}
									} else {
										baddie.Health -= (int)Math.Round(charge);
										charge = 0;
										if (baddie.GruntMp3 != null) {
											TheGameScreen.sounds [baddie.GruntMp3].Play ();
										}
									}
									break;
								}
							}
						}
					} else if (Location.X > TrackActor.Location.X + 100) {
						State = HelperState.RETURN;
						Location = new Vector2 (Location.X - 800 * elapsed, Location.Y);
						if (Location.X < TrackActor.Location.X + 100) {
							State = HelperState.HOVER;
						}
					} else {
						State = HelperState.HOVER;
					}
				}

				switch (State) {
				case HelperState.HOVER:
					deltaLocation.Y = (float)Math.Sin (gameTime.TotalGameTime.TotalSeconds) * 50.0f;
					Location = new Vector2 (TrackActor.Location.X + 100, Location.Y);
					break;
				case HelperState.CHARGE:
					deltaLocation = new Vector2 (rand.Next (20 + (int)charge), rand.Next (25 + (int)charge));
					Location = new Vector2 (TrackActor.Location.X + 50, Location.Y);
					break;
				}
			}
		}

		public override void Draw (GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (HasFrames) {
				Sprites [CurrentFrame].Draw (spriteBatch, Location + deltaLocation, Tint);
			}
		}
	}
}

