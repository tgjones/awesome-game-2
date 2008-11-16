using System;
using System.Collections.Generic;
using SD.Shared;
using System.Net;
using System.IO;

namespace AwesomeGame2.Data
{
	public class RouteData : IRouteDataService
	{
		private List<RouteInfo> _cachedLocations;

		public List<RouteInfo> GetRoutes()
		{
			if (_cachedLocations == null)
			{
				using (WebClient webClient = new WebClient())
				{
					Stream locationsStream = webClient.OpenRead("http://192.168.0.103:54321/routes");
					_cachedLocations = XmlHelper.DeserialiseRouteList(locationsStream);
				}
			}
			return _cachedLocations;
		}

		public RouteInfo GetRoute(int id)
		{
			using (WebClient webClient = new WebClient())
			{
				Stream locationsStream = webClient.OpenRead("http://192.168.0.103:54321/routes?id=" + id);
				return XmlHelper.DeserialiseRouteList(locationsStream)[0];
			}
		}
	}
}
