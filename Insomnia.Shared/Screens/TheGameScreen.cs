using System;
using System.Collections.Generic;

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
		Dictionary<string, Rectangle> spriteRects;
		Texture2D spriteSheet;

		Player girl = new Player ();
		PlayerHelper helper = new PlayerHelper ();
		List<Actor> baddies = new List<Actor>();

		float timeSinceLastBaddie = 0;

		public TheGameScreen(Game parent) : base(parent) { }

		public override void Showing ()
		{
			BackgroundColor = Color.Red;
			spriteRects = TextureAtlas.Load ("Insomnia");
			spriteSheet = Content.Load<Texture2D> ("Insomnia");

			girl.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["girl"]) };
			girl.Location = new Vector2 (50, 250);
			girl.MoveSpeed = new Vector2 (40, 0);

			helper.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["fairy"]) };
			helper.Location = new Vector2 (50, 475);
			helper.Tint = Color.Yellow;
			helper.TrackActor = girl;
			helper.Baddies = baddies;

			Baddie spider = new Baddie ();
			spider.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["spider"]) };
			spider.Location = new Vector2 (750, 550);
			spider.Speed = new Vector2 (-10, 0);
			baddies.Add (spider);
		}

		public override void Hiding ()
		{
		}

		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);

			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			timeSinceLastBaddie += elapsed;

			if (timeSinceLastBaddie > 5) {
				CullBaddies ();
				Baddie spider = new Baddie ();
				spider.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["spider"]) };
				spider.Location = new Vector2 (750, 550);
				spider.Speed = new Vector2 (-10, 0);
				baddies.Add (spider);
				timeSinceLastBaddie = 0;
			}

			if (GamePadEx.WasJustPressed (PlayerIndex.One, Buttons.Back)) {
				ScreenUtil.Show (TitleScreen.Instance);
			} else if (GamePadEx.WasJustPressed (PlayerIndex.One, Buttons.Start)) {
				ScreenUtil.Show (new CreditsScreen (Parent));
			} else {
				girl.Update (gameTime);
				helper.Update (gameTime);
				foreach (var baddie in baddies) {
					baddie.Update (gameTime);
				}
			}
		}

		public override void Draw (GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw (gameTime, spriteBatch);

			var loc = Vector2.Zero;
			var rect = spriteRects ["background"];
			while (loc.X < GraphicsDevice.Viewport.Width) {
				spriteBatch.Draw (spriteSheet, loc, rect, Color.DarkGray);
				loc.X += 200;
			}

			girl.Draw (gameTime, spriteBatch);
			helper.Draw (gameTime, spriteBatch);
			foreach (var baddie in baddies) {
				baddie.Draw (gameTime, spriteBatch);
			}
		}

		public void CullBaddies() {
			for (int i = 0; i < baddies.Count; i++) {
				if (baddies [i].IsActive == false) {
					baddies.Remove (baddies [i]);
					i--;
				}
			}
		}
	}
}

