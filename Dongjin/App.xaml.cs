using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Dongjin
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		// 기본 비밀번호 1234에 대한 해시값 (주의. 숫자패드에 숫자가 아님. 글자판 위에 숫자임)
		public static string pwHash;

		static string databaseName = "Contacts.db";
		static string passwordHashName = "PasswordHash.dat";
		static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		public static string databasePath = System.IO.Path.Combine(folderPath, databaseName);
		public static string passwordPath = System.IO.Path.Combine(folderPath, passwordHashName);

	}
}
