using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SD.Shared
{
    public class TransporterInfo
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int RouteId { get; set; }
        public DateTime LastMoved { get; set; }
        public decimal DistanceTravelled { get; set; }
        public int Capacity { get; set; }
        public int TransportTypeId { get; set; }
        public List<StockInfo> Stocks { get; set; }

        public TransporterInfo()
        {
            Stocks = new List<StockInfo>();
        }

        public TransporterInfo(int id, int playerId, int routeId, DateTime lastMoved, decimal distanceTravelled, int capacity, int transportTypeId)
            : this()
        {
            Id = id;
            PlayerId = playerId;
            RouteId = routeId;
            LastMoved = lastMoved;
            DistanceTravelled = distanceTravelled;
            Capacity = capacity;
            TransportTypeId = transportTypeId;
        }

    }
}
