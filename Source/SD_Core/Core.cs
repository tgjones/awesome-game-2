using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;

using SD.Shared;

namespace SD.Core
{
    public class Core
    {
        public static void Main()
        {
            DatabaseConnection _connection = new DatabaseConnection();
            _connection.Connect();

            Thread gameThread = new Thread(
                delegate()
                {
                    GameProcessor(_connection);
                }
                );
            gameThread.IsBackground = false;
            gameThread.Start();

#if DEBUG
            Server server = new Server(@"http://localhost:54321/", _connection);
#else
            Server server = new Server(@"http://192.168.0.105:54321/", _connection);
#endif

            new System.Threading.Thread(server.Start).Start();

        }

        static void GameProcessor(DatabaseConnection database)
        {
            List<LocationInfo> _locations;

            #region Get initial data from database
            _locations = database.GetLocations().ToList();

            foreach (LocationInfo location in _locations)
            {
                database.UpdateStockInfo(location);

                Console.WriteLine(location.Name);
                foreach (StockInfo stock in location.Stocks)
                {
                    Console.WriteLine(" - {0}:\t{1}\t{2}", Enum.GetName(typeof(ResourceEnum), stock.ResourceType), stock.Quantity, stock.UnitPrice);
                }
            }

            #endregion //Get initial data from database

            bool _exitThread = false;
            DateTime _startTime;
            DateTime _financialYear = DateTime.Now;

            while (!_exitThread)
            {
                _startTime = DateTime.Now;
                // The main thread for looping around processing game logic

                // production happens on every cycle
                database.ProductionCycle();

                // financial "year" occurs every minute
                if (_financialYear < _startTime)
                {
                    // process the next financial cycle
                    database.UpdatePrices();
                    // and set the earliest the next financial year can occur
                    _financialYear = _startTime.AddMinutes(1);
                }

                Console.Write('.');

                database.UpdateTransporters();

                // sleep for the rest of this second (assuming there is some remaining)
                TimeSpan timeRemaining = _startTime.AddSeconds(1).Subtract(DateTime.Now);// new TimeSpan(0, 0, 1);//1000 - ((DateTime.Now.Ticks / 1000L) - _startTime); // e-7 s
                
                if (timeRemaining.Milliseconds > 0)
                {
                    Thread.Sleep(timeRemaining.Milliseconds);
                }
            }

        }

    }
}
