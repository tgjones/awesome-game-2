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

            decimal Rmax = 1000;    //max resource price
            decimal Rmin = 20;      //min resource price

            decimal stockLevel = ((decimal)Quantity / (decimal)Maximum);
            decimal newPrice = Rmax -  stockLevel * (Rmax - Rmin);

            if (currentPrice == (int)newPrice)
            {
                Console.WriteLine(ResourceType + " at " + LocationId + "\toldPrice={0:C} (unchanged)", currentPrice);
            }
            else
            {
                Console.WriteLine(ResourceType + " at " + LocationId + "\toldPrice={0:C}\tnewPrice={1:C}", currentPrice, newPrice);
            }

            UnitPrice = (int)newPrice;

            #region Supply and demand model (deprecated)
            //decimal stockLevel = Quantity / Maximum;
            //int targetLevel = Maximum / 2;
            //int distanceToTarget = targetLevel - Quantity;

            //decimal demand;
            //decimal supply;

            //// demand (+ve when we need more)
            //if (Consumes == 0)
            //    demand = -1;
            //else
            //    demand = (distanceToTarget - (3 * Consumes)) / Consumes;

            //// surpless (+ve when we don't have much)
            //if (Produces == 0)
            //    supply = -1;
            //else
            //    supply = (distanceToTarget - (3 * Produces)) / Produces;

            //// factor for supply and demand
            //decimal sd = (supply + demand) * 0.00001M;
            //currentPrice = currentPrice + (UnitPrice * sd);


            //// inflation
            ////currentPrice = currentPrice + ((decimal)UnitPrice * interestRate);

            //UnitPrice = (int)Math.Round(currentPrice, 0, MidpointRounding.ToEven);

            //if (UnitPrice < 1) 
            //    UnitPrice = 1;

            //Console.WriteLine(ResourceType + " at " + LocationId + "\tsd={0:F0}\toldPrice={1:C}\tnewPrice={2:C}", supply + demand, UnitPrice, currentPrice);
            #endregion //Supply and demand model (deprecated)

        }
    }
}
