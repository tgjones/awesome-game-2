using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MySql.Data.MySqlClient;

using SD.Shared;

namespace SD.Core
{
    internal class DatabaseConnection
    {
        MySqlConnection _connection;

        #region Constructor and destructor

        ~DatabaseConnection()
        {
            Disconnect();
        }

        #endregion //Constructor and destructor


        #region Properties
        /// <summary>
        /// Is the database connected?
        /// </summary>
        internal bool IsConnected
        {
            get
            {
                if (_connection != null)
                {
                    if (_connection.State != System.Data.ConnectionState.Closed)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        #endregion // Properties

        #region Methods
        /// <summary>
        /// Establish a connection with the database
        /// </summary>
        internal bool Connect()
        {

            #region Initialise Database
            Console.Write("Connecting to database...");
            string MyConString = "SERVER=192.168.0.103;" +
                "DATABASE=sd;" +
                "UID=sduser;" +
                "PASSWORD=sdpass;";
            _connection = new MySqlConnection(MyConString);
            _connection.Open();
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                Console.Write(" Connected!\n");
            }
            else
            {
                Console.WriteLine("Failed to connect to database.");
                return false;
            }
            #endregion //Initialise Database

            return true;
        }
        void Disconnect()
        {
            if (IsConnected)
            {
                _connection.Close();
            }
        }

        /// <summary>
        /// Retrieve a list of player names from the database.
        /// </summary>
        internal IEnumerable<String> GetPlayerList()
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            List<string> players = new List<string>();

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = @"select name from players";

            using (MySqlDataReader Reader = command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    players.Add(Reader.GetString(0));
                }
            }

            return (players);
        }

        /// <summary>
        /// Retrive a list of all locations from the database.
        /// </summary>
        internal IEnumerable<LocationInfo> GetLocations()
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            List<LocationInfo> locations = new List<LocationInfo>();

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT L.id, L.latitude, L.longitude, L.name FROM locations L;";

            using (MySqlDataReader Reader = command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    int id = (int)Reader.GetUInt32(0);
                    decimal latitude = Reader.GetDecimal(1);
                    decimal longitude = Reader.GetDecimal(2);
                    string name = Reader.GetString(3);
                    LocationInfo locationInfo = new LocationInfo(id, latitude, longitude, name);
                    locations.Add(locationInfo);
                }
            }

            return (locations);
        }

        /// <summary>
        /// Update the stock levels for a location from the database.
        /// </summary>
        /// <param name="location">The location to update the stock levels for.</param>
        internal void UpdateStockInfo(LocationInfo location)
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = string.Format(@"SELECT S.commodity_id, S.quantity, S.price FROM location_stock S WHERE S.location_id={0};", location.Id);

            using (MySqlDataReader Reader = command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    int id = (int)Reader.GetUInt32(0);
                    int quantity = (int)Reader.GetUInt32(1);
                    int price = (int)Reader.GetUInt32(2);
                    StockInfo stock = location.Stocks.Find(n => (n.ResourceType == (ResourceEnum)id));
                    if (stock == null)
                    {
                        if (quantity > 0)
                        {
                            stock = new StockInfo((ResourceEnum)id, quantity, price);
                            location.Stocks.Add(stock);
                            Console.WriteLine("\nNew stock of {0} at {1}.", Enum.GetName(typeof(ResourceEnum), stock.ResourceType), location.Name);
                        }
                    }
                    else
                    {
                        if (quantity == 0)
                        {
                            location.Stocks.Remove(stock);
                            Console.WriteLine("\nNo more {0} at {1}.", Enum.GetName(typeof(ResourceEnum), stock.ResourceType), location.Name);
                        }

                        if (stock.Quantity != quantity)
                        {
                            stock.Quantity = quantity;
                            Console.WriteLine("\nQuantity of {0} changed at {1}.", Enum.GetName(typeof(ResourceEnum), stock.ResourceType), location.Name);
                        }
                        if (stock.UnitPrice != price)
                        {
                            stock.UnitPrice = price;
                            Console.WriteLine("\nPrice of {0} changed at {1}.", Enum.GetName(typeof(ResourceEnum), stock.ResourceType), location.Name);
                        }
                    }
                }
            }
        }

        #endregion Methods
    }
}
