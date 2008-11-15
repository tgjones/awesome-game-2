using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AwesomeGame2
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class AwesomeGame2 : Microsoft.Xna.Framework.Game
	{
		private static AwesomeGame2 _instance;

		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private Camera _camera;
		private Model _test0;

		public static AwesomeGame2 Instance
		{
			get
			{
				if (_instance == null)
					_instance = new AwesomeGame2();
				return _instance;
			}
		}

		private AwesomeGame2()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			_camera = new Camera();
			_camera.Position = new Vector3(10, 10, 10);
			_camera.LookAt = Vector3.Zero;

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			_test0 = this.Content.Load<Model>("Globe");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			_camera.UpdateMatrices();

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			Matrix[] transforms = new Matrix[_test0.Bones.Count];
			_test0.CopyAbsoluteBoneTransformsTo(transforms);
			foreach (ModelMesh mesh in _test0.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.World = transforms[mesh.ParentBone.Index];
					effect.View = _camera.View;
					effect.Projection = _camera.Projection;
				}

				mesh.Draw();
			}

			base.Draw(gameTime);
		}
	}
}
