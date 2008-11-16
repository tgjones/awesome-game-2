using System;
using Microsoft.Xna.Framework;

namespace AwesomeGame2
{
	public interface IPickerService
	{
		Vector3 PickedRadius
		{
			get;
		}

		string PickedModelName
		{
			get;
		}
	}
}
