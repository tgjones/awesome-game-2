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

		[Obsolete("I've made this obsolete specially to make Jason happy", false)]
		string PickedModelName
		{
			get;
		}
	}
}
