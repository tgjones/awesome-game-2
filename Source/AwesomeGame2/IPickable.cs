using System;
using Microsoft.Xna.Framework;

namespace AwesomeGame2
{
	public interface IPickable
	{
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
	}
}
