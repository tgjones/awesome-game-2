using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using MySql.Data.MySqlClient;

namespace SD_Core
{
    public class Core
    {
        public static void Main()
        {
            Thread gameThread = new Thread(new ThreadStart(GameProcessor));
            gameThread.IsBackground = false;
            gameThread.Start();

        }

        private static void GameProcessor()
        {
            MySqlConnection _connection;

            #region InitialiseDatabase
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
                return;
            }
            #endregion

            bool _exitThread = false;
            long _startTime = 0;

            while (!_exitThread)
            {
                _startTime = (DateTime.Now.Ticks / 1000L);
                // The main thread for looping around processing game logic

                foreach (string player in getPlayerList(_connection))
                    Console.WriteLine(player);

                // sleep for the rest of this second (assuming there is some remaining
                long timeRemaining = 1000 - ((DateTime.Now.Ticks / 1000L) - _startTime); // e-7 s
                
                if (timeRemaining > 0)
                {
                    Thread.Sleep((int)timeRemaining);
                }
            }

            #region Close database connection
            _connection.Close();
            #endregion

        }

        static IEnumerable<String> getPlayerList(MySqlConnection connection)
        {
            List<string> players;

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = @"select name from players";

            using (MySqlDataReader Reader = command.ExecuteReader())
            {
                players = new List<string>();

                while (Reader.Read())
                {
                    players.Add(Reader.GetString(0));
                }
            }

            return (players);
        }
    }
}
