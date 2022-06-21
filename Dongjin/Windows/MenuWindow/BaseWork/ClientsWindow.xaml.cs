using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Dongjin.Classes;
using Dongjin.Table;
using SQLite;

namespace Dongjin.Windows.MenuWindow.BaseWork
{
	/// <summary>
	/// ClientsWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ClientsWindow : Window
	{
		private SQLiteAsyncConnection conn;
		private SynchronizationContext syscContext;

		public ClientsWindow()
		{
			InitializeComponent();

			conn = DBAsyncConnectClass.Conn;
			syscContext = SynchronizationContext.Current;

			tb1.Text = DateTime.Now.Year.ToString().Substring(2, 2);
			tb2.Text = DateTime.Now.Month.ToString("00");
			tb3.Text = DateTime.Now.Day.ToString("00");

			// 거래처코드에 포커싱
			tb4.Focus();
		}

		private void SetList()
		{
		}

		private void TB4_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (tb4.Text == "")
					Close();
				else
					tb4.Text = "";
			}

			if (e.Key == Key.Enter)
			{
				NextSession();
			}
		}

		private void NextSession()
		{
			Task.Factory.StartNew(() =>
			{
				this.syscContext.Post((a) =>
				{
					conn.CreateTableAsync<Client>();

					var client =
						conn.Table<Client>().Where(c => c.Code.Equals(tb4.Text)).ToListAsync();
					

					if (client == null)
					{
						this.tbDetail1.Focus();
					}
					else
					{
						this.tbDetail1.Text = client.Name;
						this.tbDetail2.Text = client.Phone;
						this.tbDetail3.Text = client.CurrentLeftMoney.ToString();
						this.tbDetail4.Text = client.PercentCode.ToString();
						this.tbDetail51.Text = client.LastTransactionDate.Year.ToString().Substring(2, 2);
						this.tbDetail52.Text = client.LastTransactionDate.Month.ToString("00");
						this.tbDetail53.Text = client.LastTransactionDate.Day.ToString("00");
						this.tbDetail61.Text = client.LastMoneyComeDate.Year.ToString().Substring(2, 2);
						this.tbDetail62.Text = client.LastMoneyComeDate.Month.ToString("00");
						this.tbDetail63.Text = client.LastMoneyComeDate.Day.ToString("00");
						this.tbDetail71.Text = client.LastReturnDate.Year.ToString().Substring(2, 2);
						this.tbDetail72.Text = client.LastReturnDate.Month.ToString("00");
						this.tbDetail73.Text = client.LastReturnDate.Day.ToString("00");
						this.tbDetail8.Text = client.TodaySellMoney.ToString();
						this.tbDetail9.Text = client.TodayDepositMoney.ToString();
						this.tbDetail10.Text = client.TodayReturnMoney.ToString();

						this.tbInput.Focus();
					}
				}
				, null);
			});
			

		}

		private void tbDetail1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail2.Focus();

			if (e.Key == Key.Escape)
			{
				tbDetail1.Text = "";
				tb4.Focus();
			}
		}

		private void tbDetail2_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail3.Focus();

			if (e.Key == Key.Escape)
			{
				tbDetail2.Text = "";
				tb1.Focus();
			}
		}

		private void tbDetail3_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail4.Focus();

			if (e.Key == Key.Escape)
			{
				tbDetail3.Text = "";
				tb2.Focus();
			}
		}

		private void tbDetail4_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbInput.Focus();

			if (e.Key == Key.Escape)
			{
				tbDetail4.Text = "";
				tb3.Focus();
			}
		}

	}
}
