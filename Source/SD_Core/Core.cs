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

            Server server = new Server(@"http://192.168.0.103:54321/", _connection);
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
            long _startTime = 0;

            while (!_exitThread)
            {
                _startTime = (DateTime.Now.Ticks / 1000L);
                // The main thread for looping around processing game logic

                foreach (LocationInfo location in _locations)
                {
                    database.UpdateStockInfo(location);

                }
                Console.Write('.');

                // sleep for the rest of this second (assuming there is some remaining
                long timeRemaining = 1000 - ((DateTime.Now.Ticks / 1000L) - _startTime); // e-7 s
                
                if (timeRemaining > 0)
                {
                    Thread.Sleep((int)timeRemaining);
                }
            }

        }

    }
}
