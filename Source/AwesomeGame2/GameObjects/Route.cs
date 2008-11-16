using System;
using Microsoft.Xna.Framework;
using SD.Shared;
using Microsoft.Xna.Framework.Graphics;

namespace AwesomeGame2.GameObjects
{
	public class Route : DrawableGameComponent, IPickable
	{
		private BasicEffect lineEffect;
		private VertexDeclaration lineVertexDeclaration;
		private VertexPositionColor[] lineVertices;
		
		private LocationInfo _locationInfo1;
		private LocationInfo _locationInfo2;

		public LocationInfo LocationInfo1
		{
			get { return _locationInfo1; }
		}

		public LocationInfo LocationInfo2
		{
			get { return _locationInfo2; }
		}

		public Vector3[] Vertices
		{
			get;
			set;
		}

		public int PrimitiveStepCount
		{
			get { return 1; }
		}

		public string Name
		{
			get { return _locationInfo1.Name + " to " + _locationInfo2.Name; }
		}

		public Matrix World
		{
			get { return Matrix.Identity; }
		}

		public BoundingSphere BoundingSphere
		{
			get { return this.Game.Services.GetService<IGlobeService>().BoundingSphere; }
		}

		public Route(Game game, LocationInfo locationInfo1, LocationInfo locationInfo2)
			: base(game)
		{
			_locationInfo1 = locationInfo1;
			_locationInfo2 = locationInfo2;
		}

		protected override void LoadContent()
		{
			// create the effect and vertex declaration for drawing the
			// picked triangle.
			lineEffect = new BasicEffect(this.Game.GraphicsDevice, null);
			lineEffect.VertexColorEnabled = true;

			lineVertexDeclaration = new VertexDeclaration(this.Game.GraphicsDevice, VertexPositionColor.VertexElements);

			// Calculate location
			int lNumberOfSegments = 350;
			lineVertices = new VertexPositionColor[lNumberOfSegments];
			Vertices = new Vector3[lNumberOfSegments];

			for (int i = 0; i < lNumberOfSegments; i++)
			{
				float lPercentage = i / (lNumberOfSegments - 1.0f);
				Vector2 lLatLong1 = new Vector2((float)_locationInfo1.Latitude, (float)_locationInfo1.Longitude);
				Vector2 lLatLong2 = new Vector2((float)_locationInfo2.Latitude, (float)_locationInfo2.Longitude);
				Vector2 lLatLongI = Vector2.Lerp(lLatLong1, lLatLong2, lPercentage);

				float lWidth = (i % 2 == 0) ? 0.005f : -0.005f;

				float phi = lWidth + MathHelper.ToRadians(lLatLongI.X);
				float theta = lWidth + MathHelper.ToRadians(lLatLongI.Y);

				IGlobeService globe = this.Game.Services.GetService<IGlobeService>();
				float scale = globe.Radius * (float)Math.Cos(phi);
				float x = scale * (float)Math.Sin(theta);
				float y = globe.Radius * (float)Math.Sin(phi) * 1.03f; // to place 3% above surface of globe
				float z = scale * (float)Math.Cos(theta);

				// Populate vertices
				Vertices[i] = new Vector3(x, y, z);
				lineVertices[i] = new VertexPositionColor(Vertices[i], new Color(Color.Black, 0.5f));
			}

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			long lSelectedVertex = ((long)(gameTime.TotalGameTime.TotalMilliseconds / 10)) % lineVertices.Length;
			for (int i = 0; i < lineVertices.Length; i++)
			{
				float lMiddleness = 0.4f + Math.Max(0.2f - 0.01f * Math.Abs(i - lSelectedVertex), 0.0f);
				lineVertices[i].Color = new Color(1.0f, 0, 0, lMiddleness);
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			GraphicsDevice.RenderState.CullMode = CullMode.None;
			
			// Activate the line drawing BasicEffect.
			ICameraService camera = this.Game.Services.GetService<ICameraService>();
			lineEffect.Projection = camera.Projection;
			lineEffect.View = camera.View;

			lineEffect.Begin();
			lineEffect.CurrentTechnique.Passes[0].Begin();

			GraphicsDevice.VertexDeclaration = lineVertexDeclaration;
			GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, lineVertices, 0, lineVertices.Length - 2);

			lineEffect.CurrentTechnique.Passes[0].End();
			lineEffect.End();

			base.Draw(gameTime);
		}
	}
}
