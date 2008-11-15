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
        #region Connect and disconnect
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
        #endregion Connect and disconnect

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
        /// Retrieve a list of all locations from the database.
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
        /// Retrieve a location from the database by id
        /// </summary>
        internal IEnumerable<LocationInfo> GetLocations(int query_id)
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            List<LocationInfo> locations = new List<LocationInfo>();

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = string.Format(@"SELECT L.id, L.latitude, L.longitude, L.name FROM locations L WHERE L.id={0};", query_id);

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
        /// Retrieve a list of all players from the database
        /// </summary>
        internal IEnumerable<PlayerInfo> GetPlayers()
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            List<PlayerInfo> players = new List<PlayerInfo>();

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT P.id, P.email, P.password, P.name, P.joined, P.last_login, P.balance FROM players P;";

            using (MySqlDataReader Reader = command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    int id = (int)Reader.GetUInt32(0);
                    string email = Reader.GetString(1);
                    string password = Reader.GetString(2);
                    string name = Reader.GetString(3);
                    DateTime joined = (DateTime)Reader.GetMySqlDateTime(4);
                    DateTime last_login = (DateTime)Reader.GetMySqlDateTime(5);
                    int balance = (int)Reader.GetInt32(6);

                    PlayerInfo playerInfo = new PlayerInfo(id, email, password, name, joined, last_login, balance);
                    players.Add(playerInfo);
                }
            }

            return (players);
        }

        /// <summary>
        /// Retrieve a player from the database by id
        /// </summary>
        internal IEnumerable<PlayerInfo> GetPlayers(int query_id)
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            List<PlayerInfo> players = new List<PlayerInfo>();

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = string.Format(@"SELECT P.id, P.email, P.password, P.name, P.joined, P.last_login, P.balance FROM players P WHERE P.id={0};", query_id);

            using (MySqlDataReader Reader = command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    int id = (int)Reader.GetUInt32(0);
                    string email = Reader.GetString(1);
                    string password = Reader.GetString(2);
                    string name = Reader.GetString(3);
                    DateTime joined = (DateTime)Reader.GetMySqlDateTime(4);
                    DateTime last_login = (DateTime)Reader.GetMySqlDateTime(5);
                    int balance = (int)Reader.GetInt32(6);

                    PlayerInfo playerInfo = new PlayerInfo(id, email, password, name, joined, last_login, balance);
                    players.Add(playerInfo);
                }
            }

            return (players);
        }

        /// <summary>
        /// Retrieve a player from the database by email
        /// </summary>
        internal IEnumerable<PlayerInfo> GetPlayers(string query_email)
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            List<PlayerInfo> players = new List<PlayerInfo>();

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = string.Format(@"SELECT P.id, P.email, P.password, P.name, P.joined, P.last_login, P.balance FROM players P WHERE P.email='{0}';", query_email);

            using (MySqlDataReader Reader = command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    int id = (int)Reader.GetUInt32(0);
                    string email = Reader.GetString(1);
                    string password = Reader.GetString(2);
                    string name = Reader.GetString(3);
                    DateTime joined = (DateTime)Reader.GetMySqlDateTime(4);
                    DateTime last_login = (DateTime)Reader.GetMySqlDateTime(5);
                    int balance = (int)Reader.GetInt32(6);

                    PlayerInfo playerInfo = new PlayerInfo(id, email, password, name, joined, last_login, balance);
                    players.Add(playerInfo);
                }
            }

            return (players);
        }

        /// <summary>
        /// Retrieve a list of all transporters from the database
        /// </summary>
        internal IEnumerable<TransporterInfo> GetTransporters()
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            List<TransporterInfo> transporters = new List<TransporterInfo>();

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT T.id, T.player_id, T.route_id, T.last_moved, T.distance_travelled, T.capacity, T.transport_type_id FROM transporters T;";

            using (MySqlDataReader Reader = command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    int id = (int)Reader.GetUInt32(0);
                    int player_id = (int)Reader.GetUInt32(1);
                    int route_id = (int)Reader.GetUInt32(2);
                    DateTime last_moved = (DateTime)Reader.GetMySqlDateTime(3);
                    decimal distance_travelled = Reader.GetDecimal(4);
                    int capacity = (int)Reader.GetUInt32(5);
                    int transport_type_id = (int)Reader.GetUInt32(6);

                    TransporterInfo transporterInfo = new TransporterInfo(id, player_id, route_id, last_moved, distance_travelled, capacity, transport_type_id);
                    transporters.Add(transporterInfo);
                }
            }

            return (transporters);
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

        /// <summary>
        /// Update the stock levels for a transporter from the database.
        /// </summary>
        /// <param name="transporter">The transporter to update the stock levels for.</param>
        internal void UpdateStockInfo(TransporterInfo transporter)
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            MySqlCommand command = _connection.CreateCommand();
            //command.CommandText = string.Format(@"SELECT S.commodity_id, S.quantity, S.price FROM location_stock S WHERE S.location_id={0};", location.Id);
            command.CommandText = string.Format(@"SELECT S.commodity_id, S.quantity, S.bought_price FROM transporter_stock S WHERE S.transporter_id={0};", transporter.Id);

            using (MySqlDataReader Reader = command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    int id = (int)Reader.GetUInt32(0);
                    int quantity = (int)Reader.GetUInt32(1);
                    int price = (int)Reader.GetUInt32(2);
                    StockInfo stock = transporter.Stocks.Find(n => (n.ResourceType == (ResourceEnum)id));
                    if (stock == null)
                    {
                        if (quantity > 0)
                        {
                            stock = new StockInfo((ResourceEnum)id, quantity, price);
                            transporter.Stocks.Add(stock);
                            //Console.WriteLine("\nNew stock of {0} at {1}.", Enum.GetName(typeof(ResourceEnum), stock.ResourceType), location.Name);
                        }
                    }
                    else
                    {
                        if (quantity == 0)
                        {
                            transporter.Stocks.Remove(stock);
                            //Console.WriteLine("\nNo more {0} at {1}.", Enum.GetName(typeof(ResourceEnum), stock.ResourceType), location.Name);
                        }

                        if (stock.Quantity != quantity)
                        {
                            stock.Quantity = quantity;
                            //Console.WriteLine("\nQuantity of {0} changed at {1}.", Enum.GetName(typeof(ResourceEnum), stock.ResourceType), location.Name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Return a list of stock information for this location.
        /// </summary>
        List<StockFullInfo> GetStockInfo(int location_id)
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            Dictionary<ResourceEnum, StockFullInfo> stockList = new Dictionary<ResourceEnum,StockFullInfo>();
            foreach (ResourceEnum resource in Enum.GetValues(typeof(ResourceEnum)))
            {
                stockList.Add(resource, new StockFullInfo());
                stockList[resource].ResourceType = resource;
                stockList[resource].LocationId = location_id;
            }

            // get the quantity, maximum and prices for each commodity from location_stock
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = @"select commodity_id, quantity, maximum, price from location_stock where location_id = " + location_id + ";";
            using (MySqlDataReader Reader = command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    ResourceEnum resource = (ResourceEnum)(int)Reader.GetUInt32("commodity_id");
                    stockList[resource].Quantity = (int)Reader.GetUInt32("quantity");
                    stockList[resource].Maximum = (int)Reader.GetUInt32("maximum");
                    stockList[resource].UnitPrice = (int)Reader.GetUInt32("price");
                }
            }

            // get the process ids for this location from location_process
            List<int> processList = new List<int>();
            command.CommandText = @"select id as process_id from location_process where location_id = " + location_id + ";";
            using (MySqlDataReader Reader = command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    processList.Add((int)Reader.GetUInt32("process_id"));
                }
            }

            // get all the info about the stocks
            foreach (int process_id in processList)
            {
                // consumption
                command.CommandText = @"select commodity_id, quantity from process_consumption where process_id = " + process_id + ";";
                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        ResourceEnum resource = (ResourceEnum)(int)Reader.GetUInt32("commodity_id");
                        stockList[resource].Consumes += (int)Reader.GetUInt32("quantity");
                    }
                }
                // production
                command.CommandText = @"select commodity_id, quantity from process_production where process_id = " + process_id + ";";
                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        ResourceEnum resource = (ResourceEnum)(int)Reader.GetUInt32("commodity_id");
                        stockList[resource].Produces += (int)Reader.GetUInt32("quantity");
                    }
                }
            }

            return stockList.Values.ToList();
        }

        #region Production cycle
        /// <summary>
        /// Check through the locations, consuming and producing where possible.
        /// </summary>
        internal void ProductionCycle()
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            MySqlCommand command = _connection.CreateCommand();

            // get a list of locations that are overdue to produce
            Dictionary<int, int> overdueList = new Dictionary<int, int>();  // process_id, location_id
            command.CommandText = @"select id as process_id, location_id from location_process where (DATE_ADD(last_produced, INTERVAL `interval` SECOND) < NOW())";
            using (MySqlDataReader Reader = command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    int process_id = (int)Reader.GetUInt32("process_id");
                    int location_id = (int)Reader.GetUInt32("location_id");
                    overdueList.Add(process_id, location_id);
                }
            }

            // check each location to see if they have the required resources and space, and produce if possible
            foreach (KeyValuePair<int, int> overdueItem in overdueList)
            {
                ConsumeProduceProcess processInfo = new ConsumeProduceProcess();
                
                processInfo.Process_id = overdueItem.Key;
                processInfo.Location_id = overdueItem.Value;

                // get the resources consumed
                command.CommandText = @"select commodity_id, quantity from process_consumption where process_id = " + processInfo.Process_id + ";";
                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        int commodity_id = (int)Reader.GetUInt32("commodity_id");
                        StockProcessInfo stockInfo = processInfo.GetStockProcessInfo((ResourceEnum)commodity_id);
                        stockInfo.Consumed = (int)Reader.GetUInt32("quantity");
                    }
                }
                // get the resources produced
                command.CommandText = @"select commodity_id, quantity from process_production where process_id = " + processInfo.Process_id + ";";
                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        int commodity_id = (int)Reader.GetUInt32("commodity_id");
                        StockProcessInfo stockInfo = processInfo.GetStockProcessInfo((ResourceEnum)commodity_id);
                        stockInfo.Produced = (int)Reader.GetUInt32("quantity");
                    }
                }

                // begin transaction
                command.CommandText = "start transaction;";
                command.ExecuteNonQuery();

                // get the current level of stock
                command.CommandText = @"select commodity_id, quantity, maximum from location_stock where location_id = " + processInfo.Location_id + ";";
                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        int commodity_id = (int)Reader.GetUInt32("commodity_id");
                        StockProcessInfo stockInfo = processInfo.GetStockProcessInfo((ResourceEnum)commodity_id);
                        stockInfo.CurrentLevel = (int)Reader.GetUInt32("quantity");
                        stockInfo.Maximum = (int)Reader.GetUInt32("maximum");
                    }
                }

                // Do we have the required resources?
                bool canProcess = true;
                foreach (KeyValuePair<ResourceEnum, StockProcessInfo> processItem in processInfo)
                {
                    if (!processItem.Value.CanProcess)
                    {
                        canProcess = false;
                        break;
                    }
                }

                if (canProcess)
                {
                    Console.WriteLine("\n\nConsumption/Production process activated at location: " + processInfo.Location_id);

                    foreach (KeyValuePair<ResourceEnum,StockProcessInfo> processItem in processInfo)
                    {
                        // process to get the new stock levels
                        Console.Write("\nResource processed: " + processItem.Key);
                        Console.Write(" level before: " + processItem.Value.CurrentLevel);
                        processItem.Value.Process();
                        Console.Write(" level after: " + processItem.Value.CurrentLevel);
                        
                        command.CommandText = @"update location_stock " +
                            " set quantity = " + processItem.Value.CurrentLevel +
                            " where commodity_id = " + (int)processItem.Key +
                            " and location_id = " + processInfo.Location_id + ";";
                        command.ExecuteNonQuery();
                    }
                    
                    // set the timestamp for next production
                    command.CommandText = @"update location_process set last_produced = NOW() where id = " + processInfo.Process_id + ";";
                    command.ExecuteNonQuery();
                }

                //end translation
                command.CommandText = "commit";
                command.ExecuteNonQuery();

            }
        }

        /// <summary>
        ///  Struct for holding data about a production process
        /// </summary>
        class ConsumeProduceProcess : IEnumerable<KeyValuePair<ResourceEnum, StockProcessInfo>>
        {
            internal int Process_id =-1;
            internal int Location_id =-1;
            Dictionary<ResourceEnum, StockProcessInfo> stock = new Dictionary<ResourceEnum, StockProcessInfo>();

            internal StockProcessInfo GetStockProcessInfo(ResourceEnum resource)
            {
                if (!stock.ContainsKey(resource))
                {
                    stock.Add(resource, new StockProcessInfo());
                }
                return stock[resource];
            }

            #region IEnumerable<KeyValuePair<ResourceEnum, StockProcessInfo>> Members

            public IEnumerator<KeyValuePair<ResourceEnum, StockProcessInfo>> GetEnumerator()
            {
                return stock.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return stock.GetEnumerator();
            }

            #endregion
        }
        class StockProcessInfo
        {
            internal int CurrentLevel;// = 0;
            internal int Maximum;// = 0;
            internal int Consumed;// = 0;
            internal int Produced;// = 0;

            /// <summary>
            /// Check the stock info to see if this resource is ok to produce/consume.
            /// Returns true if there are sufficient resources to consume and sufficient space for production.
            /// </summary>
            internal bool CanProcess
            {
                get
                {
                    if (CurrentLevel > Maximum)
                        throw new Exception("The current level of stock exceeds the maximum.  Something has gone wrong.");
                    if (Produced > Maximum)
                        throw new Exception("Cannot produce more than the maximum number of goods.  Something has gone wrong.");
                    if (Consumed > Maximum)
                        throw new Exception("Cannot consume more than the maximum number of goods.  Something has gone wrong.");
                    if (Consumed > 0 && Produced > 0)
                        throw new Exception("Cannot consume and produce the same resource in one process.");

                    if (CurrentLevel < 0) throw new ArgumentOutOfRangeException("CurrentLevel", "Cannot be less than 0");
                    if (Consumed < 0) throw new ArgumentOutOfRangeException("Consumed", "Cannot be less than 0");
                    if (Produced < 0) throw new ArgumentOutOfRangeException("Produced", "Cannot be less than 0");

                    if (CurrentLevel < Consumed) return false;
                    if (Produced + CurrentLevel > Maximum) return false;
                    return true;
                }
            }

            /// <summary>
            /// Do the consumption and production cycle to adjust the current level
            /// </summary>
            internal void Process()
            {
                if (!CanProcess) 
                    throw new Exception("Cannot process these resources.  You should have checked with CanProcess.");

                CurrentLevel = CurrentLevel + Produced - Consumed;
            }
        }
        #endregion //Production cycle


        #region Price adjustments
        /// <summary>
        /// Update the prices for commodities at all locations
        /// </summary>
        internal void UpdatePrices()
        {
            Console.WriteLine("\n------ Financial Year ------");

            decimal interestRate = 0.0102M;
            Console.WriteLine("Interest rate: {0:P}", interestRate);

            if (!IsConnected)
                throw new Exception("Not connected to database.");

            MySqlCommand command = _connection.CreateCommand();

            // get a list of all locations
            List<int> locationList = new List<int>();
            command.CommandText = @"select id as location_id from locations;";

            using (MySqlDataReader Reader = command.ExecuteReader())
            {
                while (Reader.Read())
                {
                    int location_id = (int)Reader.GetInt32("location_id");
                    locationList.Add(location_id);
                }
            }

            // process each location in turn
            foreach (int location_id in locationList)
            {
                command.CommandText = "start transaction";
                command.ExecuteNonQuery();

                foreach (StockFullInfo stockInfo in GetStockInfo(location_id))
                {
                    int oldPrice = stockInfo.UnitPrice;
                    stockInfo.UpdatePrice(interestRate);
                    if (oldPrice != stockInfo.UnitPrice)
                    {
                        command.CommandText = "update location_stock set price = " + stockInfo.UnitPrice +
                                              " where location_id = " + stockInfo.LocationId +
                                              " and commodity_id = " + (int)stockInfo.ResourceType + ";";
                        command.ExecuteNonQuery();
                    }
                }

                command.CommandText = "commit";
                command.ExecuteNonQuery();
            }
        }

        #endregion


        #endregion Methods
    }
}
