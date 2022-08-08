using Dongjin.Classes;
using Dongjin.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dongjin.Windows.MenuWindow.CheckWork
{
	/// <summary>
	/// Interaction logic for ResultByDepartmentWindow.xaml
	/// </summary>
	public partial class ResultByDepartmentWindow : Window
	{
		private int _choice;
		private DateTime _dateTime;

		public ResultByDepartmentWindow()
		{
			InitializeComponent();

			ChoiceTB.Focus();
		}

		private void ChoiceTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
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

		private void ChoiceTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && ChoiceTB.Text == "")
			{
				Close();
			}

			if (e.Key == Key.Escape && ChoiceTB.Text != "")
			{
				ChoiceTB.Text = "";
			}

			if (e.Key == Key.Enter && int.TryParse(ChoiceTB.Text, out _choice))
			{
				if (_choice == 1 || _choice == 2)
				{
					YearTB.Focus();
				}
			}
		}

		private void YearTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && YearTB.Text == "")
			{
				ChoiceTB.Text = "";
				ChoiceTB.Focus();
			}

			if (e.Key == Key.Escape && YearTB.Text != "")
			{
				YearTB.Text = "";
			}

			if (e.Key == Key.Enter)
			{
				if (YearTB.Text.Length < 2)
					YearTB.Text = DateTime.Now.Year.ToString().Substring(2, 2);

				MonthTB.Focus();
			}
		}

		private void MonthTB_KeyDown(object sender, KeyEventArgs e)
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

		private void DayTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				// 아래 표들 초기화
				DG.ItemsSource = null;
				SumSellLB.Content = SumRefundLB.Content = SumDepositLB.Content = SumCurrentLeftMoneyLB.Content = "";
				DG.Visibility = DGSub.Visibility = Visibility.Hidden;
				Option2Grid.Visibility = Visibility.Hidden;

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

				if (VerifyDate() == false)
				{
					YearTB.Text = MonthTB.Text = DayTB.Text = "";
					YearTB.Focus();

					return;
				}

				if (_choice == 1)
				{
					DG.Visibility = DGSub.Visibility = Visibility.Visible;
					ShowDatagrid();
				}

				if (_choice == 2)
				{
					Option2Grid.Visibility= Visibility.Visible;
					ShowGrid();
				}

				DayTB.Select(DayTB.Text.Length, 0);
			}
		}

		private bool VerifyDate()
		{
			try
			{
				_dateTime = new DateTime(int.Parse("20" + YearTB.Text), int.Parse(MonthTB.Text), int.Parse(DayTB.Text));
				return true;
			}
			catch (Exception)
			{
				MessageBox.Show("날짜가 잘못되었습니다.", "날짜 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			return false;
		}

		class DatagridClass
		{
			public int ClientCode { get; set; }
			public string ClientName { get; set; }
			public int SellMoney { get; set; }
			public int RefundMoney { get; set; }
			public int DepositMoney { get; set; }
			public int CurrentLeftMoney { get; set; }
		}

		private void ShowDatagrid()
		{
			try
			{
				DB.Conn.CreateTable<ClientLedger>();
				DB.Conn.CreateTable<Client>();

				var datagridData = DB.Conn.Query<DatagridClass>(@"
SELECT a.ClientCode, b.ClientName, a.TodaySellMoney, a.TodayRefundMoney, a.TodayDepositMoney, a.CurrentLeftMoney
FROM ClientLedger AS a, Client AS b
WHERE a.ClientCode = b.ClientCode AND a.TransactionDate = ?
;", _dateTime);

				int sumSell = 0;
				int sumRefund = 0;
				int sumDeposit = 0;
				int sumCurrentLeftMoney = 0;

				foreach(DatagridClass datagridClass in datagridData)
				{
					sumSell += datagridClass.SellMoney;
					sumRefund += datagridClass.RefundMoney;
					sumDeposit += datagridClass.DepositMoney;
					sumCurrentLeftMoney += datagridClass.CurrentLeftMoney;
				}

				SumSellLB.Content = String.Format("{0:#,0}", sumSell);
				SumRefundLB.Content = String.Format("{0:#,0}", sumRefund);
				SumDepositLB.Content = String.Format("{0:#,0}", sumDeposit);
				SumCurrentLeftMoneyLB.Content = String.Format("{0:#,0}", sumCurrentLeftMoney);
				DG.ItemsSource = datagridData;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void ShowGrid()
		{
			try
			{
				var today01Money = DB.Conn.Query(@"
SELECT SUBSTR(ClientCode, 1, 1), SUM(
FROM ClientLedger

GROUP BY SUBSTR(ClientCode, 1, 1);
");


				Today01SellMoneyLB.Content = String.Format("{0:#,0}", today01Money.Sum(cl => cl.TodaySellMoney));
				Today01RefundMoneyLB.Content = String.Format("{0:#,0}", today01Money.Sum(cl => cl.TodayRefundMoney));
				Today01DepositMoneyLB.Content = String.Format("{0:#,0}", today01Money.Sum(cl => cl.TodayDepositMoney));
			}
			catch (Exception)
			{
				MessageBox.Show("과별집계 테이블을 불러오는데 실패하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
