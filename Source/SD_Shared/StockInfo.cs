using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SD.Shared
{
    public class StockInfo
    {
        public ResourceEnum ResourceType { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }

        public StockInfo()
        { }

        public StockInfo(ResourceEnum resourceType, int quantity, int unitPrice)
            : this()
        {
            this.ResourceType = resourceType;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
        }
    }
}
