using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AwesomeGame2
{
	public class Mesh : DrawableGameComponent
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
