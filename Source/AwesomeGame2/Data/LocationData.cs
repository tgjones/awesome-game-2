﻿using System;
using System.Collections.Generic;
using SD.Shared;
using System.Net;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Configuration;

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
					try
					{
						Stream locationsStream = webClient.OpenRead(ConfigurationManager.AppSettings["Server"] + "/locations");
						_cachedLocations = XmlHelper.DeserialiseLocationList(locationsStream);
					}
					catch
					{
						_cachedLocations = new List<LocationInfo>();
					}
				}
			}
			return _cachedLocations;
		}

		public LocationInfo GetLocation(int id)
		{
			using (WebClient webClient = new WebClient())
			{
				Stream locationsStream = webClient.OpenRead(ConfigurationManager.AppSettings["Server"] + "/locations?id=" + id);
				return XmlHelper.DeserialiseLocationList(locationsStream)[0];
			}
		}
		
		public Texture2D GetResourceTexture(ResourceEnum resourceType)
		{
			return AwesomeGame2.Instance.Content.Load<Texture2D>(@"Textures\Icons\" + resourceType.ToString());
		}
	}
}
