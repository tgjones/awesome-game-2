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

                switch (path)
                {
                    case "login":
                        Console.WriteLine("Login request.");
                        LoginInfo loginInfo = new LoginInfo();
                        loginInfo.LoginSuccessful = false;

                        PlayerInfo playerInfo = null;
                        {
                            //TODO: get player_id by looking up email address in players list
                            int player_id = 1;
                            List<PlayerInfo> playerList = new List<PlayerInfo>(_connection.GetPlayers(player_id));
                            playerInfo = playerList[0];
                        }

                        if (playerInfo == null)
                        {
                            loginInfo.LoginFailReason = "Could not identify player.";
                        }
                        {
                            if (_sessions.FindSession(playerInfo.Id) != null)
                            {
                                loginInfo.LoginFailReason = "Already logged in.";
                            }
                            else
                            {
                                // check authentication
                                if (false)
                                {
                                    loginInfo.LoginFailReason = "Authentication failed.";
                                }
                                else
                                {
                                    SessionInfo newSession = new SessionInfo(playerInfo.Id);
                                    _sessions.Add(newSession);
                                    loginInfo.LoginSuccessful = true;
                                    loginInfo.SessionKey = newSession.session_key;
                                }
                            }
                        }

                        XmlHelper.SerialiseLoginInfo(loginInfo, context.Response.OutputStream);
                        context.Response.OutputStream.Close();
                        break;


                    case "players":
                        Console.WriteLine("Oh the players!");

                        List<PlayerInfo> players;

                        if (query.Length > 0)
                        {
                            string[] queries = query.Split('=');
                            if (queries[0] == "id")
                                players = new List<PlayerInfo>(_connection.GetPlayers(int.Parse(queries[1])));
                            else if (queries[0] == "email")
                                players = new List<PlayerInfo>(_connection.GetPlayers(queries[1]));
                            else
                                players = new List<PlayerInfo>(_connection.GetPlayers());
                        }
                        else
                        {
                            players = new List<PlayerInfo>(_connection.GetPlayers());
                        }

                        XmlHelper.SerialisePlayerList(players, context.Response.OutputStream);

                        context.Response.OutputStream.Close();

                        break;
                    case "locations":
                        List<LocationInfo> locations;

                        if (query.Length > 0)
                        {
                            string[] queries = query.Split('=');
                            if (queries[0] == "id")
                                locations = new List<LocationInfo>(_connection.GetLocations(int.Parse(queries[1])));
                            else
                                locations = new List<LocationInfo>(_connection.GetLocations());
                        }
                        else
                        {
                            locations = new List<LocationInfo>(_connection.GetLocations());
                        }

                        foreach (LocationInfo location in locations)
                        {
                            _connection.UpdateStockInfo(location);
                        }

                        XmlHelper.SerialiseLocationList(locations, context.Response.OutputStream);

                        context.Response.OutputStream.Close();
                        break;
                    case "transporters":
                        List<TransporterInfo> transporters;
                        
                        transporters = new List<TransporterInfo>(_connection.GetTransporters());

                        foreach (TransporterInfo transporter in transporters)
                        {
                            _connection.UpdateStockInfo(transporter);
                        }

                        XmlHelper.SerialiseTransporterList(transporters, context.Response.OutputStream);

                        context.Response.OutputStream.Close();
                        break;
                    case "routes":
                        List<RouteInfo> routes;

                        if (query.Length > 0)
                        {
                            string[] queries = query.Split('=');
                            if (queries[0] == "id")
                                routes = new List<RouteInfo>(_connection.GetRoutes(int.Parse(queries[1])));
                            else
                                routes = new List<RouteInfo>(_connection.GetRoutes());
                        }
                        else
                        {
                            routes = new List<RouteInfo>(_connection.GetRoutes());
                        }

                        foreach (RouteInfo route in routes)
                        {
                            route.FromLocation = (LocationInfo)_connection.GetLocations(route.FromLocationId).First();
                            route.ToLocation = (LocationInfo)_connection.GetLocations(route.ToLocationId).First();
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
                            message.FromPlayer = (PlayerInfo)_connection.GetPlayers(message.FromPlayerId).First();
                            message.ToPlayer = (PlayerInfo)_connection.GetPlayers(message.ToPlayerId).First();
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
    }
}
