using System;
using System.Collections.Generic;
using SD.Shared;
using System.Net;
using System.IO;

namespace AwesomeGame2.Data
{
	public class PlayerData : IPlayerDataService
	{
		private List<PlayerInfo> _cachedLocations;

		public List<PlayerInfo> GetPlayers()
		{
			if (_cachedLocations == null)
			{
				using (WebClient webClient = new WebClient())
				{
					try
					{
						Stream locationsStream = webClient.OpenRead("http://192.168.0.105:54321/players");
						_cachedLocations = XmlHelper.DeserialisePlayerList(locationsStream);
					}
					catch
					{
						_cachedLocations = new List<PlayerInfo>();
					}
				}
			}
			return _cachedLocations;
		}

		public LoginInfo GetLoginInfo(PlayerInfo player)
		{
			using (WebClient webClient = new WebClient())
			{
				Stream locationsStream = webClient.OpenRead("http://192.168.0.105:54321/login?email=" + player.Email + "&password=password");
				return XmlHelper.DeserialiseLoginInfo(locationsStream);
			}
		}
	}
}
