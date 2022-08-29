using Dongjin.Classes;
using Dongjin.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Dongjin.Windows.MenuWindow.CheckWork
{
	/// <summary>
	/// Interaction logic for ResultByDepartmentWindow.xaml
	/// </summary>
	public partial class ResultByDepartmentWindow : Window
	{
		private int _choice;
		private DateTime _dateTime;
		private List<List<Label>> _todayLabelList;
		private List<List<Label>> _monthLabelList;

		public ResultByDepartmentWindow()
		{
			InitializeComponent();

			SetControlByList();

			ChoiceTB.Focus();
		}

		private void SetControlByList()
		{
			_todayLabelList = new List<List<Label>>();

			_todayLabelList.Add(new List<Label>() { 
				Today01SellMoneyLB, 
				Today01RefundMoneyLB, 
				Today01DepositMoneyLB, 
				Today01SellCountLB, 
				Today01RefundCountLB 
			});
			_todayLabelList.Add(new List<Label>() {
				Today02SellMoneyLB,
				Today02RefundMoneyLB,
				Today02DepositMoneyLB,
				Today02SellCountLB,
				Today02RefundCountLB
			});
			_todayLabelList.Add(new List<Label>() {
				Today03SellMoneyLB,
				Today03RefundMoneyLB,
				Today03DepositMoneyLB,
				Today03SellCountLB,
				Today03RefundCountLB
			});
			_todayLabelList.Add(new List<Label>() {
				Today04SellMoneyLB,
				Today04RefundMoneyLB,
				Today04DepositMoneyLB,
				Today04SellCountLB,
				Today04RefundCountLB
			});
			_todayLabelList.Add(new List<Label>() {
				Today05SellMoneyLB,
				Today05RefundMoneyLB,
				Today05DepositMoneyLB,
				Today05SellCountLB,
				Today05RefundCountLB
			});
			_todayLabelList.Add(new List<Label>() {
				Today06SellMoneyLB,
				Today06RefundMoneyLB,
				Today06DepositMoneyLB,
				Today06SellCountLB,
				Today06RefundCountLB
			});
			_todayLabelList.Add(new List<Label>() {
				Today07SellMoneyLB,
				Today07RefundMoneyLB,
				Today07DepositMoneyLB,
				Today07SellCountLB,
				Today07RefundCountLB
			});
			_todayLabelList.Add(new List<Label>() {
				TodayTotalSellMoneyLB,
				TodayTotalRefundMoneyLB,
				TodayTotalDepositMoneyLB,
				TodayTotalSellCountLB,
				TodayTotalRefundCountLB
			});

			_monthLabelList = new List<List<Label>>();

			_monthLabelList.Add(new List<Label>() {
				Month01SellMoneyLB,
				Month01RefundMoneyLB,
				Month01DepositMoneyLB,
				Month01SellCountLB,
				Month01RefundCountLB
			});
			_monthLabelList.Add(new List<Label>() {
				Month02SellMoneyLB,
				Month02RefundMoneyLB,
				Month02DepositMoneyLB,
				Month02SellCountLB,
				Month02RefundCountLB
			});
			_monthLabelList.Add(new List<Label>() {
				Month03SellMoneyLB,
				Month03RefundMoneyLB,
				Month03DepositMoneyLB,
				Month03SellCountLB,
				Month03RefundCountLB
			});
			_monthLabelList.Add(new List<Label>() {
				Month04SellMoneyLB,
				Month04RefundMoneyLB,
				Month04DepositMoneyLB,
				Month04SellCountLB,
				Month04RefundCountLB
			});
			_monthLabelList.Add(new List<Label>() {
				Month05SellMoneyLB,
				Month05RefundMoneyLB,
				Month05DepositMoneyLB,
				Month05SellCountLB,
				Month05RefundCountLB
			});
			_monthLabelList.Add(new List<Label>() {
				Month06SellMoneyLB,
				Month06RefundMoneyLB,
				Month06DepositMoneyLB,
				Month06SellCountLB,
				Month06RefundCountLB
			});
			_monthLabelList.Add(new List<Label>() {
				Month07SellMoneyLB,
				Month07RefundMoneyLB,
				Month07DepositMoneyLB,
				Month07SellCountLB,
				Month07RefundCountLB
			});
			_monthLabelList.Add(new List<Label>() {
				MonthTotalSellMoneyLB,
				MonthTotalRefundMoneyLB,
				MonthTotalDepositMoneyLB,
				MonthTotalSellCountLB,
				MonthTotalRefundCountLB
			});
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
			public int TodaySellMoney { get; set; }
			public int TodayRefundMoney { get; set; }
			public int TodayDepositMoney { get; set; }
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
;", _dateTime).OrderBy(t => t.ClientCode);

				int sumSell = 0;
				int sumRefund = 0;
				int sumDeposit = 0;
				int sumCurrentLeftMoney = 0;

				foreach(DatagridClass datagridClass in datagridData)
				{
					sumSell += datagridClass.TodaySellMoney;
					sumRefund += datagridClass.TodayRefundMoney;
					sumDeposit += datagridClass.TodayDepositMoney;
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

		class Money
		{
			public int Department { get; set; }
			public int SellMoney { get; set; }
			public int RefundMoney { get; set; }
			public int DepositMoney { get; set; }
		}

		class Count
		{
			public int Department { get; set; }
			public int SellCount { get; set; }
			public int RefundCount { get; set; }
		}

		private void ShowGrid()
		{
			// 일단 모두 0으로 초기화
			for (int i = 0; i < _todayLabelList.Count; i++)
				for (int j = 0; j < _todayLabelList[i].Count; j++)
					_todayLabelList[i][j].Content = "0";
			for (int i = 0; i < _monthLabelList.Count; i++)
				for (int j = 0; j < _monthLabelList[i].Count; j++)
					_monthLabelList[i][j].Content = "0";

			SetTodayContent();
			SetMonthContent();
		}

		private void SetTodayContent()
		{
			try
			{
				DB.Conn.CreateTable<ClientLedger>();
				var todayMoney = DB.Conn.Query<Money>(@"
SELECT SUBSTR(ClientCode, 1, 1) AS Department, SUM(TodaySellMoney) AS SellMoney, SUM(TodayRefundMoney) AS RefundMoney, SUM(TodayDepositMoney) AS DepositMoney
FROM ClientLedger
WHERE TransactionDate = ?
GROUP BY SUBSTR(ClientCode, 1, 1);
", _dateTime);

				DB.Conn.CreateTable<Document>();
				var todayCount = DB.Conn.Query<Count>(@"
SELECT SUBSTR(ClientCode, 1, 1) AS Department, SUM(CASE WHEN Choice = 1 THEN ProductCount ELSE 0 END) AS SellCount, SUM(CASE WHEN Choice = 3 THEN ProductCount ELSE 0 END) AS RefundCount
FROM Document
WHERE TransactionDate = ?
GROUP BY SUBSTR(ClientCode, 1, 1);
", _dateTime);

				int sumSellMoney = 0;
				int sumRefundMoney = 0;
				int sumDepositMoney = 0;
				int sumSellCount = 0;
				int sumRefundCount = 0;

				foreach (Money money in todayMoney)
				{
					if (money.Department < 1 || money.Department > 7)
						continue;

					// 각 과의 판매 금액
					_todayLabelList[money.Department - 1][0].Content = String.Format("{0:#,0}", money.SellMoney);
					// 각 과의 환입 금액
					_todayLabelList[money.Department - 1][1].Content = String.Format("{0:#,0}", money.RefundMoney);
					// 각 과의 입금 금액
					_todayLabelList[money.Department - 1][2].Content = String.Format("{0:#,0}", money.DepositMoney);

					sumSellMoney += money.SellMoney;
					sumRefundMoney += money.RefundMoney;
					sumDepositMoney += money.DepositMoney;
				}
				foreach (Count count in todayCount)
				{
					if (count.Department < 1 || count.Department > 7)
						continue;

					// 각 과의 판매 수
					_todayLabelList[count.Department - 1][3].Content = String.Format("{0:#,0}", count.SellCount);
					// 각 과의 환입 수
					_todayLabelList[count.Department - 1][4].Content = String.Format("{0:#,0}", count.RefundCount);

					sumSellCount += count.SellCount;
					sumRefundCount += count.RefundCount;
				}

				_todayLabelList.Last()[0].Content = String.Format("{0:#,0}", sumSellMoney);
				_todayLabelList.Last()[1].Content = String.Format("{0:#,0}", sumRefundMoney);
				_todayLabelList.Last()[2].Content = String.Format("{0:#,0}", sumDepositMoney);
				_todayLabelList.Last()[3].Content = String.Format("{0:#,0}", sumSellCount);
				_todayLabelList.Last()[4].Content = String.Format("{0:#,0}", sumRefundCount);
			}
			catch (Exception)
			{
				MessageBox.Show("당일분을 불러오는데 실패하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void SetMonthContent()
		{
			try
			{
				DB.Conn.CreateTable<ClientLedger>();
				var monthMoney = DB.Conn.Query<Money>(@"
SELECT SUBSTR(ClientCode, 1, 1) AS Department, SUM(TodaySellMoney) AS SellMoney, SUM(TodayRefundMoney) AS RefundMoney, SUM(TodayDepositMoney) AS DepositMoney
FROM ClientLedger
WHERE STRFTIME('%m', TransactionDate) = STRFTIME('%m', ?) AND STRFTIME('%Y', TransactionDate) = STRFTIME('%Y', ?)
GROUP BY SUBSTR(ClientCode, 1, 1);
", _dateTime, _dateTime);

				DB.Conn.CreateTable<Document>();
				var monthCount = DB.Conn.Query<Count>(@"
SELECT SUBSTR(ClientCode, 1, 1) AS Department, SUM(CASE WHEN Choice = 1 THEN ProductCount ELSE 0 END) AS SellCount, SUM(CASE WHEN Choice = 3 THEN ProductCount ELSE 0 END) AS RefundCount
FROM Document
WHERE STRFTIME('%m', TransactionDate) = STRFTIME('%m', ?) AND STRFTIME('%Y', TransactionDate) = STRFTIME('%Y', ?)
GROUP BY SUBSTR(ClientCode, 1, 1);
", _dateTime, _dateTime);

				int sumSellMoney = 0;
				int sumRefundMoney = 0;
				int sumDepositMoney = 0;
				int sumSellCount = 0;
				int sumRefundCount = 0;

				foreach (Money money in monthMoney)
				{
					if (money.Department < 1 || money.Department > 7)
						continue;

					// 각 과의 판매 금액
					_monthLabelList[money.Department - 1][0].Content = String.Format("{0:#,0}", money.SellMoney);
					// 각 과의 환입 금액
					_monthLabelList[money.Department - 1][1].Content = String.Format("{0:#,0}", money.RefundMoney);
					// 각 과의 입금 금액
					_monthLabelList[money.Department - 1][2].Content = String.Format("{0:#,0}", money.DepositMoney);

					sumSellMoney += money.SellMoney;
					sumRefundMoney += money.RefundMoney;
					sumDepositMoney += money.DepositMoney;
				}
				foreach (Count count in monthCount)
				{
					if (count.Department < 1 || count.Department > 7)
						continue;

					// 각 과의 판매 수
					_monthLabelList[count.Department - 1][3].Content = String.Format("{0:#,0}", count.SellCount);
					// 각 과의 환입 수
					_monthLabelList[count.Department - 1][4].Content = String.Format("{0:#,0}", count.RefundCount);

					sumSellCount += count.SellCount;
					sumRefundCount += count.RefundCount;
				}

				_monthLabelList.Last()[0].Content = String.Format("{0:#,0}", sumSellMoney);
				_monthLabelList.Last()[1].Content = String.Format("{0:#,0}", sumRefundMoney);
				_monthLabelList.Last()[2].Content = String.Format("{0:#,0}", sumDepositMoney);
				_monthLabelList.Last()[3].Content = String.Format("{0:#,0}", sumSellCount);
				_monthLabelList.Last()[4].Content = String.Format("{0:#,0}", sumRefundCount);
			}
			catch (Exception)
			{
				MessageBox.Show("당일분을 불러오는데 실패하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
