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

		string levelBAK  = "gggggggggggggggggggggggdhhhhhhhhhhhhhhhhHHHHHHHHHHHHHHHHBBBBBBBBBBBBBBBB";
		string levelOBJ  = "                                                                        ";
		string levelBAD  = "0                       1          2         3                   4      ";
		string levelTime = "8         6         5              4                                    ";

		Dictionary<string, Rectangle> girlRects;
		Texture2D girlSheet;

		Player girl = new Player ();
		PlayerHelper helper = new PlayerHelper ();
		List<Actor> baddies = new List<Actor>();

		float timeSinceLastBaddie = 0;

		public TheGameScreen(Game parent) : base(parent) { }

		public override void Showing ()
		{
			BackgroundColor = Color.Black;

			girlRects = TextureAtlas.Load ("girl");
			girlSheet = Content.Load<Texture2D> ("girl");
			spriteRects = TextureAtlas.Load ("Insomnia");
			spriteSheet = Content.Load<Texture2D> ("Insomnia");
			spriteShadow = Content.Load<Texture2D> ("shadow");

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
			girl.Location = new Vector2 (50, 425);
			girl.MoveSpeed = new Vector2 (200, 0);
			girl.Baddies = baddies;

			helper.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["fairy"]) };
			helper.Location = new Vector2 (50, 450);
			helper.Tint = Color.Yellow;
			helper.TrackActor = girl;
			helper.Baddies = baddies;
			helper.Attack = 60;

			Baddie aCookie = new Baddie ();
			aCookie.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["cookie"]) };
			aCookie.Location = new Vector2 (550, 500);
			aCookie.Attack = -1;
			baddies.Add (aCookie);
		}

		public override void Hiding ()
		{
		}

		float lastLocationX = 0;
		int baddieType = 0;
		int timePerBaddie = 4;
		int numBaddies = 1;
		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);

			if (girl.Health < 1) {
				ScreenUtil.Show (new CreditsScreen (Parent));
				return;
			}

			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			timeSinceLastBaddie += elapsed;

			if (timeSinceLastBaddie > timePerBaddie) {
				CullBaddies ();
				var addCookie = false;
				Baddie baddie = new Baddie ();
				switch (baddieType) {
				case 0:
					baddie.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["cicken"]) };
					baddie.Location = new Vector2 (1024, 450);
					baddie.Speed = new Vector2 (-75, 0);
					baddie.Health = 30;
					if (lastLocationX > girl.locWorld.X && girl.Health < 3) {
						lastLocationX = girl.locWorld.X;
						addCookie = true;
					}
					break;
				case 1:
					baddie.Sprites = new GameSprite[] { 
						new GameSprite (spriteSheet, spriteRects ["toy-1"]),
						new GameSprite (spriteSheet, spriteRects ["toy-2"]),
						new GameSprite (spriteSheet, spriteRects ["toy-3"]) 
					};
					baddie.Location = new Vector2 (1024, 520);
					baddie.Speed = new Vector2 (-50, 0);
					baddie.Health = 50;
					break;
				case 2:
					baddie.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["spider"]) };
					baddie.Location = new Vector2 (1024, 500);
					baddie.Speed = new Vector2 (-200, 0);
					baddie.Health = 20;
					break;
				case 3:
					baddie.Sprites = new GameSprite[] { 
						new GameSprite (spriteSheet, spriteRects ["slime-1"]),
						new GameSprite (spriteSheet, spriteRects ["slime-2"]),
						new GameSprite (spriteSheet, spriteRects ["slime-3"]), 
						new GameSprite (spriteSheet, spriteRects ["slime-4"]),
						new GameSprite (spriteSheet, spriteRects ["slime-5"]), 
						new GameSprite (spriteSheet, spriteRects ["slime-6"]) 
					};
					baddie.Location = new Vector2 (1024, 520);
					baddie.Speed = new Vector2 (-5, 0);
					baddie.Health = 60;
					break;
				case 4:
					baddie.Sprites = new GameSprite[] { 
						new GameSprite (spriteSheet, spriteRects ["slime-1"]),
						new GameSprite (spriteSheet, spriteRects ["slime-2"]),
						new GameSprite (spriteSheet, spriteRects ["slime-3"]), 
						new GameSprite (spriteSheet, spriteRects ["slime-4"]),
						new GameSprite (spriteSheet, spriteRects ["slime-5"]), 
						new GameSprite (spriteSheet, spriteRects ["slime-6"]) 
					};
					baddie.Location = new Vector2 (1024, 520);
					baddie.Speed = new Vector2 (-5, 0);
					baddie.Health = 60;
					break;
				}
				baddies.Add (baddie);
				timeSinceLastBaddie = 0;
				baddieType = (baddieType + 1) % numBaddies;

				if (addCookie) {
					Baddie aCookie = new Baddie ();
					aCookie.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["cookie"]) };
					aCookie.Location = new Vector2 (1024, 500);
					aCookie.Attack = -1;
					baddies.Add (aCookie);
				}
			}

			if (GamePadEx.WasJustPressed (PlayerIndex.One, Buttons.Back)) {
				ScreenUtil.Show (TitleScreen.Instance);
			} else if (GamePadEx.WasJustPressed (PlayerIndex.One, Buttons.Start)) {
				//ScreenUtil.Show (new CreditsScreen (Parent));
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
			int index = -(int)Math.Floor(loc.X / 200.0f) - 1;

			var lvlBAD = levelBAD [Math.Max (index, 0)];
			if (lvlBAD != ' ') {
				numBaddies = toNumber (lvlBAD) + 1;
			}

			var lvlTime = levelTime [Math.Max (index, 0)];
			if (lvlTime != ' ') {
				timePerBaddie = toNumber (lvlTime);
			}

			while (index < levelBAK.Length) {
				var rect = spriteRects ["grass"];
				switch (levelBAK [Math.Max (index, 0)]) {
				case 'g':
					rect = spriteRects ["grass"];
					break;
				case 'h':
					rect = spriteRects ["hall-1"];
					break;
				case 'H':
					rect = spriteRects ["hall-2"];
					break;
				case 'B':
					rect = spriteRects ["hall-3"];
					break;
				}
				loc.X = (float)Math.Round(girl.locWorld.X) + index * 200;
				spriteBatch.Draw (spriteSheet, loc, rect, Color.White);
				index++;
			}
		}

		private void drawFairyLight(SpriteBatch spriteBatch) {
			var loc = helper.Location + helper.deltaLocation;
			loc.X += helper.Sprites [0].TextureRect.Width / 2;
			loc.X -= spriteShadow.Bounds.Width / 2;
			loc.Y += helper.Sprites [0].TextureRect.Height / 2;
			loc.Y -= spriteShadow.Bounds.Height / 2;
			spriteBatch.Draw (spriteShadow, loc, Color.White);

		}

		private void drawPlayerHealth(SpriteBatch spriteBatch) {
			var rect = spriteRects ["cookie"];
			var loc = Vector2.One * 10;
			var locDelta = new Vector2 (rect.Width + 20, 0);
			for (int i = 0; i < 3; i++) {
				var tint = i < girl.Health ? Color.White : Color.Black;
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

		string toNumberLookup = "0123456789abcdefghijklmnopqrstuvwxyz";
		private int toNumber(char c) {
			return toNumberLookup.IndexOf (c.ToString().ToLower()[0]);
		}
	}
}

