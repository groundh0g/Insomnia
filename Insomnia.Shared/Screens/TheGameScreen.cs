using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
		Texture2D spriteBoss;

		public static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect> ();
		public static Song song;

		string levelBAK  = "gggggggggggggggggggggggdhhhhhhhhhhhhhhhhHHHHHHHHHHHHHHHHBBBBBBBBBBBBBBBB";
		string levelOBJ  = "                              0      1      2      3      4             ";
		string levelBAD  = "0                       1          2         3                   4      ";
		string levelTime = "8         6         5              4                                    ";

		Dictionary<string, Rectangle> girlRects;
		Texture2D girlSheet;

		/*Player girl = new Player ();
		PlayerHelper helper = new PlayerHelper ();*/
	    List<Actor> baddies = new List<Actor>();
        public List<Player> girls = new List<Player>();
        public List<PlayerHelper> helpers = new List<PlayerHelper>();

		float timeSinceLastBaddie = 0;

		public TheGameScreen(Game parent) : base(parent) { }

        Int16 pcount = 4;

		public override void Showing ()
		{
			BackgroundColor = Color.Black;

			girlRects = TextureAtlas.Load ("girl");
			girlSheet = Content.Load<Texture2D> ("girl");
			spriteRects = TextureAtlas.Load ("Insomnia");
			spriteSheet = Content.Load<Texture2D> ("Insomnia");
			spriteShadow = Content.Load<Texture2D> ("shadow");
			spriteBoss = Content.Load<Texture2D> ("eilrahc");

            girls = new List<Player>()
            {
                new Player (),
                new Player (),
                new Player (),
                new Player ()
            };

            helpers = new List<PlayerHelper>()
            {
                new PlayerHelper (),
                new PlayerHelper (),
                new PlayerHelper (),
                new PlayerHelper ()
            };

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
            var i = 0;
            foreach (var girl in girls)
            {
                girl.PlayerSprites = new Dictionary<Player.PlayerState, List<GameSprite>>();

                girl.PlayerSprites.Add(Player.PlayerState.IDLE, idleSprites);
                girl.PlayerSprites.Add(Player.PlayerState.RUN, runSprites);
                girl.PlayerSprites.Add(Player.PlayerState.JUMP, jumpSprites);
				girl.Location = new Vector2(50, 425);
                girl.MoveSpeed = new Vector2(200, 0);
                girl.Baddies = baddies;
                girl.PlayerIndex = (PlayerIndex)i;

				var helper = helpers[i];

                helper.Sprites = new GameSprite[] { new GameSprite(spriteSheet, spriteRects["fairy"]) };
                helper.Location = new Vector2(50, 450);
                var tints = new List<Color>()
                {
                    new Color(1.0f, 1.0f, 1.0f),
                    new Color(1.0f, 1.0f, 0.0f),
                    new Color(0.0f, 1.0f, 1.0f),
                    new Color(1.0f, 0.0f, 1.0f)
                };
                helper.Tint = tints[i];
                helper.TrackActor = girl;
                helper.Baddies = baddies;
                helper.Attack = 60;
                i++;
            }
			Baddie aCookie = new Baddie ();
			aCookie.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["cookie"]) };
			aCookie.Location = new Vector2 (550, 500);
			aCookie.Attack = -1;
			baddies.Add (aCookie);

			sounds.Clear ();
			sounds.Add ("cluck", Content.Load<SoundEffect> ("cluck"));
			sounds.Add ("chicken-death", Content.Load<SoundEffect> ("chicken-death"));
			sounds.Add ("clank", Content.Load<SoundEffect> ("clank"));
			sounds.Add ("get-the-girl", Content.Load<SoundEffect> ("get-the-girl"));
			sounds.Add ("unicorn", Content.Load<SoundEffect> ("unicorn"));
			sounds.Add ("unicorn-death", Content.Load<SoundEffect> ("unicorn-death"));
			sounds.Add ("slime", Content.Load<SoundEffect> ("slime"));
			sounds.Add ("spider", Content.Load<SoundEffect> ("spider"));
			sounds.Add ("girl-hit", Content.Load<SoundEffect> ("girl-hit"));
			sounds.Add ("girl-death", Content.Load<SoundEffect> ("girl-death"));

