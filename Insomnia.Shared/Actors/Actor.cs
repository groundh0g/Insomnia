using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MoreOnCode.Xna.Lib.Graphics;

namespace Insomnia.Shared
{
	public class Actor
	{
		public bool IsActive { get; set; }

		public Vector2 Location { get; set; }
		public GameSprite[] Sprites { get; set; }
		public int Health { get; set; }
		public int Attack { get; set; }
		public double TimePerFrame { get; set; }
		public int CurrentFrame { get; set; }
		public Color Tint { get; set; }
		public Vector2 Speed { get; set; }

		public Actor () : this(null, null, null, null, null) { }

		public Actor (Vector2? location, int? health, int? attack, GameSprite[] sprites, double? timePerFrame) {
			this.Sprites = sprites ?? new GameSprite[0];
			this.TimePerFrame = timePerFrame.HasValue ? timePerFrame.Value : 0.1;
			this.Location = location.HasValue ? location.Value : Vector2.Zero;
			this.Health = health.HasValue ? health.Value : 3;
			this.Attack = attack.HasValue ? attack.Value : 1;
			this.CurrentFrame = 0;
			this.Tint = Color.White;
			this.Speed = Vector2.Zero;
			this.IsActive = true;
		}

		protected bool HasFrames {
			get {
				return Sprites != null && Sprites.Length > 0;
			}
		}

		protected double timeOnCurrentFrame = 0.0;
		public virtual void Update(GameTime gameTime) {
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

			Location += Speed * elapsed;

			timeOnCurrentFrame -= elapsed;
			if (timeOnCurrentFrame < 0.0) {
				if (HasFrames && IsActive) {
					CurrentFrame = (CurrentFrame + 1) % Sprites.Length;
				}
				timeOnCurrentFrame = TimePerFrame;
			}
		}

		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
			if (HasFrames && IsActive) {
				Sprites [CurrentFrame].Draw (spriteBatch, Location, Tint);
			}
		}
	}
}

