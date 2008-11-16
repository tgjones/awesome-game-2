using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SD.Shared;
using AwesomeGame2.Data;

namespace AwesomeGame2.GameObjects
{
	public class RouteInfoPanel : InfoPanel
	{
		private Route _route;

		public int RouteID
		{
			get { return _route.RouteInfo.Id; }
		}

		public RouteInfoPanel(Game game, Route route)
			: base(game, route)
		{
			_route = route;
		}

		protected override void DrawDetail()
		{
			DrawString(_paragraphFont, "From: " + _route.RouteInfo.FromLocation.Name);
			DrawString(_paragraphFont, "To: " + _route.RouteInfo.ToLocation.Name);
			DrawString(_paragraphFont, null);
			DrawString(_paragraphFont, "Distance: " + _route.RouteInfo.Distance.ToString("F0") + " miles");
			DrawString(_paragraphFont, "Maximum Speed: " + _route.RouteInfo.Speed.ToString("F0") + " miles per hour");
			DrawString(_paragraphFont, "Condition: " + _route.RouteInfo.State.ToString("P"));
		}
	}
}
