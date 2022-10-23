using Dongjin.Classes;
using Dongjin.Table;
using Dongjin.Windows.MenuWindow.CheckWork.Print;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Dongjin.Windows.MenuWindow.CheckWork
{
	/// <summary>
	/// Interaction logic for ClientLedgerWindow.xaml
	/// </summary>
	public partial class ClientLedgerWindow : Window
	{
		private int _clientCode;
		private DateTime _targetDateTime;
		private DateTime _MonthFirstDateTime;
		private List<ClientLedgerDatagrid> _clientLedgerData;
		private int _sumSell;
		private int _sumRefund;
		private int _sumDeposit;

		public ClientLedgerWindow()
		{
			InitializeComponent();

			YearTB.Focus();
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

		private void ClientCodeTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
		}

		private void YearTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && YearTB.Text == "")
			{
				Close();
			}
			if (e.Key == Key.Escape && YearTB.Text != "")
			{
				YearTB.Text = "";
			}

			if (e.Key == Key.Enter && YearTB.Text.Length < 2)
			{
				YearTB.Text = DateTime.Now.Year.ToString().Substring(2, 2);
				MonthTB.Focus();
			}
			if (e.Key == Key.Enter && YearTB.Text.Length == 2)
			{
				MonthTB.Focus();
			}
		}

		private void MonthTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && MonthTB.Text == "")
			{
				YearTB.Text = "";
				YearTB.Focus();
			}
			if (e.Key == Key.Escape && MonthTB.Text != "")
			{
				MonthTB.Text = "";
			}

			if (e.Key == Key.Enter && MonthTB.Text.Length < 2)
			{
				MonthTB.Text = DateTime.Now.Month.ToString("00");
				DayTB.Focus();
			}
			if (e.Key == Key.Enter && MonthTB.Text.Length == 2)
			{
				DayTB.Focus();
			}
		}

		private void DayTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && DayTB.Text == "")
			{
				MonthTB.Text = "";
				MonthTB.Focus();
			}
			if (e.Key == Key.Escape && DayTB.Text != "")
			{
				DayTB.Text = "";
			}

			if (e.Key == Key.Enter && DayTB.Text.Length < 2)
			{
				DayTB.Text = DateTime.Now.Day.ToString("00");
				ClientCodeTB.Focus();
			}
			if (e.Key == Key.Enter && DayTB.Text.Length == 2)
			{
				ClientCodeTB.Focus();
			}
		}

		class ClientLedgerDatagrid
		{
			public DateTime TransactionDate { get; set; }
			public int PrevLeftMoney { get; set; }
			public int TodaySellMoney { get; set; }
			public int TodayRefundMoney { get; set; }
			public int TodayDepositMoney { get; set; }
			public int CurrentLeftMoney { get; set; }
		}

		private void ClientCodeTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				DayTB.Text = ClientCodeTB.Text = ClientNameTB.Text = "";
				DayTB.Focus();
			}

			if (e.Key == Key.Enter && int.TryParse(ClientCodeTB.Text, out _clientCode))
			{
				if (!SetTargetDateTime())
				{
					YearTB.Text = MonthTB.Text = DayTB.Text = ClientCodeTB.Text = ClientNameTB.Text = "";
					YearTB.Focus();
					return;
				}

				if (!IsOnDBByClientCode(_clientCode))
					return;

				try
				{
					DB.Conn.CreateTable<ClientLedger>();

					var test = DB.Conn.Table<ClientLedger>().ToList().Where(cl => cl.ClientCode == _clientCode).ToList();

					List<ClientLedger> clientLedgers = DB.Conn.Table<ClientLedger>().ToList().Where(cl => cl.ClientCode == _clientCode &&
					cl.TransactionDate >= _MonthFirstDateTime &&
					cl.TransactionDate <= _targetDateTime).ToList();

					_clientLedgerData = new List<ClientLedgerDatagrid>();
					_sumSell = 0;
					_sumRefund = 0;
					_sumDeposit = 0;
					foreach (ClientLedger cl in clientLedgers)
					{
						var input = new ClientLedgerDatagrid();
						input.TransactionDate = cl.TransactionDate;
						input.TodaySellMoney = cl.TodaySellMoney;
						input.TodayRefundMoney = cl.TodayRefundMoney;
						input.TodayDepositMoney = cl.TodayDepositMoney;
						input.CurrentLeftMoney = cl.CurrentLeftMoney;
						input.PrevLeftMoney = input.CurrentLeftMoney - input.TodaySellMoney 
							+ input.TodayRefundMoney + input.TodayDepositMoney;

						_sumSell += input.TodaySellMoney;
						_sumRefund += input.TodayRefundMoney;
						_sumDeposit += input.TodayDepositMoney;

						_clientLedgerData.Add(input);
					}

					SumSellLB.Content = String.Format("{0:#,0}", _sumSell);
					SumRefundLB.Content = String.Format("{0:#,0}", _sumRefund);
					SumDepositLB.Content = String.Format("{0:#,0}", _sumDeposit);

					DG.ItemsSource = null;
					DG.ItemsSource = _clientLedgerData;

					ProgressTB.Focus();
				}
				catch (Exception)
				{


				}
			}
		}

		private bool SetTargetDateTime()
		{
			try
			{
				_targetDateTime = new DateTime(2000 + int.Parse(YearTB.Text), int.Parse(MonthTB.Text), int.Parse(DayTB.Text));
				_MonthFirstDateTime = new DateTime(_targetDateTime.Year, _targetDateTime.Month, 1);
			}
			catch (Exception)
			{
				MessageBox.Show("올바른 날짜를 입력하세요.", "날짜 오류", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
			return true;
		}

		private bool IsOnDBByClientCode(int clientCode)
		{
			try
			{
				DB.Conn.CreateTable<Client>();

				Client client = DB.Conn.Table<Client>().ToList().Where(cl => cl.ClientCode == clientCode).FirstOrDefault();
				if (client != null)
				{
					ClientNameTB.Text = client.ClientName;
					return true;
				}
				else
					return false;
			}
			catch (Exception)
			{

			}

			return false;
		}

		private void ProgressTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape || (e.Key == Key.Enter && (ProgressTB.Text != "E" && ProgressTB.Text != "e")))
			{
				DG.ItemsSource = null;
				SumSellLB.Content = SumRefundLB.Content = SumDepositLB.Content = "0";
				ClientCodeTB.Text = ClientNameTB.Text = "";
				ClientCodeTB.Focus();
			}
			
			if (e.Key == Key.Enter && (ProgressTB.Text == "E" || ProgressTB.Text == "e"))
			{
				Close();
			}
		}

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
			new PrintCLWindow(DG).Show();
			return;
        }
    }
}
