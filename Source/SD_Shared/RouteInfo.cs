using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SD.Shared
{
    public class RouteInfo
    {
        public int Id { get; set; }
        public int FromLocationId { get; set; }
        public int ToLocationId { get; set; }
        public decimal Distance { get; set; }
        public decimal Speed { get; set; }
        public int Cost { get; set; }
        public int PlayerId { get; set; }
        public decimal State { get; set; }
        public LocationInfo FromLocation { get; set; }
        public LocationInfo ToLocation { get; set; }

        public RouteInfo(int id, int fromLocationId, int toLocationId, decimal distance, decimal speed, int cost, int playerId, decimal state)
        {
            Id = id;
            FromLocationId = fromLocationId;
            ToLocationId = toLocationId;
            Distance = distance;
            Speed = speed;
            Cost = cost;
            PlayerId = playerId;
            State = state;
        }
    }
}
