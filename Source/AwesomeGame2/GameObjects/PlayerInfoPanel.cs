using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SD.Shared;
using AwesomeGame2.Data;
using System.Collections.Generic;

namespace AwesomeGame2.GameObjects
{
	public class PlayerInfoPanel : InfoPanel
	{
		private PlayerInfo _playerInfo;
		private List<PlayerInfo> _playerInfos;

		protected override string GetTitle()
		{
			return "Player - " + (_playerInfo == null ? "Not logged in" : _playerInfo.Name);
		}

		public PlayerInfoPanel(Game game)
			: base(game, game.GraphicsDevice.Viewport.Width - 210, 200)
		{
			IPlayerDataService playerData = this.Game.Services.GetService<IPlayerDataService>();
			_playerInfos = playerData.GetPlayers();
		}

		protected override void DrawDetail()
		{
			DrawString(_headingFont, "Login as:");
			foreach (PlayerInfo player in _playerInfos)
				DrawString(_paragraphFont, "   " + player.Email, true);
		}
	}
}
