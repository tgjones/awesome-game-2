using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SD.Shared;
using AwesomeGame2.Data;

namespace AwesomeGame2.GameObjects
{
	public abstract class InfoPanel : DrawableGameComponent
	{
		private SpriteBatch _spriteBatch;
		private Texture2D _whitePixelTexture;
		private float _alpha;
		private int y;

		protected SpriteFont _titleBarFont, _headingFont, _subHeadingFont, _paragraphFont;
		protected IPickable _boundObject;

		public InfoPanel(Game game, IPickable boundObject)
			: base(game)
		{
			_boundObject = boundObject;
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(this.GraphicsDevice);
			_whitePixelTexture = this.Game.Content.Load<Texture2D>(@"Textures\WhitePixel");
			_titleBarFont = this.Game.Content.Load<SpriteFont>(@"Fonts\Titlebar");
			_headingFont = this.Game.Content.Load<SpriteFont>(@"Fonts\Heading");
			_subHeadingFont = this.Game.Content.Load<SpriteFont>(@"Fonts\SubHeading");
			_paragraphFont = this.Game.Content.Load<SpriteFont>(@"Fonts\Paragraph");
			base.LoadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			if (_alpha < 1.0f)
				_alpha += 2.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (_alpha > 1.0f)
				_alpha = 1.0f;

			y = 31;
			
			_spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
			_spriteBatch.DrawString(_titleBarFont, _boundObject.GetType().Name + " - " + _boundObject.Name, new Vector2(13, 10), new Color(Color.White, _alpha));
			DrawString(_headingFont, _boundObject.Name);
			DrawDetail();
			_spriteBatch.Draw(_whitePixelTexture, new Rectangle(10, 10, 300, 18), new Color(0.6f, 0.6f, 0.6f, _alpha * 0.3f));
			_spriteBatch.Draw(_whitePixelTexture, new Rectangle(10, 30, 300, y - 27), new Color(0.6f, 0.6f, 0.6f, _alpha * 0.3f));
			_spriteBatch.End();

			base.Draw(gameTime);
		}

		protected void DrawString(SpriteFont font, string text)
		{
			if (!string.IsNullOrEmpty(text))
				_spriteBatch.DrawString(font, text, new Vector2(14, y), new Color(Color.White, _alpha));

			y += font.LineSpacing;
		}

		protected abstract void DrawDetail();
	}
}
