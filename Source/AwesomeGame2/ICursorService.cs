using System;
using Microsoft.Xna.Framework;

namespace AwesomeGame2
{
	public interface ICursorService
	{
		Ray CalculateCursorRay(Matrix projectionMatrix, Matrix viewMatrix);
	}
}
