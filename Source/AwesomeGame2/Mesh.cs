using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace AwesomeGame2
{
	public class Mesh : DrawableGameComponent, IPickable
	{
		private string _assetName;

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

		public override void Draw(GameTime gameTime)
		{
			ICameraService camera = this.Game.Services.GetService<ICameraService>();

			Matrix[] transforms = new Matrix[this.Model.Bones.Count];
			this.Model.CopyAbsoluteBoneTransformsTo(transforms);
			foreach (ModelMesh mesh in this.Model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.World = transforms[mesh.ParentBone.Index] * this.World;
					effect.View = camera.View;
					effect.Projection = camera.Projection;

					Sunlight.ApplyToBasicEffect(effect);
					effect.SpecularColor = Vector3.Zero;
				}

				mesh.Draw();
			}

			base.Draw(gameTime);
		}
	}
}
