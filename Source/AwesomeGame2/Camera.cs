﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AwesomeGame2
{
	public class Camera
	{
		#region Properties

		public Vector3 Position
		{
			get;
			set;
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

		public Camera()
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

		public void UpdateMatrices()
		{
			this.View = Matrix.CreateLookAt(this.Position, this.LookAt, this.Up);
			this.Projection = Matrix.CreatePerspectiveFieldOfView(this.FieldOfView,
				this.AspectRatio, this.NearPlaneDistance, this.FarPlaneDistance);
		}

		#endregion
	}
}