using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AwesomeGame2.Input
{
	public class MouseComponent : GameComponent, IMouseService
	{
		private MouseState _previousMouseState;
        private MouseState _currentMouseState;

        public MouseComponent(Game game)
            : base(game)
        {
            _previousMouseState = Mouse.GetState();
            _currentMouseState = Mouse.GetState();
        }

        public int ScrollWheelValueChange
        {
            get { return _currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue; }
        }

				public bool LeftClickPressed
				{
					get { return _currentMouseState.LeftButton == ButtonState.Pressed; }
				}

				public bool RightClickPressed
        {
            get { return _currentMouseState.RightButton == ButtonState.Pressed; }
        }

        public override void Update(GameTime gameTime)
		{
			_previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

			base.Update(gameTime);
		}
	}
}
