using System;
using SD.Shared;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace AwesomeGame2.Data
{
	public interface IPlayerDataService
	{
		List<PlayerInfo> GetPlayers();
	}
}
