using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Insomnia.Shared
{
	public class Baddie : Actor
	{

        public new bool isBoss { get; set; }

		public Baddie () : base()
		{
            isBoss = false;
		}

		public override void Update (GameTime gameTime, Game Parent)
		{
			base.Update (gameTime, Parent);
		}

		public override void Draw (GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw (gameTime, spriteBatch);
		}

        public override void OnDeath(GameTime gametime, Game Parent)
        {
            if (this.isBoss)
            {
                MoreOnCode.Xna.Lib.Util.ScreenUtil.Show(new CreditsScreen(Parent));
            }
        }
    }
}

