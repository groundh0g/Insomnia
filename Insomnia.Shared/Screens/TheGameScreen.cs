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

		Dictionary<string, Rectangle> girlRects;
		Texture2D girlSheet;

		Player girl = new Player ();
		PlayerHelper helper = new PlayerHelper ();
		List<Actor> baddies = new List<Actor>();
		Baddie aCookie = new Baddie ();

		float timeSinceLastBaddie = 0;

		public TheGameScreen(Game parent) : base(parent) { }

		public override void Showing ()
		{
			BackgroundColor = Color.Red;
			girlRects = TextureAtlas.Load ("girl");
			girlSheet = Content.Load<Texture2D> ("girl");
			spriteRects = TextureAtlas.Load ("Insomnia");
			spriteSheet = Content.Load<Texture2D> ("Insomnia");
			spriteShadow = Content.Load<Texture2D> ("shadow");

//			girl.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["girl"]) };
			var idleSprites = new List<GameSprite>() { 
				new GameSprite (girlSheet, girlRects ["Idle (1)"]),
				new GameSprite (girlSheet, girlRects ["Idle (2)"]),
				new GameSprite (girlSheet, girlRects ["Idle (3)"]),
				new GameSprite (girlSheet, girlRects ["Idle (4)"]),
				new GameSprite (girlSheet, girlRects ["Idle (5)"]),
				new GameSprite (girlSheet, girlRects ["Idle (6)"]),
				new GameSprite (girlSheet, girlRects ["Idle (7)"]),
				new GameSprite (girlSheet, girlRects ["Idle (8)"]),
				new GameSprite (girlSheet, girlRects ["Idle (9)"]),
				new GameSprite (girlSheet, girlRects ["Idle (10)"]),
			};
			var runSprites = new List<GameSprite>() { 
				new GameSprite (girlSheet, girlRects ["Run (1)"]),
				new GameSprite (girlSheet, girlRects ["Run (2)"]),
				new GameSprite (girlSheet, girlRects ["Run (3)"]),
				new GameSprite (girlSheet, girlRects ["Run (4)"]),
				new GameSprite (girlSheet, girlRects ["Run (5)"]),
				new GameSprite (girlSheet, girlRects ["Run (6)"]),
				new GameSprite (girlSheet, girlRects ["Run (7)"]),
				new GameSprite (girlSheet, girlRects ["Run (8)"]),
			};
			var jumpSprites = new List<GameSprite>() { 
				new GameSprite (girlSheet, girlRects ["Jump (1)"]),
				new GameSprite (girlSheet, girlRects ["Jump (2)"]),
				new GameSprite (girlSheet, girlRects ["Jump (3)"]),
				new GameSprite (girlSheet, girlRects ["Jump (4)"]),
				new GameSprite (girlSheet, girlRects ["Jump (5)"]),
				new GameSprite (girlSheet, girlRects ["Jump (6)"]),
				new GameSprite (girlSheet, girlRects ["Jump (7)"]),
				new GameSprite (girlSheet, girlRects ["Jump (8)"]),
				new GameSprite (girlSheet, girlRects ["Jump (9)"]),
				new GameSprite (girlSheet, girlRects ["Jump (10)"]),
			};
			girl.PlayerSprites = new Dictionary<Player.PlayerState, List<GameSprite>> ();
			girl.PlayerSprites.Add (Player.PlayerState.IDLE, idleSprites);
			girl.PlayerSprites.Add (Player.PlayerState.RUN, runSprites);
			girl.PlayerSprites.Add (Player.PlayerState.JUMP, jumpSprites);
			girl.Location = new Vector2 (50, 450);
			girl.MoveSpeed = new Vector2 (40, 0);
			girl.Baddies = baddies;

			helper.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["fairy"]) };
			helper.Location = new Vector2 (50, 475);
			helper.Tint = Color.Yellow;
			helper.TrackActor = girl;
			helper.Baddies = baddies;

			aCookie.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["cookie"]) };
			aCookie.Location = new Vector2 (550, 550);
			aCookie.Attack = -1;
			baddies.Add (aCookie);

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

			if (girl.Health < 1) {
				ScreenUtil.Show (new CreditsScreen (Parent));
				return;
			}

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

			drawBackground (spriteBatch);
				
			girl.Draw (gameTime, spriteBatch);
			foreach (var baddie in baddies) {
				baddie.Draw (gameTime, spriteBatch);
			}
			helper.Draw (gameTime, spriteBatch);

			drawFairyLight (spriteBatch);

			drawPlayerHealth (spriteBatch);
		}

		private void drawBackground(SpriteBatch spriteBatch) {
			var loc = girl.locWorld;
			var rect = spriteRects ["background"];

			while (loc.X < -rect.Width) {
				loc.X += rect.Width;
			}

			while (loc.X < GraphicsDevice.Viewport.Width) {
				spriteBatch.Draw (spriteSheet, loc, rect, Color.White);
				loc.X += 200;
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

		private void drawPlayerHealth(SpriteBatch spriteBatch) {
			var rect = spriteRects ["cookie"];
			var loc = Vector2.One * 10;
			var locDelta = new Vector2 (rect.Width + 20, 0);
			for (int i = 0; i < 3; i++) {
				var tint = i < girl.Health ? Color.White : Color.DarkGray;
				spriteBatch.Draw (
					spriteSheet, 
					new Rectangle((int)loc.X, (int)loc.Y, rect.Width, rect.Height),
					rect,
					tint);
				loc += locDelta;
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

