using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SD.Shared
{
    public class LocationInfo
    {
        int _id;
        decimal _latitude;
        decimal _longitude;
        string _name;
        List<StockInfo> _stocks = new List<StockInfo>();

        public int Id { get { return _id; } }
        public decimal Latitude { get { return _latitude; } }
        public decimal Longitude { get { return _longitude; } }
        public string Name { get { return _name; } }
        public List<StockInfo> Stocks { get { return _stocks; } }

        public LocationInfo(int id, decimal latitude, decimal longitude, string name)
        {
            _id = id;
            _latitude = latitude;
            _longitude = longitude;
            _name = name;
        }

        public LocationInfo(int id, decimal latitude, decimal longitude, string name, List<StockInfo> stocks)
            : this(id, latitude, longitude, name)
        {
            _stocks.Clear();
            _stocks.AddRange(stocks);
        }
    }


}
