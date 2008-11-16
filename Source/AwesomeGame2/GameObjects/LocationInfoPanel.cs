using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SD.Shared;
using AwesomeGame2.Data;

namespace AwesomeGame2.GameObjects
{
	public class LocationInfoPanel : DrawableGameComponent
	{
		private SpriteBatch _spriteBatch;
		private Texture2D _whitePixelTexture;
		private SpriteFont _headingFont, _subHeadingFont, _paragraphFont;

		private int _locationID;
		private LocationInfo _locationInfo;

		public int LocationID
		{
			get { return _locationID; }
		}

		public LocationInfoPanel(Game game, int locationID)
			: base(game)
		{
			_locationID = locationID;
		}

		public override void Initialize()
		{
			ILocationDataService locationData = this.Game.Services.GetService<ILocationDataService>();
			_locationInfo = locationData.GetLocation(_locationID);
			base.Initialize();
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
			_spriteBatch.Draw(_whitePixelTexture, new Rectangle(10, 10, 260, 300), new Color(0.6f, 0.6f, 0.6f, 0.3f));
			_spriteBatch.DrawString(_headingFont, _locationInfo.Name, new Vector2(20, 20), Color.White);
			_spriteBatch.DrawString(_paragraphFont, "This is the location", new Vector2(20, 70), Color.LightGray);
			_spriteBatch.DrawString(_paragraphFont, "description. Blah blah blah.", new Vector2(20, 100), Color.LightGray);
			_spriteBatch.DrawString(_subHeadingFont, "Stocks", new Vector2(20, 150), Color.White);
			int y = 170;
			foreach (StockInfo stock in _locationInfo.Stocks)
				_spriteBatch.DrawString(_paragraphFont, stock.ResourceType.ToString() + " - " + stock.Quantity + " available - Cost " + stock.UnitPrice, new Vector2(20, y += 30), Color.LightGray);
			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
