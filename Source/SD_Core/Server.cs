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

        public Server(string uriPrefix, DatabaseConnection connection)
        {
            if (string.IsNullOrEmpty(uriPrefix)) throw new ArgumentNullException("uriPrefix");
            if (connection == null) throw new ArgumentNullException("connection");

            System.Threading.ThreadPool.SetMaxThreads(5, 1000);
            System.Threading.ThreadPool.SetMinThreads(5, 5);
            _listener = new HttpListener();
            _listener.Prefixes.Add(uriPrefix);

            _connection = connection;
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
                    default:
                        break;
                }

                //List<LocationInfo> locations;
                //locations = new List<LocationInfo>(_connection.GetLocations());

                //foreach (LocationInfo location in locations)
                //{
                //    _connection.UpdateStockInfo(location);
                //}

                //XmlHelper.SerialiseLocationList(locations, context.Response.OutputStream);

                //context.Response.OutputStream.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Request error: " + ex);
            }

            Console.WriteLine("Thread exiting.");
        }
    }
}
