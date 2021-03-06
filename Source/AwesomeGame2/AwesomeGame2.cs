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
using AwesomeGame2.Data;
using SD.Shared;
using AwesomeGame2.GameObjects;

namespace AwesomeGame2
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class AwesomeGame2 : Microsoft.Xna.Framework.Game
	{
		private static AwesomeGame2 _instance;
		private GraphicsDeviceManager _graphics;

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

			//_graphics.PreferredBackBufferWidth = 1680;
			//_graphics.PreferredBackBufferHeight = 1080;
			//_graphics.IsFullScreen = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			Input.MouseComponent mouse = new Input.MouseComponent(this);
			this.Services.AddService(typeof(Input.IMouseService), mouse);
			this.Components.Add(mouse);

			this.Components.Add(new Sun(this, 20.0f, 90.0f));

			this.Components.Add(new Starfield(this, 1500));

			Globe globe = new Globe(this, 100, 100, Vector3.Zero, -180, 180, -90, 90, 2);
			this.Components.Add(globe);
			this.Services.AddService(typeof(IGlobeService), globe);

			Picker picker = new Picker(this);
			this.Services.AddService(typeof(IPickerService), picker);
			this.Components.Add(picker);

			Cursor cursor = new Cursor(this, this.Content) { DrawOrder = 1000 };
			this.Services.AddService(typeof(ICursorService), cursor);
			this.Components.Add(cursor);

			Camera camera = new Camera(this);
			this.Services.AddService(typeof(ICameraService), camera);
			this.Components.Add(camera);

			ILocationDataService locationData = new LocationData();
			this.Services.AddService(typeof(ILocationDataService), locationData);
			List<LocationInfo> lLocations = locationData.GetLocations();

			foreach (LocationInfo lLocation in lLocations)
				this.Components.Add(new Location(this, lLocation));

			IRouteDataService routeData = new RouteData();
			this.Services.AddService(typeof(IRouteDataService), routeData);
			List<RouteInfo> lRoutes = routeData.GetRoutes();

			foreach (RouteInfo lRoute in lRoutes)
				this.Components.Add(new Route(this, lRoute));

			IPlayerDataService playerData = new PlayerData();
			this.Services.AddService(typeof(IPlayerDataService), playerData);
			this.Components.Add(new PlayerInfoPanel(this));

			base.Initialize();
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

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);
			base.Draw(gameTime);
		}
	}
}
