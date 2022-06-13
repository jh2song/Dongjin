using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Dongjin.Classes
{
	class DBConnectClass
	{
		public static SQLiteConnection Conn { get; set; }

		public static void DBConnect(string databasePath)
		{
			try
			{
				Conn = new SQLiteConnection(databasePath);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
