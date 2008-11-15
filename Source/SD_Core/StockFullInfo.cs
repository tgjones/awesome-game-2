using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SD.Shared;

namespace SD.Core
{
    /// <summary>
    /// Internal class for getting all the info about a stock at a particular location
    /// </summary>
    internal class StockFullInfo
    {
        internal int LocationId { get; set; }
        internal ResourceEnum ResourceType { get; set; }
        internal int Quantity { get; set; }
        internal int UnitPrice { get; set; }
        internal int Maximum { get; set; }
        internal int Consumes { get; set; }
        internal int Produces { get; set; }
        internal int ProcessId { get; set; }

        internal void UpdatePrice(decimal interestRate)
        {
            // if we cannot store this stock then we don't care how much it costs
            if (Maximum == 0) return;

            decimal currentPrice = UnitPrice;

            // from 0 to 1
            decimal stockLevel = Quantity / Maximum;
            // demand (+ve when we need more)
            int targetLevel = Maximum / 2;
            int distanceToTarget = targetLevel - Quantity;

            decimal demand;
            decimal supply;
            if (Consumes == 0)
                demand = -1;
            else
                demand = distanceToTarget / Consumes;
            // surpless (+ve when we don't have much)
            if (Produces == 0)
                supply = -1;
            else
                supply = distanceToTarget / Produces;

            // factor for supply and demand
            decimal sd = (supply + demand) * 0.0001M;
            currentPrice = currentPrice + (UnitPrice * sd);

            Console.WriteLine(ResourceType + " at " + LocationId + "\tsd={0:F0}\toldPrice={1:C}\tnewPrice={2:C}", supply + demand, UnitPrice, currentPrice);

            // inflation
            //currentPrice = currentPrice + ((decimal)UnitPrice * interestRate);

            UnitPrice = (int)Math.Round(currentPrice, 0, MidpointRounding.ToEven);

            if (UnitPrice < 1) 
                UnitPrice = 1;

        }
    }
}
