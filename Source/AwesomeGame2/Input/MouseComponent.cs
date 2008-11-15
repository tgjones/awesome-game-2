using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AwesomeGame2.Input
{
	public class MouseComponent : GameComponent
	{
		private MouseState _previousMouseState;

		public MouseComponent(Game game)
			: base(game)
		{
			
		}

		public override void Update(GameTime gameTime)
		{
			_previousMouseState = Mouse.GetState();

			base.Update(gameTime);
		}
	}
}
