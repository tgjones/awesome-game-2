﻿using System;
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
        static object _connectionLock = new object();

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
            string MyConString = "SERVER=80.82.119.156;" +
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

            lock (_connectionLock)
            {
                MySqlCommand command = _connection.CreateCommand();
                command.CommandText = @"select name from players";
                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        players.Add(Reader.GetString(0));
                    }
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

            lock (_connectionLock)
            {
                MySqlCommand command = _connection.CreateCommand();
                command.CommandText = @"SELECT L.id, L.latitude, L.longitude, L.name, L.type FROM locations L;";

                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        int id = (int)Reader.GetUInt32(0);
                        decimal latitude = Reader.GetDecimal(1);
                        decimal longitude = Reader.GetDecimal(2);
                        string name = Reader.GetString(3);
                        LocationEnum locationtype = (LocationEnum)Reader.GetUInt32(4);
                        LocationInfo locationInfo = new LocationInfo(id, latitude, longitude, name, locationtype);
                        locations.Add(locationInfo);
                    }
                }
            }

            return (locations);
        }

        /// <summary>
        /// Retrieve a location from the database by id
        /// </summary>
        internal LocationInfo GetLocation(int query_id)
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            LocationInfo result = null;
            lock (_connectionLock)
            {
                MySqlCommand command = _connection.CreateCommand();
                command.CommandText = string.Format(@"SELECT L.id, L.latitude, L.longitude, L.name, L.type FROM locations L WHERE L.id={0};", query_id);

                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        int id = (int)Reader.GetUInt32(0);
                        decimal latitude = Reader.GetDecimal(1);
                        decimal longitude = Reader.GetDecimal(2);
                        string name = Reader.GetString(3);
                        LocationEnum locationtype = (LocationEnum)Reader.GetUInt32(4);
                        LocationInfo locationInfo = new LocationInfo(id, latitude, longitude, name, locationtype);
                        result = locationInfo;
                    }
                }
            }

            return result;
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

            lock (_connectionLock)
            {
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
            }

            return (players);
        }

        /// <summary>
        /// Retrieve a player from the database by id
        /// </summary>
        internal PlayerInfo GetPlayer(int query_id)
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            PlayerInfo result = null;

            lock (_connectionLock)
            {
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
                        result =playerInfo;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Retrieve a player from the database by email
        /// </summary>
        internal PlayerInfo GetPlayer(string query_email)
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            PlayerInfo result = null;

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = string.Format(@"SELECT P.id, P.email, P.password, P.name, P.joined, P.last_login, P.balance FROM players P WHERE P.email='{0}';", query_email);

            lock (_connectionLock)
            {
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
                        result = playerInfo;
                    }
                }
            }
            return result;
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

            lock (_connectionLock)
            {
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
            }
            return (transporters);
        }


        internal TransporterInfo CreateTransporter(int player_id, int route_id, int from_location_id)
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            TransporterInfo transporter = new TransporterInfo();

            int t_id=-1;
            lock (_connectionLock)
            {
                MySqlCommand command = _connection.CreateCommand();
                command.CommandText = @"SELECT id FROM transporters ORDER BY id DESC LIMIT 1";
                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        t_id = (int)Reader.GetUInt32("id") + 1;
                    }
                }

                command = _connection.CreateCommand();
                command.CommandText = @"INSERT INTO transporters SET " +
                                       " id=" + t_id +
                                       ", player_id=" + player_id +
                                       ", route_id=" + route_id + 
                                       ", capacity=" + 1000 +
                                       ", transport_type_id=0" +
                                       ", last_moved=NOW()";
                command.ExecuteNonQuery();
            }

            return (GetTransporters().FirstOrDefault(n => n.Id == t_id));

        }


        internal void MoveResourcesToTransporter(int location_id, int tranporter_id, ResourceEnum resource, int quantity)
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            MySqlCommand command = _connection.CreateCommand(); 
            lock (_connectionLock)
            {
                // remove from location
                command.CommandText = @"UPDATE location_stock SET quantity=quantity-" + quantity + " WHERE commodity_id=" + (int) resource + " AND location_id=" + location_id;
                command.ExecuteNonQuery();

                // remove existing items from transporter
                command.CommandText = @"DELETE FROM transporter_stock WHERE transporter_id=" + tranporter_id + " AND commodity_id=" + (int)resource;
                command.ExecuteNonQuery();

                // set on transporter 
                command.CommandText = @"INSERT INTO transporter_stock SET quantity=" + quantity + 
                                            ", transporter_id=" + tranporter_id + 
                                            ", commodity_id=" + (int)resource + 
                                            ", bought_price=10";
                command.ExecuteNonQuery();
            }
        }

        internal IEnumerable<TransporterInfo> GetTransporters(int player_id)
        {
            return GetTransporters().Where(t => t.PlayerId == player_id);
        }

        /// <summary>
        /// Retrieve a list of all routes from the database
        /// </summary>
        internal IEnumerable<RouteInfo> GetRoutes()
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            List<RouteInfo> routes = new List<RouteInfo>();

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT R.id, R.from_location_id, R.to_location_id, R.distance, R.speed, R.cost, R.player_id, R.state FROM transport_routes R;";

            lock (_connectionLock)
            {
                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        int id = (int)Reader.GetUInt32(0);
                        int from_location_id = (int)Reader.GetUInt32(1);
                        int to_location_id = (int)Reader.GetUInt32(2);
                        decimal distance = Reader.GetDecimal(3);
                        decimal speed = Reader.GetDecimal(4);
                        int cost = (int)Reader.GetUInt32(5);
                        int player_id = (int)Reader.GetUInt32(6);
                        decimal state = Reader.GetDecimal(7);
                        
                        RouteInfo routeInfo = new RouteInfo(id, from_location_id, to_location_id, distance, speed, cost, player_id, state);
                        routes.Add(routeInfo);
                    }
                }
            }
            return (routes);
        }

        /// <summary>
        /// Retrieve a list of all messages from the database
        /// </summary>
        internal IEnumerable<MessageInfo> GetMessages()
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            List<MessageInfo> messages = new List<MessageInfo>();

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT M.id, M.to_player_id, M.from_player_id, M.subject, M.body FROM messages M;";

            lock (_connectionLock)
            {
                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        int id = (int)Reader.GetUInt32(0);
                        int to_player_id = (int)Reader.GetUInt32(1);
                        int from_player_id = (int)Reader.GetUInt32(2);
                        string subject = Reader.GetString(3);
                        string body = Reader.GetString(4);

                        MessageInfo messageInfo = new MessageInfo(id, to_player_id, from_player_id, subject, body);
                        messages.Add(messageInfo);
                    }
                }
            }
            return (messages);
        }

        /// <summary>
        /// Retrieve a route from the database by id
        /// </summary>
        internal RouteInfo GetRoute(int query_id)
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            RouteInfo result = null;

            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = string.Format(@"SELECT R.id, R.from_location_id, R.to_location_id, R.distance, R.speed, R.cost, R.player_id, R.state FROM transport_routes R WHERE R.id={0};", query_id);

            lock (_connectionLock)
            {
                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        int id = (int)Reader.GetUInt32(0);
                        int from_location_id = (int)Reader.GetUInt32(1);
                        int to_location_id = (int)Reader.GetUInt32(2);
                        decimal distance = Reader.GetDecimal(3);
                        decimal speed = Reader.GetDecimal(4);
                        int cost = (int)Reader.GetUInt32(5);
                        int player_id = (int)Reader.GetUInt32(6);
                        decimal state = Reader.GetDecimal(7);

                        RouteInfo routeInfo = new RouteInfo(id, from_location_id, to_location_id, distance, speed, cost, player_id, state);
                        result = routeInfo;
                    }
                }
            }
            return result;
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

            lock (_connectionLock)
            {
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
        }

        /// <summary>
        /// Update the stock levels for a transporter from the database.
        /// </summary>
        /// <param name="transporter">The transporter to update the stock levels for.</param>
        internal void UpdateStockInfo(TransporterInfo transporter)
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            lock (_connectionLock)
            {
                MySqlCommand command = _connection.CreateCommand();
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

            MySqlCommand command;

            // get the quantity, maximum and prices for each commodity from location_stock
            lock (_connectionLock)
            {
                command = _connection.CreateCommand();
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
            }

            // get the process ids for this location from location_process
            List<int> processList = new List<int>();
            lock (_connectionLock)
            {
                command.CommandText = @"select id as process_id from location_process where location_id = " + location_id + ";";
                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        processList.Add((int)Reader.GetUInt32("process_id"));
                    }
                }
            }

            // get all the info about the stocks
            foreach (int process_id in processList)
            {
                // consumption
                lock (_connectionLock)
                {
                    command.CommandText = @"select commodity_id, quantity from process_consumption where process_id = " + process_id + ";";
                    using (MySqlDataReader Reader = command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ResourceEnum resource = (ResourceEnum)(int)Reader.GetUInt32("commodity_id");
                            stockList[resource].Consumes += (int)Reader.GetUInt32("quantity");
                        }
                    }
                }
                // production
                lock (_connectionLock)
                {
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
            lock (_connectionLock)
            {
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
            }

            // check each location to see if they have the required resources and space, and produce if possible
            foreach (KeyValuePair<int, int> overdueItem in overdueList)
            {
                ConsumeProduceProcess processInfo = new ConsumeProduceProcess();
                
                processInfo.Process_id = overdueItem.Key;
                processInfo.Location_id = overdueItem.Value;

                // get the resources consumed
                lock (_connectionLock)
                {
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
                }

                // get the resources produced
                lock (_connectionLock)
                {
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
                }

                // begin transaction
                lock (_connectionLock)
                {
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

                        foreach (KeyValuePair<ResourceEnum, StockProcessInfo> processItem in processInfo)
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
            lock (_connectionLock)
            {
                command.CommandText = @"select id as location_id from locations;";

                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        int location_id = (int)Reader.GetInt32("location_id");
                        locationList.Add(location_id);
                    }
                }
            }

            // process each location in turn
            foreach (int location_id in locationList)
            {
                lock (_connectionLock)
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
        }

        #endregion

        #region Move transporters
        /// <summary>
        /// Move the transporters
        /// </summary>
        internal void UpdateTransporters()
        {
            if (!IsConnected)
                throw new Exception("Not connected to database.");

            // get the list of transporters
            List<int> transporterIdList = new List<int>();
            lock (_connectionLock)
            {
                MySqlCommand command = _connection.CreateCommand();
                command.CommandText = @"SELECT T.id as tranporter_id FROM transporters T;";

                using (MySqlDataReader Reader = command.ExecuteReader())
                {
                    while (Reader.Read())
                    {
                        transporterIdList.Add((int)Reader.GetUInt32("tranporter_id"));
                    }
                }
            }

            foreach (int transporter_id in transporterIdList)
            {
                lock (_connectionLock)
                {
                    MySqlCommand command;
                    command = _connection.CreateCommand();

                    // begin transaction
                    command.CommandText = "start transaction;";
                    command.ExecuteNonQuery();

                    int player_id;
                    int route_id;
                    DateTime last_moved = DateTime.Now;
                    decimal distance_travelled=0;
                    int transport_type_id;
                    int capacity;
                    int elapsed = 0;

                    decimal route_distance=0;
                    decimal route_speed=0;
                    decimal route_state=1;

                    DateTime db_now = DateTime.Now;

                    // get transporter info
                    command.CommandText = @"SELECT T.player_id, T.route_id, T.distance_travelled, T.transport_type_id, T.capacity, R.distance, R.speed, R.state, UNIX_TIMESTAMP(NOW()) - UNIX_TIMESTAMP(last_moved) AS elapsed
                                            FROM transporters T, transport_routes R
                                            WHERE T.id = " + transporter_id + " AND T.route_id = R.id;";

                    using (MySqlDataReader Reader = command.ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            player_id = (int)Reader.GetUInt32("player_id");
                            route_id = (int)Reader.GetUInt32("route_id");
                            distance_travelled = Reader.GetDecimal("distance_travelled");
                            transport_type_id = (int)Reader.GetUInt32("transport_type_id");
                            capacity = (int)Reader.GetUInt32("capacity");
                            elapsed = (int)Reader.GetUInt32("elapsed");

                            route_distance = Reader.GetDecimal("distance");
                            route_speed = Reader.GetDecimal("speed");
                            route_state = Reader.GetDecimal("state");
                        }
                    }

                    // update transporter position
                    decimal newDistanceTravelled = distance_travelled + ((route_speed * route_state * (decimal)elapsed) / (decimal)3600);
                    
                    if (newDistanceTravelled >= route_distance)
                    {
                        // we've arrived so don't overshoot!
                        newDistanceTravelled = route_distance;
                        if (newDistanceTravelled != distance_travelled)
                        {
                            // we've JUST arrived
                            // (add stock to location, delete transporter, player.balance += location_stock.price*t.load)
                            // Actually, just notify the player - they can sell the goods if they want
                        }
                    }
                    // update the database (position of transporter)
                    if (newDistanceTravelled != distance_travelled)
                    {
                        command.CommandText = string.Format(@"UPDATE transporters SET distance_travelled = {0}, last_moved=NOW() WHERE id={1};", newDistanceTravelled, transporter_id);
                        command.ExecuteNonQuery();
                    }

                    command.CommandText = "commit;";
                    command.ExecuteNonQuery();

                }   //lock
            }
        }

        #endregion

        #endregion Methods
    }
}
