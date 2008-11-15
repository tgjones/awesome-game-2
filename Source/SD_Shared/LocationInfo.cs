using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SD.Shared
{
    public class LocationInfo
    {
        public int Id { get; private set; }
        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }
        public string Name { get; private set; }

        public List<StockInfo> Stocks { get; private set; }

        public LocationInfo(int id, decimal latitude, decimal longitude, string name)
        {
            this.Id = id;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Name = name;
            this.Stocks = new List<StockInfo>();
        }
    }


}
