using System;
using Microsoft.Xna.Framework;

namespace AwesomeGame2
{
	public interface ICameraService
	{
		Matrix View
		{
			get;
		}

		Matrix Projection
		{
			get;
		}
	}
}
