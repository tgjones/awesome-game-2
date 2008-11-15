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
        public int CommodityId { get; set; }
        public int Capacity { get; set; }
        public int Load { get; set; }
        public int TransportTypeId { get; set; }

        public TransporterInfo(int id, int playerId, int routeId, DateTime lastMoved, decimal distanceTravelled, int commodityId, int capacity, int load, int transportTypeId)
        {
            Id = id;
            PlayerId = playerId;
            RouteId = routeId;
            LastMoved = lastMoved;
            DistanceTravelled = distanceTravelled;
            CommodityId = commodityId;
            Capacity = capacity;
            Load = load;
            TransportTypeId = transportTypeId;
        }
    }
}
