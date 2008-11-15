using System;
using Microsoft.Xna.Framework;

namespace AwesomeGame2
{
	public static class ExtensionMethods
	{
		public static T GetService<T>(this GameServiceContainer container)
		{
			return (T) container.GetService(typeof(T));
		}
	}
}
