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

		public RouteInfo RouteInfo
		{
			get;
			set;
		}

		public bool IsSelected
		{
			get;
			set;
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
			get { return "Route " + RouteInfo.Id.ToString(); }
		}

		public Matrix World
		{
			get { return Matrix.Identity; }
		}

		public BoundingSphere BoundingSphere
		{
			get { return this.Game.Services.GetService<IGlobeService>().BoundingSphere; }
		}

		public Route(Game game, RouteInfo routeInfo)
			: base(game)
		{
			this.RouteInfo = routeInfo;
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
				Vector2 lLatLong1 = new Vector2((float)RouteInfo.FromLocation.Latitude, (float)RouteInfo.FromLocation.Longitude);
				Vector2 lLatLong2 = new Vector2((float)RouteInfo.ToLocation.Latitude, (float)RouteInfo.ToLocation.Longitude);
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
				lineVertices[i] = new VertexPositionColor(Vertices[i], Color.Black);
			}

			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{
			long lSelectedVertex = ((long)(gameTime.TotalGameTime.TotalMilliseconds / 10)) % lineVertices.Length;
			for (int i = 0; i < lineVertices.Length; i++)
			{
				float lMiddleness = 0.4f + Math.Max(0.2f - 0.01f * Math.Abs(i - lSelectedVertex), 0.0f);
				if (IsSelected)
					lineVertices[i].Color = new Color(1.0f, lMiddleness, lMiddleness, 1.0f);
				else
					lineVertices[i].Color = new Color(1.0f, 0.0f, 0.0f, lMiddleness);
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
