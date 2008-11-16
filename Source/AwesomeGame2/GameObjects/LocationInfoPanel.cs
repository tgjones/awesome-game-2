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

		protected override string GetTitle()
		{
			return "Location - " + _locationInfo.Name;
		}

		public LocationInfoPanel(Game game, Location location)
			: base(game, 10, 276)
		{
			ILocationDataService locationData = this.Game.Services.GetService<ILocationDataService>();
			_locationInfo = locationData.GetLocation(location.LocationID);
		}

		protected override void DrawDetail()
		{
			DrawString(_headingFont, _locationInfo.Name);
			DrawString(_paragraphFont, _locationInfo.LocationType.ToString().Replace('_', ' '));
			DrawString(_paragraphFont, null);
			DrawString(_subHeadingFont, "Stocks");

			ILocationDataService locationData = this.Game.Services.GetService<ILocationDataService>();
			
			foreach (StockInfo stock in _locationInfo.Stocks)
				DrawString(_paragraphFont, stock.ResourceType.ToString() + " - " + stock.Quantity + " available - Cost " + stock.UnitPrice);

			foreach (StockInfo stock in _locationInfo.Stocks)
				DrawImage(locationData.GetResourceTexture(stock.ResourceType), 64, 64);

			FinishImages(64);
		}
	}
}
