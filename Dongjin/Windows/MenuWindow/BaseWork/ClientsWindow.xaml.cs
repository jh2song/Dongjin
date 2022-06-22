﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;
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
		List<TextBox> textBoxes = new List<TextBox>();
		
		public ClientsWindow()
		{
			InitializeComponent();

			conn = Classes.DB.Conn;
			syscContext = SynchronizationContext.Current;

			tb1.Text = DateTime.Now.Year.ToString().Substring(2, 2);
			tb2.Text = DateTime.Now.Month.ToString("00");
			tb3.Text = DateTime.Now.Day.ToString("00");

			// 거래처코드에 포커싱
			tb4.Focus();

			SetList();
		}

		public void SetList()
		{
			textBoxes.Add(tbDetail1);
			textBoxes.Add(tbDetail2);
			textBoxes.Add(tbDetail3);
			textBoxes.Add(tbDetail4);
			textBoxes.Add(tbDetail51);
			textBoxes.Add(tbDetail61);
			textBoxes.Add(tbDetail71);
			textBoxes.Add(tbDetail8);
			textBoxes.Add(tbDetail9);
			textBoxes.Add(tbDetail10);
		}

		private void TB4_KeyDown(object sender, KeyEventArgs e)
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

		public async void NextSession()
		{
			await conn.CreateTableAsync<Client>();

			string tb4Text = tb4.Text;

			List<Client> client =
				await conn.Table<Client>().Where(c => c.Code.Equals(tb4Text)).ToListAsync();


			if (client.Count == 0)
			{
				tbDetail1.Dispatcher.Invoke(() =>
				{
					tbDetail1.Focus();
				});
			}
			else
			{
				await SetTextBox(tbDetail1, client[0].Name);
				await SetTextBox(tbDetail2, client[0].Phone);
				await SetTextBox(tbDetail3, client[0].CurrentLeftMoney.ToString());
				await SetTextBox(tbDetail4, client[0].PercentCode.ToString());
				await SetTextBox(tbDetail51, client[0].LastTransactionDate.Year.ToString().Substring(2, 2));
				await SetTextBox(tbDetail52, client[0].LastTransactionDate.Month.ToString("00"));
				await SetTextBox(tbDetail53, client[0].LastTransactionDate.Day.ToString("00"));
				await SetTextBox(tbDetail61, client[0].LastMoneyComeDate.Year.ToString().Substring(2, 2));
				await SetTextBox(tbDetail62, client[0].LastMoneyComeDate.Month.ToString("00"));
				await SetTextBox(tbDetail63, client[0].LastMoneyComeDate.Day.ToString("00"));
				await SetTextBox(tbDetail71, client[0].LastReturnDate.Year.ToString().Substring(2, 2));
				await SetTextBox(tbDetail72, client[0].LastReturnDate.Month.ToString("00"));
				await SetTextBox(tbDetail73, client[0].LastReturnDate.Day.ToString("00"));
				await SetTextBox(tbDetail8, client[0].TodaySellMoney.ToString());
				await SetTextBox(tbDetail9, client[0].TodayDepositMoney.ToString());
				await SetTextBox(tbDetail10, client[0].TodayReturnMoney.ToString());


				tbcmd.Dispatcher.Invoke(() => 
				{
					tbcmd.Focus();
				});
			}
		}

		internal async Task SetTextBox(TextBox tb, string s)
		{
			tb.Dispatcher.Invoke(() =>
			{
				tb.Text = s;
			});

			await Task.Delay(50);
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
				tbcmd.Focus();

			if (e.Key == Key.Escape)
			{
				tbDetail4.Text = "";
				tb3.Focus();
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Key == Key.Tab)
			{
				e.Handled = true;
			}
			else
			{
				base.OnKeyDown(e);
			}
		}

		private void TBcmd_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				int updateNumber = int.Parse(tbcmd.Text);
				if (updateNumber >= 1 && updateNumber <= 10)
				{
					textBoxes[updateNumber - 1].Focus();
				}
				else if (tbcmd.Text == "D" || tbcmd.Text == "d")
				{
					try
					{
						// 데이터베이스에서 삭제
						conn.CreateTableAsync<Client>();
						conn.ExecuteAsync("DELETE FROM Client WHERE Code = ?;", int.Parse(tb4.Text));
					}
					catch (Exception ex)
					{
						MessageBox.Show("데이터베이스 삭제에 오류가 발생하였습니다", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
						Debug.WriteLine(ex.ToString());
					}
				}
				else
				{
					try
					{
						// 데이터베이스에 저장
						Client client = new Client()
						{
							Code = int.Parse(tb4.Text),
							Name = tbDetail1.Text,
							Phone = tbDetail2.Text,
							CurrentLeftMoney = int.Parse(tbDetail3.Text),
							PercentCode = int.Parse(tbDetail4.Text),
							LastTransactionDate = DateTime.Parse(tbDetail51.Text + "-" + tbDetail52.Text + "-" + tbDetail53.Text),
							LastMoneyComeDate = DateTime.Parse(tbDetail61.Text + "-" + tbDetail62.Text + "-" + tbDetail63.Text),
							LastReturnDate = DateTime.Parse(tbDetail71.Text + "-" + tbDetail72.Text + "-" + tbDetail73.Text),
							TodaySellMoney = int.Parse(tbDetail8.Text),
							TodayDepositMoney = int.Parse(tbDetail9.Text),
							TodayReturnMoney = int.Parse(tbDetail10.Text)
						};

						conn.CreateTableAsync<Client>();
						conn.InsertAsync(client);
					}
					catch (Exception ex)
					{
						MessageBox.Show("데이터베이스 저장에 오류가 발생하였습니다", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
						Debug.WriteLine(ex.ToString());
					}
				}
			}
			else if (e.Key == Key.Escape)
			{

			}
		}

		private void tbDetail51_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail52.Focus();
		}

		private void tbDetail52_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail53.Focus();
		}

		private void tbDetail53_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbcmd.Focus();
		}

		private void tbDetail61_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail62.Focus();
		}

		private void tbDetail62_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail63.Focus();
		}

		private void tbDetail63_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbcmd.Focus();
		}

		private void tbDetail71_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail72.Focus();
		}

		private void tbDetail72_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail73.Focus();
		}

		private void tbDetail73_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbcmd.Focus();
		}

		private void tbDetail8_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbcmd.Focus();
		}

		private void tbDetail9_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbcmd.Focus();
		}

		private void tbDetail10_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbcmd.Focus();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			conn?.CloseAsync();
		}
	}
}