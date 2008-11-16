using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SD.Shared
{
    public class LocationInfo
    {
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Name { get; set; }
        public LocationEnum LocationType { get; set; }
        public List<StockInfo> Stocks { get; set; }

        public LocationInfo()
        {
            Stocks = new List<StockInfo>();
        }

        public LocationInfo(int id, decimal latitude, decimal longitude, string name, LocationEnum locationType)
            : this()
        {
            Id = id;
            Latitude = latitude;
            Longitude = longitude;
            Name = name;
            LocationType = locationType;
        }


    }


}
