using System;
using Microsoft.Xna.Framework;

namespace AwesomeGame2
{
	public interface IGlobeService
	{
		float Radius
		{
			get;
		}

		BoundingSphere BoundingSphere
		{
			get;
		}
	}
}
