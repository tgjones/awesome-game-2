﻿using System;
using System.Collections.Generic;
using SD.Shared;
using System.Net;
using System.IO;

namespace AwesomeGame2.Data
{
	public class LocationData : ILocationDataService
	{
		private List<LocationInfo> _cachedLocations;

		public List<LocationInfo> GetLocations()
		{
			if (_cachedLocations == null)
			{
				using (WebClient webClient = new WebClient())
				{
					Stream locationsStream = webClient.OpenRead("http://192.168.0.103:54321/locations");
					_cachedLocations = XmlHelper.DeserialiseLocationList(locationsStream);
				}
			}
			return _cachedLocations;
		}

		public LocationInfo GetLocation(int id)
		{
			using (WebClient webClient = new WebClient())
			{
				Stream locationsStream = webClient.OpenRead("http://192.168.0.103:54321/location?id=" + id);
				return XmlHelper.DeserialiseLocationList(locationsStream)[0];
			}
		}
	}
}
