using System.Windows;

namespace Dongjin
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		static string databaseName = "Dongjin.db";
		private static string folderPath = @"C:\동진화장품";
		public static string databasePath = System.IO.Path.Combine(folderPath, databaseName);
	}
}