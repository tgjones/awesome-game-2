using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SD.Shared;
using AwesomeGame2.Data;

namespace AwesomeGame2.GameObjects
{
	public class RouteInfoPanel : DrawableGameComponent
	{
		private SpriteBatch _spriteBatch;
		private Texture2D _whitePixelTexture;
		private SpriteFont _headingFont, _subHeadingFont, _paragraphFont;

		private LocationInfo _locationInfo1;
		private LocationInfo _locationInfo2;

		public int LocationID1
		{
			get { return _locationInfo1.Id; }
		}

		public int LocationID2
		{
			get { return _locationInfo2.Id; }
		}

		public RouteInfoPanel(Game game, LocationInfo locationInfo1, LocationInfo locationInfo2)
			: base(game)
		{
			_locationInfo1 = locationInfo1;
			_locationInfo2 = locationInfo2;
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(this.GraphicsDevice);
			_whitePixelTexture = this.Game.Content.Load<Texture2D>(@"Textures\WhitePixel");
			_headingFont = this.Game.Content.Load<SpriteFont>(@"Fonts\Heading");
			_subHeadingFont = this.Game.Content.Load<SpriteFont>(@"Fonts\SubHeading");
			_paragraphFont = this.Game.Content.Load<SpriteFont>(@"Fonts\Paragraph");
			base.LoadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			_spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
			_spriteBatch.Draw(_whitePixelTexture, new Rectangle(10, 10, 260, 150), new Color(0.6f, 0.6f, 0.6f, 0.3f));
			_spriteBatch.DrawString(_headingFont, "Route", new Vector2(20, 20), Color.White);
			_spriteBatch.DrawString(_paragraphFont, _locationInfo1.Name, new Vector2(20, 70), Color.LightGray);
			_spriteBatch.DrawString(_paragraphFont, "to" + _locationInfo2.Name, new Vector2(20, 100), Color.LightGray);
			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
