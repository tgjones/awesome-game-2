using System;
using SD.Shared;
using System.Collections.Generic;

namespace AwesomeGame2.Data
{
	public interface ILocationDataService
	{
		List<LocationInfo> GetLocations();
		LocationInfo GetLocation(int id);
	}
}
