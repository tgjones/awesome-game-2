using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SD.Shared;
using AwesomeGame2.Data;

namespace AwesomeGame2.GameObjects
{
	public class LocationInfoPanel : InfoPanel
	{
		private LocationInfo _locationInfo;

		public int LocationID
		{
			get { return _locationInfo.Id; }
		}

		public LocationInfoPanel(Game game, Location location)
			: base(game, null)
		{
			_boundObject = location;

			ILocationDataService locationData = this.Game.Services.GetService<ILocationDataService>();
			_locationInfo = locationData.GetLocation(location.LocationID);
		}

		protected override void DrawDetail()
		{
			DrawString(_paragraphFont, _locationInfo.LocationType.ToString().Replace('_', ' '));
			DrawString(_paragraphFont, null);
			DrawString(_subHeadingFont, "Stocks");
			
			foreach (StockInfo stock in _locationInfo.Stocks)
				DrawString(_paragraphFont,
					stock.ResourceType.ToString() + " - " + stock.Quantity + " available - Cost " + stock.UnitPrice);
		}
	}
}
