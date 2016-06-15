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
		Texture2D spriteShadow;

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
			spriteShadow = Content.Load<Texture2D> ("shadow");

			girl.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["girl"]) };
			girl.Location = new Vector2 (50, 250);
			girl.MoveSpeed = new Vector2 (40, 0);

			helper.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["fairy"]) };
			helper.Location = new Vector2 (50, 475);
			helper.Tint = Color.Yellow;
			helper.TrackActor = girl;
			helper.Baddies = baddies;

//			Baddie spider = new Baddie ();
//			spider.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["spider"]) };
//			spider.Location = new Vector2 (750, 550);
//			spider.Speed = new Vector2 (-10, 0);
//			baddies.Add (spider);
		}

		public override void Hiding ()
		{
		}

		int baddieType = 2;
		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);

			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			timeSinceLastBaddie += elapsed;

			if (timeSinceLastBaddie > 7) {
				CullBaddies ();
				Baddie baddie = new Baddie ();
				switch (baddieType) {
				case 0:
					baddie.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["spider"]) };
					baddie.Location = new Vector2 (1024, 550);
					baddie.Speed = new Vector2 (-100, 0);
					break;
				case 1:
					baddie.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["cicken"]) };
					baddie.Location = new Vector2 (1024, 500);
					baddie.Speed = new Vector2 (-25, 0);
					break;
				case 2:
					baddie.Sprites = new GameSprite[] { 
						new GameSprite (spriteSheet, spriteRects ["toy-1"]),
						new GameSprite (spriteSheet, spriteRects ["toy-2"]),
						new GameSprite (spriteSheet, spriteRects ["toy-3"]) 
					};
					baddie.Location = new Vector2 (1024, 570);
					baddie.Speed = new Vector2 (-25, 0);
					break;
				}
				baddies.Add (baddie);
				timeSinceLastBaddie = 0;
				baddieType = (baddieType + 1) % 3;
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
				spriteBatch.Draw (spriteSheet, loc, rect, Color.White);
				loc.X += 200;
			}
				
			drawFairyLight (spriteBatch);

			girl.Draw (gameTime, spriteBatch);
			helper.Draw (gameTime, spriteBatch);
			foreach (var baddie in baddies) {
				baddie.Draw (gameTime, spriteBatch);
			}
		}

		private void drawFairyLight(SpriteBatch spriteBatch) {
			var tint = new Color (1.0f, 1.0f, 1.0f, 0.75f);

			spriteBatch.Draw (spriteShadow, helper.Location + helper.deltaLocation - Vector2.One * 140.0f, tint);
			spriteBatch.Draw (
				spriteShadow, 
				new Rectangle (0, 0, (int)(helper.Location.X + helper.deltaLocation.X - 139), 1000), 
				new Rectangle (512, 0, 20, 20), 
				tint);
			spriteBatch.Draw (
				spriteShadow, 
				new Rectangle (
					(int)(helper.Location.X + helper.deltaLocation.X - 140), 
					0, 
					1500, 
					(int)(helper.Location.Y + helper.deltaLocation.Y - 139)), 
				new Rectangle (512, 0, 20, 20), 
				tint);
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

