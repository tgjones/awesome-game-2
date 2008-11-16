using System;
using Microsoft.Xna.Framework;
using SD.Shared;

namespace AwesomeGame2.GameObjects
{
	public class Location : DrawableGameComponent, IPickable
	{
		#region Fields

		private LocationInfo _locationInfo;
		private Mesh _simpleFactory;
		private Matrix _translationScaling;
		private Matrix _world;

		#endregion

		#region Properties

		public string Name
		{
			get { return _locationInfo.Name; }
		}

		public Matrix World
		{
			get { return _world; }
		}

		public BoundingSphere BoundingSphere
		{
			get { return _simpleFactory.BoundingSphere.Transform(_translationScaling); }
		}

		public Vector3[] Vertices
		{
			get { return _simpleFactory.Vertices; }
		}

		public int PrimitiveStepCount
		{
			get { return 3; }
		}

		public int LocationID
		{
			get { return _locationInfo.Id; }
		}

		#endregion

		public Location(Game game, LocationInfo locationInfo)
			: base(game)
		{
			_locationInfo = locationInfo;
		}

		public override void Initialize()
		{
			_simpleFactory = new Mesh(this.Game, @"Models\simplefactory");
			_simpleFactory.Initialize();

			// Calculate location.
			float phi = MathHelper.ToRadians((float) _locationInfo.Latitude);
			float theta = MathHelper.ToRadians((float) _locationInfo.Longitude);

			IGlobeService globe = this.Game.Services.GetService<IGlobeService>();
			float scale = globe.Radius * (float) Math.Cos(phi);
			float x = scale * (float) Math.Sin(theta);
			float y = globe.Radius * (float) Math.Sin(phi);
			float z = scale * (float) Math.Cos(theta);

			_translationScaling = Matrix.CreateScale(.1f)
				* Matrix.CreateTranslation(x, y, z);
			_world = Matrix.CreateRotationX(-phi)
				* Matrix.CreateRotationZ(-theta)
				* Matrix.CreateRotationX(MathHelper.PiOver2)
				* _translationScaling;

			base.Initialize();
		}

		public override void Draw(GameTime gameTime)
		{
			_simpleFactory.World = _world;
			_simpleFactory.Draw(gameTime);

			base.Draw(gameTime);
		}
	}
}
