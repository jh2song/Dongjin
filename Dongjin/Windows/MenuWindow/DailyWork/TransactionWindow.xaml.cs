using Dongjin.Classes;
using Dongjin.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dongjin.Windows.MenuWindow.DailyWork
{
	/// <summary>
	/// Interaction logic for TransactionWindow.xaml
	/// </summary>
	public partial class TransactionWindow : Window
	{
		private int choice;
		private Client foundClient;

		public TransactionWindow()
		{
			InitializeComponent();

			ChoiceTB.Focus();
		}

		private void ChoiceTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (ChoiceTB.Text == "")
				{
					Close();
				}
				else
				{
					ChoiceTB.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (ChoiceTB.Text == "")
					return;

				choice = int.Parse(ChoiceTB.Text);

				YearTB.Focus();
			}
		}

		private void ChoiceTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotUnderboundNumber(e.Text, 1, 3);
		}

		private void YearTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
		}

		private void MonthTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
		}

		private void DayTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
		}

		private void YearTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (YearTB.Text == "")
				{
					ChoiceTB.Text = "";
					ChoiceTB.Focus();
				}
				else
				{
					YearTB.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (YearTB.Text.Length < 2)
					YearTB.Text = DateTime.Now.Year.ToString().Substring(2, 2);

				MonthTB.Focus();
			}
		}

		private void MonthTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (MonthTB.Text == "")
				{
					YearTB.Text = "";
					YearTB.Focus();
				}
				else
				{
					MonthTB.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (MonthTB.Text.Length < 2)
					MonthTB.Text = DateTime.Now.Month.ToString("00");

				DayTB.Focus();
			}
		}

		private void DayTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (DayTB.Text == "")
				{
					MonthTB.Text = "";
					MonthTB.Focus();
				}
				else
				{
					DayTB.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (DayTB.Text.Length < 2)
					DayTB.Text = DateTime.Now.Day.ToString("00");

				ClientCodeTB.Focus();
			}
		}

		private void ClientCodeTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
		}

		private void ClientCodeTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (ClientCodeTB.Text == "")
				{
					DayTB.Text = "";
					DayTB.Focus();
				}
				else
				{
					ClientCodeTB.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				// false: 거래처코드가 거래처 DB에 등록되어있지 않음
				// true: 거래처코드가 거래처 DB에 등록되어있음
				if (!ShowNameByCode(ClientCodeTB.Text))
					return;

				// do - something
				PrevMonthLeftMoneyTB.Text = GetPrevMonthLeftMoney();
				MonthSellMoneyTB.Text = foundClient.MonthSellMoney.ToString();
				MonthDepositMoneyTB.Text = foundClient.MonthDepositMoney.ToString();
				MonthReturnMoneyTB.Text = foundClient.MonthReturnMoney.ToString();
				//LastTransactionDateTB.Text = foundClient.LastTransactionDate.Year.ToString("0000").Substring(2, 2) + "/" 
				//	+ foundClient.LastTransactionDate.Month.ToString("00") + "/" 
				//	+ foundClient.LastTransactionDate.Day.ToString("00");
			}
		}

		private string GetPrevMonthLeftMoney()
		{
			try
			{
				DB.Conn.CreateTable<Transaction>();

				int inputClientCode = int.Parse(ClientCodeTB.Text);

				DateTime today = DateTime.Today;
				DateTime month = new DateTime(today.Year, today.Month, 1);
				DateTime last = month.AddDays(-1);

				var history = DB.Conn.Table<Transaction>()
					.Where(t => t.ClientCode == inputClientCode &&
					t.TransactionDate <= last)
					.OrderByDescending(d => d.TransactionDate).FirstOrDefault();

				if (history == null)
					return "0";
				else
					return history.CurrentLeftMoney.ToString();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				MessageBox.Show("전월말미수를 불러오는데 실패하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
				return "-1";
			}
		}

		private bool ShowNameByCode(string codeStr)
		{
			int parsedCode;
			if (int.TryParse(codeStr, out parsedCode))
			{
				try
				{
					DB.Conn.CreateTable<Client>();
					foundClient = DB.Conn.Find<Client>(c => c.ClientCode == parsedCode);
					if (foundClient == null)
						return false;
					else
					{
						ClientNameTB.Text = foundClient.ClientName;
						return true;
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.ToString());
					MessageBox.Show("거래처 DB에서 오류가 발생했습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}
			}
			else
			{
				return false;
			}
		}

	}
}
