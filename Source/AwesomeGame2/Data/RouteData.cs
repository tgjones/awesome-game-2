using System;
using System.Collections.Generic;
using SD.Shared;
using System.Net;
using System.IO;
using System.Configuration;

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
					try
					{
						Stream locationsStream = webClient.OpenRead(ConfigurationManager.AppSettings["Server"] + "/routes");
						_cachedLocations = XmlHelper.DeserialiseRouteList(locationsStream);
					}
					catch
					{
						_cachedLocations = new List<RouteInfo>();
					}
				}
			}
			return _cachedLocations;
		}

		public RouteInfo GetRoute(int id)
		{
			using (WebClient webClient = new WebClient())
			{
				Stream locationsStream = webClient.OpenRead(ConfigurationManager.AppSettings["Server"] + "/routes?id=" + id);
				return XmlHelper.DeserialiseRouteList(locationsStream)[0];
			}
		}
	}
}
