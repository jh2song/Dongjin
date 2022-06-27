using Dongjin.Classes;
using Dongjin.Table;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dongjin.Windows.MenuWindow.BaseWork
{
	/// <summary>
	/// Interaction logic for CompanyWindow.xaml
	/// </summary>
	public partial class CompanyWindow : Window
	{
		private SQLiteConnection _conn;
		public CompanyWindow()
		{
			InitializeComponent();

			_conn = DB.Conn;
			TB1.Focus();
		}

		private void TB1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (TB1.Text.Length == 0)
					Close();
				else
					TB1.Text = "";
			}

			if (e.Key == Key.Enter)
			{
				TB2.Text = GetNameByCode(TB1.Text);

				if (TB2.Text == "")
				{
					TB2.Focus();
				}
				else
				{
					TBCmd.Focus();
				}
			}
		}

		private void TB2_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (TB2.Text == "")
				{
					TB1.Focus();
				}
				else
				{
					TB2.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				TBCmd.Focus();
			}
		}

		private void TBCmd_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (TBCmd.Text == "")
				{
					TB2.Focus();
				}
				else
				{
					TBCmd.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (TBCmd.Text == "D" || TBCmd.Text == "d")
				{
					DeleteDB();
					TB1.Text = TB2.Text = TBCmd.Text = "";
					TB1.Focus();
				}
				else
				{
					SaveDB();
					TB1.Text = TB2.Text = TBCmd.Text = "";
					TB1.Focus();
				}
			}
		}

		public static string GetNameByCode(string code)
		{
			DB.Conn.CreateTable<Company>();

			List<Company> company =
				DB.Conn.Table<Company>().Where(c => c.Code.Equals(code)).ToList();

			if (company.Count > 0)
			{ 
				return company[0].Name;
			}
			
			return "";
		}

		private void DeleteDB()
		{
			try
			{
				_conn.CreateTable<Company>();
				int parsedCode;
				if (int.TryParse(TB1.Text, out parsedCode))
					_conn.Execute($"DELETE FROM Company WHERE Code = {parsedCode};");
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				MessageBox.Show("데이터베이스 삭제에 오류가 발생하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void SaveDB()
		{
			try
			{
				Company company = new Company();
				company.Code = int.Parse(TB1.Text);
				company.Name = TB2.Text;

				var targetCompany = _conn.Find<Company>(company.Code);
				if (targetCompany == null)
					_conn.Insert(company);
				else
					_conn.Update(company);
			}	
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				MessageBox.Show("데이터베이스 저장에 오류가 발생하였습니다", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
