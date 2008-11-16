using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AwesomeGame2
{
	public class Mesh : DrawableGameComponent, IPickable
	{
		private string _assetName;
		private Effect _effect;

		public Effect Effect
		{
			get { return _effect; }
		}

		public bool IsSelected
		{
			get;
			set;
		}

		public Matrix World
		{
			get;
			set;
		}

		public Model Model
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			set;
		}

		public Texture Texture
		{
			get;
			set;
		}

		private Dictionary<string, object> TagData
		{
			get
			{
				// Look up our custom collision data from the Tag property of the model.
				Dictionary<string, object> tagData = (Dictionary<string, object>) this.Model.Tag;

				if (tagData == null)
				{
					throw new InvalidOperationException(
							"Model.Tag is not set correctly. Make sure your model " +
							"was built using the custom TrianglePickingProcessor.");
				}

				return tagData;
			}
		}

		public BoundingSphere BoundingSphere
		{
			get { return (BoundingSphere) this.TagData["BoundingSphere"]; }
		}

		public Vector3[] Vertices
		{
			get { return (Vector3[]) this.TagData["Vertices"]; }
		}

		public int PrimitiveStepCount
		{
			get { return 3; }
		}

		public Mesh(Game game, string assetName)
			: base(game)
		{
			this.World = Matrix.Identity;
			_assetName = assetName;
		}

		public override void Initialize()
		{
			this.Model = this.Game.Content.Load<Model>(_assetName);
			base.Initialize();
		}

		protected override void LoadContent()
		{
			_effect = this.Game.Content.Load<Effect>(@"Effects\Factory");
			base.LoadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			ICameraService camera = this.Game.Services.GetService<ICameraService>();
			Matrix wvp = this.World * camera.View * camera.Projection;

			Matrix[] transforms = new Matrix[this.Model.Bones.Count];
			this.Model.CopyAbsoluteBoneTransformsTo(transforms);

			_effect.Begin(SaveStateMode.SaveState);
			foreach (EffectPass effectPass in _effect.CurrentTechnique.Passes)
			{
				effectPass.Begin();
				
				foreach (ModelMesh mesh in this.Model.Meshes)
				{
					this.GraphicsDevice.Indices = mesh.IndexBuffer;
					foreach (ModelMeshPart meshPart in mesh.MeshParts)
					{
						Texture texture = null;
						if (this.Texture != null)
							texture = this.Texture;
						else
							texture = ((BasicEffect) meshPart.Effect).Texture;
						_effect.Parameters["Texture"].SetValue(texture);
						_effect.Parameters["TextureEnabled"].SetValue(((BasicEffect) meshPart.Effect).TextureEnabled);
						_effect.Parameters["DiffuseColour"].SetValue(((BasicEffect) meshPart.Effect).DiffuseColor);
						_effect.Parameters["WorldViewProjection"].SetValue(transforms[mesh.ParentBone.Index] * wvp);
						_effect.Parameters["InverseWorld"].SetValue(Matrix.Invert(this.World));
						_effect.Parameters["CullMode"].SetValue((int) CullMode.CullCounterClockwiseFace);
						_effect.CommitChanges();

						this.GraphicsDevice.VertexDeclaration = meshPart.VertexDeclaration;
						this.GraphicsDevice.Vertices[0].SetSource(mesh.VertexBuffer, 0, meshPart.VertexStride);
						this.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
							meshPart.BaseVertex, meshPart.StartIndex, meshPart.NumVertices,
							meshPart.StartIndex, meshPart.PrimitiveCount);
					}
				}

				effectPass.End();
			}
			_effect.End();

			base.Draw(gameTime);
		}
	}
}
