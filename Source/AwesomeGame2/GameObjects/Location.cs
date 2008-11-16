using System;
using Microsoft.Xna.Framework;
using SD.Shared;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using AwesomeGame2.Data;

namespace AwesomeGame2.GameObjects
{
	public class Location : DrawableGameComponent, IPickable
	{
		#region Fields

		private LocationInfo _locationInfo;
		private Mesh _locationMesh;
		private List<Mesh> _stockMeshes;
		private Matrix _translationScaling;
		private Matrix _world;

		#endregion

		#region Properties

		public bool IsSelected
		{
			get;
			set;
		}

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
			get { return _locationMesh.BoundingSphere.Transform(_translationScaling); }
		}

		public Vector3[] Vertices
		{
			get { return _locationMesh.Vertices; }
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
			string locationMeshAssetName = _locationInfo.LocationType.ToString();
			try
			{
				_locationMesh = new Mesh(this.Game, @"Models\LocationTypes\" + locationMeshAssetName);
				_locationMesh.Initialize();
			}
			catch
			{
#warning Remove this once all location type meshes are available
				_locationMesh = new Mesh(this.Game, @"Models\LocationTypes\City");
				_locationMesh.Initialize();
			}

			_stockMeshes = new List<Mesh>();
			foreach (StockInfo stock in _locationInfo.Stocks)
			{
				Mesh stockMesh = new Mesh(this.Game, @"Models\StockSign");
				ILocationDataService locationData = this.Game.Services.GetService<ILocationDataService>();
				stockMesh.Texture = locationData.GetResourceTexture(stock.ResourceType);
				stockMesh.Initialize();
				_stockMeshes.Add(stockMesh);
			}

			// Calculate location.
			float phi = MathHelper.ToRadians((float) _locationInfo.Latitude);
			float theta = MathHelper.ToRadians((float) _locationInfo.Longitude);

			IGlobeService globe = this.Game.Services.GetService<IGlobeService>();
			float scale = globe.Radius * (float) Math.Cos(phi);
			float x = scale * (float) Math.Sin(theta);
			float y = globe.Radius * (float) Math.Sin(phi) * 1.04f; // to place 4% above surface of globe;
			float z = scale * (float) Math.Cos(theta);

			_translationScaling = Matrix.CreateScale(.08f)
				* Matrix.CreateTranslation(x, y, z);
			_world = Matrix.CreateRotationX(-phi)
				* Matrix.CreateRotationZ(-theta)
				* Matrix.CreateRotationX(MathHelper.PiOver2)
				* _translationScaling;

			base.Initialize();
		}

		public override void Draw(GameTime gameTime)
		{
			_locationMesh.World = _world;
			_locationMesh.Effect.Parameters["IsSelected"].SetValue(this.IsSelected);
			_locationMesh.Draw(gameTime);

			if (_locationInfo.LocationType != LocationEnum.City)
			{
				int row = 0; int column = 0;
				for (int i = 0; i < _stockMeshes.Count; i++)
				{
					Mesh stockMesh = _stockMeshes[i];
					stockMesh.World = Matrix.CreateRotationY(MathHelper.Pi)
						* Matrix.CreateTranslation(-1.2f + (column * 1.2f), 0, 1.2f + (row * 1.2f))
						* _world;
					stockMesh.Draw(gameTime);

					if (i > 0 && (i + 1) % 3 == 0)
					{
						++row;
						column = 0;
					}
					else
					{
						++column;
					}
				}
			}

			base.Draw(gameTime);
		}
	}
}
