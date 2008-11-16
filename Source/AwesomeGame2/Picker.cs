using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using AwesomeGame2.GameObjects;

namespace AwesomeGame2
{
	public class Picker : DrawableGameComponent, IPickerService
	{
		#region Fields

		IPickable SelectedPickable;
		private bool SelectedPickableLocked;

		// To keep things efficient, the picking works by first applying a bounding
		// sphere test, and then only bothering to test each individual triangle
		// if the ray intersects the bounding sphere. This allows us to trivially
		// reject many models without even needing to bother looking at their triangle
		// data. This field keeps track of which models passed the bounding sphere
		// test, so you can see the difference between this approximation and the more
		// accurate triangle picking.
		private List<string> insideBoundingSpheres = new List<string>();

		// Store the name of the model underneath the cursor (or null if there is none).
		//private string pickedModelName;

		// Vertex array that stores exactly which triangle was picked.
		private VertexPositionColor[] pickedRadius =
        {
            new VertexPositionColor(Vector3.Zero, Color.Tomato),
            new VertexPositionColor(Vector3.Zero, Color.Tomato),
        };

		// Vertex array that stores exactly which triangle was picked.
		private VertexPositionColor[] pickedTriangle =
        {
            new VertexPositionColor(Vector3.Zero, Color.Magenta),
            new VertexPositionColor(Vector3.Zero, Color.Magenta),
            new VertexPositionColor(Vector3.Zero, Color.Magenta),
        };

		// Effect and vertex declaration for drawing the picked triangle.
		private BasicEffect lineEffect;
		private VertexDeclaration lineVertexDeclaration;

		#endregion

		#region Properties

		public Vector3 PickedRadius
		{
			get
			{
				Vector3 lOutput = pickedRadius[1].Position;
				lOutput.Normalize();
				return lOutput;
			}
		}

		#endregion

		public Picker(Game game)
			: base(game)
		{
			this.UpdateOrder = 1;
		}

		#region Methods

		protected override void LoadContent()
		{
			// create the effect and vertex declaration for drawing the
			// picked triangle.
			lineEffect = new BasicEffect(this.Game.GraphicsDevice, null);
			lineEffect.VertexColorEnabled = true;

			lineVertexDeclaration = new VertexDeclaration(this.Game.GraphicsDevice, VertexPositionColor.VertexElements);

			base.LoadContent();
		}

		/// <summary>
		/// Runs a per-triangle picking algorithm over all the models in the scene,
		/// storing which triangle is currently under the cursor.
		/// </summary>
		public override void Update(GameTime gameTime)
		{
			ICameraService camera = this.Game.Services.GetService<ICameraService>();
			ICursorService cursor = this.Game.Services.GetService<ICursorService>();

			// Look up a collision ray based on the current cursor position. See the
			// Picking Sample documentation for a detailed explanation of this.
			Ray cursorRay = cursor.CalculateCursorRay(camera.Projection, camera.View);

			// Clear the previous picking results.
			insideBoundingSpheres.Clear();

			IPickable pickedPickable = null;
			pickedRadius[1].Position = Vector3.Zero;

			// Keep track of the closest object we have seen so far, so we can
			// choose the closest one if there are several models under the cursor.
			float closestIntersection = float.MaxValue;

			// Loop over all our models.
			IEnumerable<IPickable> pickables = this.Game.Components.OfType<IPickable>();
			foreach (IPickable pickable in pickables)
			{
				bool insideBoundingSphere;
				Vector3 vertex1, vertex2, vertex3;

				// Perform the ray to model intersection test.
				float? intersection = RayIntersectsModel(cursorRay, pickable,
																								 pickable.World,
																								 out insideBoundingSphere,
																								 out vertex1, out vertex2,
																								 out vertex3);

				// If this model passed the initial bounding sphere test, remember
				// that so we can display it at the top of the screen.
				if (insideBoundingSphere)
					insideBoundingSpheres.Add(pickable.Name);

				// Do we have a per-triangle intersection with this model?
				if (intersection != null)
				{
					// If so, is it closer than any other model we might have
					// previously intersected?
					if (intersection < closestIntersection)
					{
						// Store information about this model.
						closestIntersection = intersection.Value;

						pickedPickable = pickable;

						// Store vertex positions so we can display the picked triangle.
						pickedTriangle[0].Position = vertex1;
						pickedTriangle[1].Position = vertex2;
						pickedTriangle[2].Position = vertex3;

						// Store intersection point positions so we can display the picked radius
						pickedRadius[1].Position = cursorRay.Direction * 2000.0f;
					}
				}
			}

			Input.IMouseService lMouseService = this.Game.Services.GetService<Input.IMouseService>();

			if (lMouseService.LeftClickPressed || SelectedPickable is Globe)
				SelectedPickableLocked = false;

			if (SelectedPickable != null && !SelectedPickableLocked)
			{
				SelectedPickable.IsSelected = false;
				SelectedPickable = null;
			}

			if (pickedPickable != null && !SelectedPickableLocked)
			{
				pickedPickable.IsSelected = true;
				SelectedPickable = pickedPickable;

				if (lMouseService.LeftClickPressed)
					SelectedPickableLocked = true;
			}

			if (SelectedPickable != null && SelectedPickable is Location)
			{
				LocationInfoPanel locationInfoPanel = this.Game.Components.OfType<LocationInfoPanel>().SingleOrDefault();
				bool add = true;
				if (locationInfoPanel != null)
				{
					if (locationInfoPanel.LocationID != ((Location)SelectedPickable).LocationID)
						this.Game.Components.Remove(locationInfoPanel);
					else
						add = false;
				}
				if (add)
				{
					LocationInfoPanel panel = new LocationInfoPanel(this.Game, ((Location)SelectedPickable).LocationID);
					this.Game.Components.Add(panel);
				}
			}
			else
			{
				this.Game.Components.Remove(this.Game.Components.OfType<LocationInfoPanel>().SingleOrDefault());
			}

			if (SelectedPickable != null && SelectedPickable is Route)
			{
				RouteInfoPanel panel = this.Game.Components.OfType<RouteInfoPanel>().SingleOrDefault();
				bool add = true;
				if (panel != null)
				{
					if (panel.RouteID != ((Route)SelectedPickable).RouteInfo.Id)
						this.Game.Components.Remove(panel);
					else
						add = false;
				}
				if (add)
				{
					panel = new RouteInfoPanel(this.Game, ((Route)SelectedPickable));
					this.Game.Components.Add(panel);
				}
			}
			else
			{
				this.Game.Components.Remove(this.Game.Components.OfType<RouteInfoPanel>().SingleOrDefault());
			}

			base.Update(gameTime);
		}


