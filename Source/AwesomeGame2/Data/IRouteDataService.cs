using System;
using SD.Shared;
using System.Collections.Generic;

namespace AwesomeGame2.Data
{
	public interface IRouteDataService
	{
		List<RouteInfo> GetRoutes();
		RouteInfo GetRoute(int id);
	}
}
