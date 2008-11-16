using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using SD.Shared;

namespace SD.Core
{
    internal class Server
    {
        HttpListener _listener;
        DatabaseConnection _connection;
        SessionList _sessions;

        public Server(string uriPrefix, DatabaseConnection connection)
        {
            if (string.IsNullOrEmpty(uriPrefix)) throw new ArgumentNullException("uriPrefix");
            if (connection == null) throw new ArgumentNullException("connection");

            System.Threading.ThreadPool.SetMaxThreads(5, 1000);
            System.Threading.ThreadPool.SetMinThreads(5, 5);
            _listener = new HttpListener();
            _listener.Prefixes.Add(uriPrefix);

            _connection = connection;

            _sessions = new SessionList();
        }

        public void Start()
        {
            _listener.Start();
            while (true)
                try
                {
                    HttpListenerContext request = _listener.GetContext();
                    ThreadPool.QueueUserWorkItem(ProcessRequest, request);
                }
                catch (HttpListenerException) { break; }
                catch (InvalidOperationException) { break; }
        }

        public void Stop() { _listener.Stop(); }

        void ProcessRequest(object listenerContext)
        {
            try
            {
                var context = (HttpListenerContext)listenerContext;
                Uri uri = context.Request.Url;
                Console.WriteLine("Got " + uri.PathAndQuery + " so path is " + uri.AbsolutePath + " and query is " + uri.Query);

                string path = uri.AbsolutePath.Substring(1);
                string query="";
                if (uri.Query.Length > 0)
                    query = uri.Query.Substring(1);

                Console.WriteLine("So path is now " + path + " and query is " + query);

                // split the parameters into a handy dictionary
                Dictionary<string, string> paramDict = SeparateParameters(query);

                // see if a session key was provided, and get the player_id for authenticated player
                int authenticatedPlayerId = -1;
                if (paramDict.ContainsKey("sessionkey"))
                {
                    SessionInfo authenticatedSession = _sessions.FindSession(new Guid(paramDict["sessionkey"]));
                    if (authenticatedSession != null)
                        authenticatedPlayerId = authenticatedSession.player_id;
                }

                switch (path)
                {
                    case "login":
                        Console.WriteLine("Login request.");
                        LoginInfo loginInfo = new LoginInfo();
                        loginInfo.LoginSuccessful = false;
                        string password="";

                        PlayerInfo playerInfo = null;
                        string[] parameters = query.Split('&');

                        foreach (string parameter in parameters)
                        {
                            string[] param = parameter.Split('=');
                            if (param[0] == "email")
                            {
                                playerInfo = _connection.GetPlayer(param[1]);
                            }
                            else if (param[0] == "password")
                            {
                                password = param[1];
                            }
                        }


                        if (playerInfo == null)
                        {
                            loginInfo.LoginFailReason = "Could not identify player.";
                        }
                        else
                        {
                            if (_sessions.FindSession(playerInfo.Id) != null)
                            {
                                loginInfo.LoginFailReason = "Already logged in.";
                            }
                            else
                            {
                                // check authentication
                                if (password == playerInfo.Password)
                                {
                                    SessionInfo newSession = new SessionInfo(playerInfo.Id);
                                    _sessions.Add(newSession);
                                    loginInfo.LoginSuccessful = true;
                                    loginInfo.SessionKey = newSession.session_key;
                                }
                                else
                                {
                                    loginInfo.LoginFailReason = "Authentication failed.";
                                }
                            }
                        }

                        XmlHelper.SerialiseLoginInfo(loginInfo, context.Response.OutputStream);
                        context.Response.OutputStream.Close();
                        break;
                    case "logout":
                        if (paramDict.ContainsKey("sessionkey"))
                        {
                            _sessions.Remove(_sessions.FindSession(new Guid(paramDict["sessionkey"])));
                        }
                        break;
                    case "players":
                        Console.WriteLine("Oh the players!");

                        List<PlayerInfo> players = new List<PlayerInfo>();

                        if (paramDict.ContainsKey("id"))
                            players.Add(_connection.GetPlayer(int.Parse(paramDict["id"])));
                        else if (paramDict.ContainsKey("email"))
                            players.Add(_connection.GetPlayer(paramDict["email"]));
                        else
                            players.AddRange(_connection.GetPlayers());

                        // don't expose private data
                        foreach (PlayerInfo player in players)
                        {
                            if (player.Id != authenticatedPlayerId)
                            {
                                //player.Email = "[private]";
                                player.Balance = int.MinValue;
                                player.Joined = DateTime.MinValue;
                                player.LastLogin = DateTime.MinValue;
                                player.Password = string.Empty;
                            }
                        }

                        XmlHelper.SerialisePlayerList(players, context.Response.OutputStream);

                        context.Response.OutputStream.Close();

                        break;
                    case "locations":
                        List<LocationInfo> locations = new List<LocationInfo>();

                        if (query.Length > 0)
                        {
                            string[] queries = query.Split('=');
                            if (queries[0] == "id")
                                locations.Add(_connection.GetLocation(int.Parse(queries[1])));
                            else
                                locations.AddRange(_connection.GetLocations());
                        }
                        else
                        {
                            locations.AddRange(_connection.GetLocations());
                        }

                        foreach (LocationInfo location in locations)
                        {
                            _connection.UpdateStockInfo(location);
                        }

                        XmlHelper.SerialiseLocationList(locations, context.Response.OutputStream);

                        context.Response.OutputStream.Close();
                        break;

                    case "transporters":
                        List<TransporterInfo> transporters = new List<TransporterInfo>();
                        transporters.AddRange(_connection.GetTransporters(authenticatedPlayerId));
                        // update info on these stock
                        foreach (TransporterInfo transporter in transporters)
                            _connection.UpdateStockInfo(transporter);

                        XmlHelper.SerialiseTransporterList(transporters, context.Response.OutputStream);
                        context.Response.OutputStream.Close();
                        break;

                    case "transport":
                        int from_id = -1;
                        int to_id = -1;
                        Dictionary<ResourceEnum, int> cargoList = new Dictionary<ResourceEnum, int>();

                        // get data from parameters
                        if (paramDict.ContainsKey("from")) from_id = int.Parse(paramDict["from"]);
                        if (paramDict.ContainsKey("to")) to_id = int.Parse(paramDict["to"]);
                        foreach (ResourceEnum resource in Enum.GetValues(typeof(ResourceEnum)))
                        {
                            string key = "stock" + resource.ToString().ToLower() ;
                            if (paramDict.ContainsKey(key))
                                cargoList.Add(resource, int.Parse(paramDict[key]));
                        }

                        //validate data
                        RequestReply reply = new RequestReply();
                        reply.Success = true;
                        if (authenticatedPlayerId == -1)
                        {
                            reply.ErrorMessage = "Authentication failed.";
                            reply.Success=false;
                        }

                        LocationInfo fromLocation = null;
                        if (reply.Success)
                        {
                            fromLocation = _connection.GetLocation(from_id);
                            if (fromLocation == null)
                            {
                                reply.ErrorMessage = "From location not found.";
                                reply.Success = false;
                            }
                        }

                        LocationInfo toLocation = null;
                        if (reply.Success)
                        {
                            toLocation = _connection.GetLocation(from_id);
                            if (toLocation == null)
                            {
                                reply.ErrorMessage = "To location not found.";
                                reply.Success = false;
                            }
                        }

                        if (reply.Success)
                        {
                            _connection.UpdateStockInfo(fromLocation);
                            if (cargoList.Count == 0)
                            {
                                reply.ErrorMessage = "No cargo requested.";
                                reply.Success = false;
                            }
                        }

                        if (reply.Success)
                        {

                            //TODO: transaction to make sure resources are not sold whilst we are checking stocks

                            foreach (KeyValuePair<ResourceEnum, int> cargo in cargoList)
                            {
                                StockInfo stockInfo = fromLocation.Stocks.First(s => s.ResourceType == cargo.Key);
                                if (stockInfo == null)
                                {
                                    reply.ErrorMessage = "Resource " + cargo.Key + " is not available.";
                                    reply.Success = false;
                                }
                                if (stockInfo.Quantity < cargo.Value)
                                {
                                    reply.ErrorMessage = "Resource " + cargo.Key + " is not available in sufficient quantities.";
                                    reply.Success = false;
                                }
                            }
                        }

                        RouteInfo route = null;
                        if (reply.Success)
                        {
                            // Determine which route to use
                            route = _connection.GetRoutes().Where(r => r.FromLocationId == from_id).Where(r => r.ToLocationId == to_id).FirstOrDefault();
                            if (route == null)
                            {
                                reply.ErrorMessage = "There is no route from " + fromLocation.Name + " to " + toLocation.Name + ".";
                                reply.Success = false;
                            }
                        }

                        if (reply.Success)
                        {
                            TransporterInfo newTransporter = _connection.CreateTransporter(authenticatedPlayerId, route.Id, from_id);

                            foreach (KeyValuePair<ResourceEnum, int>cargo in cargoList)
                            {
                                _connection.MoveResourcesToTransporter(from_id, newTransporter.Id, cargo.Key, cargo.Value);
                            }
                        }

                        XmlHelper.SerialiseRequestReply(reply, context.Response.OutputStream);
                        context.Response.OutputStream.Close();

                        break;

                    case "routes":
                        List<RouteInfo> routes  = new List<RouteInfo>();

                        if (query.Length > 0)
                        {
                            string[] queries = query.Split('=');
                            if (queries[0] == "id")
                                routes.Add(_connection.GetRoute(int.Parse(queries[1])));
                            else
                                routes.AddRange(_connection.GetRoutes());
                        }
                        else
                        {
                            routes.AddRange(_connection.GetRoutes());
                        }

                        foreach (RouteInfo r in routes)
                        {
                            r.FromLocation = _connection.GetLocation(r.FromLocationId);
                            r.ToLocation = _connection.GetLocation(r.ToLocationId);
                        }

                        XmlHelper.SerialiseRouteList(routes, context.Response.OutputStream);

                        context.Response.OutputStream.Close();
                        break;
                    case "messages":
                        List<MessageInfo> messages;

                        //if (query.Length > 0)
                        //{
                        //    string[] queries = query.Split('=');
                        //    if (queries[0] == "id")
                        //        messages = new List<MessageInfo>(_connection.GetMessages(int.Parse(queries[1])));
                        //    else
                        //        messages = new List<MessageInfo>(_connection.GetMessages());
                        //}
                        //else
                        //{
                            messages = new List<MessageInfo>(_connection.GetMessages());
                        //}

                        foreach (MessageInfo message in messages)
                        {
                            message.FromPlayer = _connection.GetPlayer(message.FromPlayerId);
                            message.ToPlayer = _connection.GetPlayer(message.ToPlayerId);
                        }

                        XmlHelper.SerialiseMessageList(messages, context.Response.OutputStream);

                        context.Response.OutputStream.Close();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Request error: " + ex);
            }

            Console.WriteLine("Thread exiting.");
        }

        Dictionary<string, string> SeparateParameters(string parameters)
        {
            string[] parameterAry = parameters.Split('&');

            Dictionary<string, string> paramDict = new Dictionary<string, string>();

            foreach (string parameter in parameterAry)
            {
                string[] bits = parameter.Split('=');
                if (bits.Length == 2)
                {
                    paramDict.Add(bits[0].ToLower(), bits[1]);
                }
            }

            return paramDict;
        }
    }
}
