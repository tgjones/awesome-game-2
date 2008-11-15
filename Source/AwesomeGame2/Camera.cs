﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AwesomeGame2
{
    public class Camera : GameComponent, ICameraService
	{
        private float _targetRadius;
        private Vector3 _position;

		#region Properties

		public Vector3 Position
		{
			get { return _position; }
            set { _position = value; _targetRadius = _position.Length(); }
		}

		public Vector3 LookAt
		{
			get;
			set;
		}

		public Vector3 Up
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

			this.Up = Vector3.Up;
		}

		#endregion

		#region Methods

        public override void Update(GameTime gameTime)
        {
            Input.IMouseService lMouseService = this.Game.Services.GetService<Input.IMouseService>();
            System.Diagnostics.Debug.WriteLine(lMouseService.ScrollWheelValueChange);

            // Update the zoom target level
            _targetRadius -= 0.05f * lMouseService.ScrollWheelValueChange; // sensitivity
            if (_targetRadius < 2.5f)
                _targetRadius = 2.5f; // closest zoom radius

            // Update the zoom
            float lRadius = _position.Length() + Math.Min(gameTime.ElapsedRealTime.Milliseconds / 250.0f, 1.0f) * (_targetRadius - _position.Length()); // lag speed
            _position.Normalize();
            _position *= lRadius;

			this.View = Matrix.CreateLookAt(this.Position, this.LookAt, this.Up);
			this.Projection = Matrix.CreatePerspectiveFieldOfView(this.FieldOfView,
				this.AspectRatio, this.NearPlaneDistance, this.FarPlaneDistance);
		}

		#endregion
	}
}