		/// <summary>
		/// Checks whether a ray intersects a model. This method needs to access
		/// the model vertex data, so the model must have been built using the
		/// custom TrianglePickingProcessor provided as part of this sample.
		/// Returns the distance along the ray to the point of intersection, or null
		/// if there is no intersection.
		/// </summary>
		private static float? RayIntersectsModel(Ray ray, IPickable pickable, Matrix modelTransform,
																		 out bool insideBoundingSphere,
																		 out Vector3 vertex1, out Vector3 vertex2,
																		 out Vector3 vertex3)
		{
			vertex1 = vertex2 = vertex3 = Vector3.Zero;

			// The input ray is in world space, but our model data is stored in object
			// space. We would normally have to transform all the model data by the
			// modelTransform matrix, moving it into world space before we test it
			// against the ray. That transform can be slow if there are a lot of
			// triangles in the model, however, so instead we do the opposite.
			// Transforming our ray by the inverse modelTransform moves it into object
			// space, where we can test it directly against our model data. Since there
			// is only one ray but typically many triangles, doing things this way
			// around can be much faster.

			Matrix inverseTransform = Matrix.Invert(modelTransform);

			ray.Position = Vector3.Transform(ray.Position, inverseTransform);
			ray.Direction = Vector3.TransformNormal(ray.Direction, inverseTransform);

			// Start off with a fast bounding sphere test.
			if (pickable.BoundingSphere.Intersects(ray) == null)
			{
				// If the ray does not intersect the bounding sphere, we cannot
				// possibly have picked this model, so there is no need to even
				// bother looking at the individual triangle data.
				insideBoundingSphere = false;

				return null;
			}
			else
			{
				// The bounding sphere test passed, so we need to do a full
				// triangle picking test.
				insideBoundingSphere = true;

				// Keep track of the closest triangle we found so far,
				// so we can always return the closest one.
				float? closestIntersection = null;

				// Loop over the vertex data, 3 at a time (3 vertices = 1 triangle).
				Vector3[] vertices = pickable.Vertices;
				for (int i = 0; i < vertices.Length - 2; i += pickable.PrimitiveStepCount)
				{
					// Perform a ray to triangle intersection test.
					float? intersection;

					RayIntersectsTriangle(ref ray,
																ref vertices[i],
																ref vertices[i + 1],
																ref vertices[i + 2],
																out intersection);

					// Does the ray intersect this triangle?
					if (intersection != null)
					{
						// If so, is it closer than any other previous triangle?
						if ((closestIntersection == null) ||
								(intersection < closestIntersection))
						{
							// Store the distance to this triangle.
							closestIntersection = intersection;

							// Transform the three vertex positions into world space,
							// and store them into the output vertex parameters.
							Vector3.Transform(ref vertices[i],
																ref modelTransform, out vertex1);

							Vector3.Transform(ref vertices[i + 1],
																ref modelTransform, out vertex2);

							Vector3.Transform(ref vertices[i + 2],
																ref modelTransform, out vertex3);
						}
					}
				}

				return closestIntersection;
			}
		}


