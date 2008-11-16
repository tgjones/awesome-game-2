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
		private int _width;
		private int x;
		private int y;
		private int imageX;

		protected SpriteFont _titleBarFont, _headingFont, _subHeadingFont, _paragraphFont;

		public InfoPanel(Game game, int x, int width)
			: base(game)
		{
			this.x = x;
			_width = width;
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

			imageX = 0;
			y = 31;
			
			_spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
			_spriteBatch.DrawString(_titleBarFont, GetTitle(), new Vector2(x + 3, 10), new Color(Color.White, _alpha));
			DrawDetail();
			_spriteBatch.Draw(_whitePixelTexture, new Rectangle(x, 10, _width, 18), new Color(0.6f, 0.6f, 0.6f, _alpha * 0.3f));
			_spriteBatch.Draw(_whitePixelTexture, new Rectangle(x, 30, _width, y - 27), new Color(0.6f, 0.6f, 0.6f, _alpha * 0.3f));
			_spriteBatch.End();

			base.Draw(gameTime);
		}

		protected void FinishImages(int height)
		{
			if (imageX != 0)
			{
				y += height + 4;
				imageX = 0;
			}
		}

		protected void DrawImage(Texture2D image, int width, int height)
		{
			if (imageX + width > _width)
				FinishImages(height);

			_spriteBatch.Draw(image, new Rectangle(imageX + x + 4, y, width, height), new Color(Color.White, _alpha));

			imageX += width + 4;
		}

		protected void DrawString(SpriteFont font, string text)
		{
			DrawString(font, text, false);
		}

		protected void DrawString(SpriteFont font, string text, bool enableSelection)
		{
			Color lDrawColor = new Color(Color.White, _alpha);
			if (enableSelection)
			{
				Input.IMouseService lMouseService = this.Game.Services.GetService<Input.IMouseService>();
				if (
					lMouseService.X > x &&
					lMouseService.X < (x + _width) &&
					lMouseService.Y > y &&
					lMouseService.Y < (y + font.LineSpacing))
				{
					lDrawColor = new Color(Color.PowderBlue, _alpha); // hover
				}
			}

			if (!string.IsNullOrEmpty(text))
				_spriteBatch.DrawString(font, text, new Vector2(x + 4, y), lDrawColor);

			y += font.LineSpacing;
		}

		protected abstract string GetTitle();
		protected abstract void DrawDetail();
	}
}
