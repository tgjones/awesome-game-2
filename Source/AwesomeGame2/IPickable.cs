using System;
using Microsoft.Xna.Framework;

namespace AwesomeGame2
{
	public interface IPickable
	{
		bool IsSelected
		{
			get;
			set;
		}

		string Name
		{
			get;
		}

		Matrix World
		{
			get;
		}

		BoundingSphere BoundingSphere
		{
			get;
		}

		Vector3[] Vertices
		{
			get;
		}

		int PrimitiveStepCount
		{
			get;
		}
	}
}
