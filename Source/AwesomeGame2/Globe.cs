using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using AwesomeGame2.Data;
using SD.Shared;

namespace AwesomeGame2
{
	public class Globe : DrawableGameComponent, IPickable, IGlobeService
	{
		#region Fields

		private int _slices, _stacks;
		private Vector3 _centre;
		private float _longitudeFrom, _longitudeTo;
		private float _latitudeFrom, _latitudeTo;
		private float _radius;

		private int _numberOfVertices, _numberOfIndices;
		private Vector3[] _vertices;

		private Effect _effect;
		private VertexDeclaration _vertexDeclaration;
		private VertexBuffer _vertexBuffer;
		private IndexBuffer _indexBuffer;

		#endregion

		#region Properties

		public Matrix World
		{
			get;
			set;
		}

		public string Name
		{
			get { return "Globe"; }
		}

		public BoundingSphere BoundingSphere
		{
			get { return new BoundingSphere(_centre, _radius); }
		}

		public Vector3[] Vertices
		{
			get { return _vertices; }
		}

		public float Radius
		{
			get { return _radius; }
		}

		#endregion

		#region Constructor

		public Globe(Game game,
			int slices, int stacks, Vector3 centre,
			float longitudeFrom, float longitudeTo,
			float latitudeFrom, float latitudeTo,
			float radius)
			: base(game)
		{
			_slices = slices;
			_stacks = stacks;
			_centre = centre;
			_longitudeFrom = longitudeFrom;
			_longitudeTo = longitudeTo;
			_latitudeFrom = latitudeFrom;
			_latitudeTo = latitudeTo;
			_radius = radius;

			this.World = Matrix.Identity;
		}

		#endregion

		#region Methods

		protected override void LoadContent()
		{
			VertexPositionNormalTexture[] vertices = CreateVertexBuffer();
			CreateIndexBuffer(vertices);

			_effect = this.Game.Content.Load<Effect>(@"Effects\Globe");
			_vertexDeclaration = new VertexDeclaration(this.GraphicsDevice, VertexPositionNormalTexture.VertexElements);

			_effect.Parameters["PlanetTexture"].SetValue(this.Game.Content.Load<Texture2D>(@"Textures\earth"));
			_effect.Parameters["DetailTexture"].SetValue(this.Game.Content.Load<Texture2D>(@"Textures\Noise"));

			base.LoadContent();
		}

		private VertexPositionNormalTexture[] CreateVertexBuffer()
		{
			_numberOfVertices = (_stacks + 1) * (_slices + 1);
			VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[_numberOfVertices];

			int vertexIndex = 0;
			for (int stack = 0; stack <= _stacks; stack++)
			{
				float phi = (MathHelper.PiOver2) - (stack * MathHelper.Pi / _stacks);
				float y = _radius * (float) Math.Sin(phi);
				float scale = _radius * (float) Math.Cos(phi);

				for (int slice = 0; slice <= _slices; slice++)
				{
					float theta = slice * MathHelper.TwoPi / _slices;
					float x = scale * (float) Math.Sin(theta);
					float z = scale * (float) Math.Cos(theta);

					Vector3 normal = new Vector3(x, y, z);
					vertices[vertexIndex++] = new VertexPositionNormalTexture(
						normal + _centre,
						normal,
						new Vector2((float) slice / _slices, (float) stack / _stacks));
				}
			}

			_vertexBuffer = new VertexBuffer(
				this.GraphicsDevice,
				typeof(VertexPositionNormalTexture),
				vertices.Length,
				BufferUsage.None);
			_vertexBuffer.SetData<VertexPositionNormalTexture>(vertices);

			return vertices;
		}

		private void CreateIndexBuffer(VertexPositionNormalTexture[] vertices)
		{
			List<short> indices = new List<short>();

			for (short lat = 0; lat < _stacks; lat++)
			{
				for (short lng = 0; lng <= _slices; lng++)
				{
					if (lng == 0 && lat != 0)
						indices.Add((short) ((lat * (_slices + 1)) + lng));

					indices.Add((short) ((lat * (_slices + 1)) + lng));
					indices.Add((short) ((lat + 1) * (_slices + 1) + lng));

					if (lng == _slices && lat != _stacks - 1)
						indices.Add((short) ((lat + 1) * (_slices + 1) + lng));
				}
			}

			_numberOfIndices = indices.Count;
			short[] indicesArray = indices.ToArray();

			_indexBuffer = new IndexBuffer(
				this.GraphicsDevice,
				typeof(short),
				indicesArray.Length,
				BufferUsage.None);
			_indexBuffer.SetData<short>(indicesArray);

			_vertices = new Vector3[(_numberOfIndices - 2) * 3];
			int index = 0, indexIndex = 0;
			_vertices[index++] = vertices[indices[indexIndex++]].Position;
			_vertices[index++] = vertices[indices[indexIndex++]].Position;
			_vertices[index++] = vertices[indices[indexIndex++]].Position;
			while (indexIndex < indices.Count)
			{
				_vertices[index++] = vertices[indices[indexIndex - 2]].Position;
				_vertices[index++] = vertices[indices[indexIndex - 1]].Position;
				_vertices[index++] = vertices[indices[indexIndex - 0]].Position;
				++indexIndex;
			}
		}

		public override void Draw(GameTime gameTime)
		{
			ICameraService camera = this.Game.Services.GetService<ICameraService>();
			_effect.Parameters["WorldViewProjection"].SetValue(this.World * camera.View * camera.Projection);

			this.GraphicsDevice.VertexDeclaration = _vertexDeclaration;
            
			_effect.Begin(SaveStateMode.SaveState);
			foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
			{
				pass.Begin();
				this.GraphicsDevice.Vertices[0].SetSource(_vertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes);
				this.GraphicsDevice.Indices = _indexBuffer;
				this.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleStrip, 0, 0, _numberOfVertices, 0, _numberOfIndices - 2);
				pass.End();
			}
			_effect.End();
		}

		#endregion
	}
}