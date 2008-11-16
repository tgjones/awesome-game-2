using System;
using SD.Shared;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace AwesomeGame2.Data
{
	public interface ILocationDataService
	{
		List<LocationInfo> GetLocations();
		LocationInfo GetLocation(int id);
		Texture2D GetResourceTexture(ResourceEnum resource);
	}
}