		/// <summary>
		/// Checks whether a ray intersects a triangle. This uses the algorithm
		/// developed by Tomas Moller and Ben Trumbore, which was published in the
		/// Journal of Graphics Tools, volume 2, "Fast, Minimum Storage Ray-Triangle
		/// Intersection".
		/// 
		/// This method is implemented using the pass-by-reference versions of the
		/// XNA math functions. Using these overloads is generally not recommended,
		/// because they make the code less readable than the normal pass-by-value
		/// versions. This method can be called very frequently in a tight inner loop,
		/// however, so in this particular case the performance benefits from passing
		/// everything by reference outweigh the loss of readability.
		/// </summary>
		private static void RayIntersectsTriangle(ref Ray ray,
																			ref Vector3 vertex1,
																			ref Vector3 vertex2,
																			ref Vector3 vertex3, out float? result)
		{
			// Compute vectors along two edges of the triangle.
			Vector3 edge1, edge2;

			Vector3.Subtract(ref vertex2, ref vertex1, out edge1);
			Vector3.Subtract(ref vertex3, ref vertex1, out edge2);

			// Compute the determinant.
			Vector3 directionCrossEdge2;
			Vector3.Cross(ref ray.Direction, ref edge2, out directionCrossEdge2);

			float determinant;
			Vector3.Dot(ref edge1, ref directionCrossEdge2, out determinant);

			// If the ray is parallel to the triangle plane, there is no collision.
			if (determinant > -float.Epsilon && determinant < float.Epsilon)
			{
				result = null;
				return;
			}

			float inverseDeterminant = 1.0f / determinant;

			// Calculate the U parameter of the intersection point.
			Vector3 distanceVector;
			Vector3.Subtract(ref ray.Position, ref vertex1, out distanceVector);

			float triangleU;
			Vector3.Dot(ref distanceVector, ref directionCrossEdge2, out triangleU);
			triangleU *= inverseDeterminant;

			// Make sure it is inside the triangle.
			if (triangleU < 0 || triangleU > 1)
			{
				result = null;
				return;
			}

			// Calculate the V parameter of the intersection point.
			Vector3 distanceCrossEdge1;
			Vector3.Cross(ref distanceVector, ref edge1, out distanceCrossEdge1);

			float triangleV;
			Vector3.Dot(ref ray.Direction, ref distanceCrossEdge1, out triangleV);
			triangleV *= inverseDeterminant;

			// Make sure it is inside the triangle.
			if (triangleV < 0 || triangleU + triangleV > 1)
			{
				result = null;
				return;
			}

			// Compute the distance along the ray to the triangle.
			float rayDistance;
			Vector3.Dot(ref edge2, ref distanceCrossEdge1, out rayDistance);
			rayDistance *= inverseDeterminant;

			// Is the triangle behind the ray origin?
			if (rayDistance < 0)
			{
				result = null;
				return;
			}

			result = rayDistance;
		}

		public override void Draw(GameTime gameTime)
		{
			/*GraphicsDevice device = this.Game.GraphicsDevice;
			RenderState renderState = device.RenderState;

			// Set line drawing renderstates. We disable backface culling
			// and turn off the depth buffer because we want to be able to
			// see the picked triangle outline regardless of which way it is
			// facing, and even if there is other geometry in front of it.
			renderState.FillMode = FillMode.WireFrame;
			renderState.CullMode = CullMode.None;
			renderState.DepthBufferEnable = false;

			// Activate the line drawing BasicEffect.
			ICameraService camera = this.Game.Services.GetService<ICameraService>();
			lineEffect.Projection = camera.Projection;
			lineEffect.View = camera.View;

			lineEffect.Begin();
			lineEffect.CurrentTechnique.Passes[0].Begin();

			// Draw the triangle.
			device.VertexDeclaration = lineVertexDeclaration;

			device.DrawUserPrimitives(PrimitiveType.TriangleList, pickedTriangle, 0, 1);
			//device.DrawUserPrimitives(PrimitiveType.LineList, pickedRadius, 0, 1);

			lineEffect.CurrentTechnique.Passes[0].End();
			lineEffect.End();

			// Reset renderstates to their default values.
			renderState.FillMode = FillMode.Solid;
			renderState.CullMode = CullMode.None;
			renderState.DepthBufferEnable = true;*/

			base.Draw(gameTime);
		}

		#endregion
	}
}