//			song = Content.Load<Song> ("game-music");
//			MediaPlayer.IsRepeating = true;
//			MediaPlayer.Volume = 1.0f;
//			MediaPlayer.Play (song);
		}

		public override void Hiding ()
		{
		}

		float lastLocationX = 0;
		int baddieType = 0;
		int timePerBaddie = 7;
		int numBaddies = 1;
		public override void Update (GameTime gameTime)
		{
			base.Update (gameTime);

            pcount = 4;

            foreach (var girl in girls)
            {
				if (girl.Health < 1 || girl.IsActive == false)
                {
                    pcount--;
                }
            }

            if (pcount == 0)
            {
                ScreenUtil.Show(new CreditsScreen(Parent));
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
			timeSinceLastBaddie += elapsed;

			if (timeSinceLastBaddie > timePerBaddie) {
				CullBaddies ();
				var addCookie = false;
				Baddie baddie = new Baddie ();
				switch (baddieType) {
				case 0:
					baddie.Sprites = new GameSprite[] { 
						new GameSprite (spriteSheet, spriteRects ["chicken-1"]),
						new GameSprite (spriteSheet, spriteRects ["chicken-2"]),
						new GameSprite (spriteSheet, spriteRects ["chicken-3"]), 
						new GameSprite (spriteSheet, spriteRects ["chicken-4"]) 
					};
					baddie.Location = new Vector2 (1024, 380);
					baddie.Speed = new Vector2 (-75, 0);
					baddie.Health = 30;
					baddie.GruntMp3 = "cluck";
					baddie.DeathMp3 = "chicken-death";
					foreach (Player girl in girls) {
						if (girl.Health < 2) {
							addCookie = true;
						}
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
					baddie.GruntMp3 = "get-the-girl";
					baddie.DeathMp3 = "clank";
					break;
				case 2:
					baddie.Sprites = new GameSprite[] { new GameSprite (spriteSheet, spriteRects ["spider"]) };
					baddie.Location = new Vector2 (1024, 500);
					baddie.Speed = new Vector2 (-200, 0);
					baddie.Health = 20;
					//baddie.GruntMp3 = "spider";
					baddie.DeathMp3 = "spider";
					break;
				case 3:
					baddie.Sprites = new GameSprite[] { 
						new GameSprite (spriteSheet, spriteRects ["Slime-1"]),
						new GameSprite (spriteSheet, spriteRects ["Slime-2"]),
						new GameSprite (spriteSheet, spriteRects ["Slime-3"]), 
						new GameSprite (spriteSheet, spriteRects ["Slime-4"]),
						new GameSprite (spriteSheet, spriteRects ["Slime-5"]), 
						new GameSprite (spriteSheet, spriteRects ["Slime-6"]) 
					};
					baddie.Location = new Vector2 (1024, 520);
					baddie.Speed = new Vector2 (-40, 0);
					baddie.Health = 55;
					baddie.GruntMp3 = "slime";
					baddie.DeathMp3 = "slime";
					break;
				case 4:
					baddie.Sprites = new GameSprite[] { 
						new GameSprite (spriteBoss, spriteBoss.Bounds)
					};
					baddie.Location = new Vector2 (1024, 235);
					baddie.Speed = new Vector2 (-5, 0);
					baddie.Health = 500;
					baddie.isBoss = true;
					baddie.GruntMp3 = "unicorn";
					baddie.DeathMp3 = "unicorn-death";
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

			if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back)) {
				ScreenUtil.Show (TitleScreen.Instance);
			} else if (GamePadEx.WasJustPressed (PlayerIndex.One, Buttons.Start)) {
				//ScreenUtil.Show (new CreditsScreen (Parent));
			} else {
                for (var i = 0; i < girls.Count; i++)
                {
                    if (girls[i].IsActive)
                    {
                        girls[i].Update(gameTime, Parent);
                        helpers[i].Update(gameTime, Parent);
                    }
                }
                foreach (var baddie in baddies) {
					baddie.Update (gameTime, Parent);
				}
			}
		}

		public override void Draw (GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw (gameTime, spriteBatch);

			drawBackground (spriteBatch);

            for (var i = 0; i < girls.Count; i++)
            {
                if (girls[i].IsActive)
                {
                    girls[i].Draw(gameTime, spriteBatch);
                    helpers[i].Draw(gameTime, spriteBatch);
                }
            }
            foreach (var baddie in baddies)
            {
                baddie.Draw(gameTime, spriteBatch);
            }

            drawFairyLight (spriteBatch);

			drawPlayerHealth (spriteBatch);
		}

		Vector2? endOfWorld = null;
		private void drawBackground(SpriteBatch spriteBatch) {
			if (endOfWorld.HasValue) {
				girls [0].locWorld = endOfWorld.Value;
				baddieType = 4;
			}
			var loc = girls[0].locWorld;
			int index = -(int)Math.Floor(loc.X / 200.0f) - 1;

			var lvlBAD = levelBAD [Math.Max (index, 0)];
			if (lvlBAD != ' ') {
				numBaddies = toNumber (lvlBAD) + 1;
			}

			var lvlTime = levelTime [Math.Max (index, 0)];
			if (lvlTime != ' ') {
				timePerBaddie = toNumber (lvlTime);
			}

			if (index == levelBAK.Length - 5 && endOfWorld == null) {
				endOfWorld = girls [0].locWorld;
			}

			while (index < levelBAK.Length) {
				var lvlOBJ = levelOBJ [Math.Max (index, 0)];
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
				loc.X = (float)Math.Round(girls[0].locWorld.X) + index * 200;
				spriteBatch.Draw (spriteSheet, loc, rect, Color.White);

				if (lvlOBJ != ' ') {
					var locDelta = new Vector2 (-100,100);
					spriteBatch.Draw(spriteSheet, loc + locDelta, spriteRects["portrait-" + (toNumber(lvlOBJ) + 1)], Color.White);
				}

				index++;
			}
		}

		private void drawFairyLight(SpriteBatch spriteBatch) {
			
            int _locx = 0,
                _locy = 0;

            foreach (var helper in helpers)
            {
				if (helper.TrackActor.IsActive) {
					_locx += (int)helper.Location.X - helper.Sprites [0].TextureRect.Width / 2;
					_locy += (int)helper.Location.Y - helper.Sprites [0].TextureRect.Height / 2;
				}
            }

            _locx /= 4;
            _locy /= 4;

            Vector2 loc = new Vector2(_locx - 2000, _locy - 2000);

			spriteBatch.Draw (spriteShadow, loc, Color.White);

		}

		private void drawPlayerHealth(SpriteBatch spriteBatch) {
            foreach (var girl in girls)
            {
                var rect = spriteRects["cookie"];
                var loc = Vector2.One * 10;
                var locDelta = new Vector2(rect.Width + 20, 0);
                for (int i = 0; i < 3; i++)
                {
                    if (GamePadEx.GetState(girl.PlayerIndex).IsConnected && girl.IsActive)
                    {
                        var tint = i < girl.Health ? Color.White : Color.DarkGray;
                        spriteBatch.Draw(
                            spriteSheet,
                            new Rectangle((int)loc.X, (int)loc.Y + (int)girl.PlayerIndex * (rect.Width + 20), rect.Width, rect.Height),
                            rect,
                            tint);
                        loc += locDelta;
                    }
                }
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

