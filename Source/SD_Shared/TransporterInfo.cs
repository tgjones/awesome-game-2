using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SD.Shared
{
    public class TransporterInfo
    {
        List<StockInfo> _stocks = new List<StockInfo>();

        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int RouteId { get; set; }
        public DateTime LastMoved { get; set; }
        public decimal DistanceTravelled { get; set; }
        public int Capacity { get; set; }
        public int TransportTypeId { get; set; }
        public List<StockInfo> Stocks { get { return _stocks; } }

        public TransporterInfo(int id, int playerId, int routeId, DateTime lastMoved, decimal distanceTravelled, int capacity, int transportTypeId)
        {
            Id = id;
            PlayerId = playerId;
            RouteId = routeId;
            LastMoved = lastMoved;
            DistanceTravelled = distanceTravelled;
            Capacity = capacity;
            TransportTypeId = transportTypeId;
        }

        public TransporterInfo(int id, int playerId, int routeId, DateTime lastMoved, decimal distanceTravelled, int capacity, int transportTypeId, List<StockInfo> stocks)
            : this(id, playerId, routeId, lastMoved, distanceTravelled, capacity, transportTypeId)
        {
            _stocks.Clear();
            _stocks.AddRange(stocks);
        }
    }
}
