using System;

namespace AwesomeGame2
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			using (AwesomeGame2 game = AwesomeGame2.Instance)
			{
				game.Run();
			}
		}
	}
}

