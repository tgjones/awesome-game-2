using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AwesomeGame2
{
    public class Camera : GameComponent, ICameraService
	{
        private float _radius;
        private float _targetRadius;
        private Vector3 _pickedRadius;
        private Vector2 _position;

		#region Properties

        public Vector3 LookAt
		{
			get;
			set;
		}

		/// <summary>
		/// Perspective aspect ratio.
		/// </summary>
		public float AspectRatio
		{
			get;
			set;
		}

		/// <summary>
		/// Perspective field of view.
		/// </summary>
		public float FieldOfView
		{
			get;
			set;
		}

		/// <summary>
		/// Distance to the near clipping plane.
		/// </summary>
		public float NearPlaneDistance
		{
			get;
			set;
		}

		/// <summary>
		/// Distance to the far clipping plane.
		/// </summary>
		public float FarPlaneDistance
		{
			get;
			set;
		}

		public Matrix View
		{
			get;
			private set;
		}

		public Matrix Projection
		{
			get;
			private set;
		}

		#endregion

		#region Constructor

		public Camera(Game game) : base (game)
		{
			Rectangle clientBounds = AwesomeGame2.Instance.Window.ClientBounds;
			this.AspectRatio = clientBounds.Width / (float) clientBounds.Height;

			this.FieldOfView = MathHelper.ToRadians(45.0f);

			this.NearPlaneDistance = 1.0f;
			this.FarPlaneDistance = 10000.0f;

            this._targetRadius = this._radius = 100.0f;
		}

		#endregion

		#region Methods

        public override void Update(GameTime gameTime)
        {
            Input.IMouseService lMouseService = this.Game.Services.GetService<Input.IMouseService>();
            Picker lPicker = this.Game.Services.GetService<Picker>();

            // Update the zoom target level
            _targetRadius -= 0.05f * lMouseService.ScrollWheelValueChange; // sensitivity
            if (_targetRadius < 3.0f)
                _targetRadius = 3.0f; // closest zoom radius
            if (_targetRadius > 6.0f)
                _targetRadius = 6.0f; // furthest zoom radius

            // Update the zoom
            _radius += Math.Min(gameTime.ElapsedGameTime.Ticks * 2.0f, TimeSpan.TicksPerSecond) * (_targetRadius - _radius) / TimeSpan.TicksPerSecond; // lag speed
            
            // Update the dragged position
            if (!lMouseService.RightClickPressed || float.IsNaN(lPicker.PickedRadius.Length()))
            {
                _pickedRadius = Vector3.Zero;
            }
            else if (_pickedRadius.Length() == 0.0f)
            {
                _pickedRadius = lPicker.PickedRadius;
            }
            else
            {
                Vector3 lDifference = lPicker.PickedRadius - _pickedRadius;
                lDifference = Vector3.Transform(lDifference, this.View) * (float)Math.Sqrt(_radius) / 4.0f;

                _position.X -= lDifference.X;
                _position.Y += lDifference.Y;

                _pickedRadius = lPicker.PickedRadius;
            }

            Matrix lMatrix =
                Matrix.CreateRotationX(_position.Y) *
                Matrix.CreateRotationY(_position.X);

            Vector3 lPosition = Vector3.Transform(Vector3.Backward * _radius, lMatrix);
            Vector3 lUp = Vector3.Transform(Vector3.Up, lMatrix);
            
            this.View = Matrix.CreateLookAt(lPosition, this.LookAt, lUp);
			this.Projection = Matrix.CreatePerspectiveFieldOfView(this.FieldOfView,
				this.AspectRatio, this.NearPlaneDistance, this.FarPlaneDistance);
		}

		#endregion
	}
}
