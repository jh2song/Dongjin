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

		static string databaseName = "동진_데이터베이스.db";
		static string passwordHashName = "비밀번호_해시.dat";
		static string folderPath = @"C:\동진화장품";
		public static string databasePath = System.IO.Path.Combine(folderPath, databaseName);
		public static string passwordPath = System.IO.Path.Combine(folderPath, passwordHashName);

	}
}