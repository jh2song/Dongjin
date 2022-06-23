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
		private SQLiteConnection conn;
		private SynchronizationContext syscContext;
		List<TextBox> textBoxes = new List<TextBox>();
		private bool onDBByCode = true;

		public ClientsWindow()
		{
			InitializeComponent();

			conn = DB.Conn;
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
			textBoxes.Add(tbDetail52);
			textBoxes.Add(tbDetail53);
			textBoxes.Add(tbDetail61);
			textBoxes.Add(tbDetail62);
			textBoxes.Add(tbDetail63);
			textBoxes.Add(tbDetail71);
			textBoxes.Add(tbDetail72);
			textBoxes.Add(tbDetail73);
			textBoxes.Add(tbDetail8);
			textBoxes.Add(tbDetail9);
			textBoxes.Add(tbDetail10);
			textBoxes.Add(tbDetail11);
			textBoxes.Add(tbDetail12);
			textBoxes.Add(tbDetail13);
			textBoxes.Add(tbDetail14);
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

		public void NextSession()
		{
			conn.CreateTable<Client>();

			string tb4Text = tb4.Text;

			List<Client> client =
				conn.Table<Client>().Where(c => c.Code.Equals(tb4Text)).ToList();


			if (client.Count == 0)
			{
				onDBByCode = false;
				tbDetail1.Dispatcher.Invoke(() =>
				{
					tbDetail1.Focus();
				});
			}
			else
			{
				SetTextBox(tbDetail1, client[0].Name);
				SetTextBox(tbDetail2, client[0].Phone);
				SetTextBox(tbDetail3, client[0].CurrentLeftMoney.ToString());
				SetTextBox(tbDetail4, client[0].PercentCode.ToString());
				string target = client[0].LastTransactionDate.Year.ToString("0000");
				SetTextBox(tbDetail51, target.Substring(2, 2));
				SetTextBox(tbDetail52, client[0].LastTransactionDate.Month.ToString("00"));
				SetTextBox(tbDetail53, client[0].LastTransactionDate.Day.ToString("00"));
				target = client[0].LastMoneyComeDate.Year.ToString("0000");
				SetTextBox(tbDetail61, target.Substring(2, 2));
				SetTextBox(tbDetail62, client[0].LastMoneyComeDate.Month.ToString("00"));
				SetTextBox(tbDetail63, client[0].LastMoneyComeDate.Day.ToString("00"));
				target = client[0].LastReturnDate.Year.ToString("0000");
				SetTextBox(tbDetail71, target.Substring(2, 2));
				SetTextBox(tbDetail72, client[0].LastReturnDate.Month.ToString("00"));
				SetTextBox(tbDetail73, client[0].LastReturnDate.Day.ToString("00"));
				SetTextBox(tbDetail8, client[0].TodaySellMoney.ToString());
				SetTextBox(tbDetail9, client[0].TodayDepositMoney.ToString());
				SetTextBox(tbDetail10, client[0].TodayReturnMoney.ToString());


				tbcmd.Dispatcher.Invoke(() => 
				{
					tbcmd.Focus();
				});
			}
		}

		internal void SetTextBox(TextBox tb, string s)
		{
			tb.Dispatcher.Invoke(() =>
			{
				tb.Text = s;
			});

			Task.Delay(50);
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
			{
				if (onDBByCode == false)
				{
					tbDetail51.Text = "00";
					tbDetail52.Text = "00";
					tbDetail53.Text = "00";
					tbDetail61.Text = "00";
					tbDetail62.Text = "00";
					tbDetail63.Text = "00";
					tbDetail71.Text = "00";
					tbDetail72.Text = "00";
					tbDetail73.Text = "00";
					tbDetail8.Text = "0";
					tbDetail9.Text = "0";
					tbDetail10.Text = "0";
					tbDetail11.Text = "0";
					tbDetail12.Text = "0";
					tbDetail13.Text = "0";
					tbDetail14.Text = "0";
				}

				if (tbDetail3.Text == "")
					tbDetail3.Text = "0";
				if (tbDetail4.Text == "")
					tbDetail4.Text = "8";

				tbcmd.Focus();
			}

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
				int updateNumber;
				int.TryParse(tbcmd.Text, out updateNumber);

				if (updateNumber >= 1 && updateNumber <= 10)
				{
					textBoxes[updateNumber - 1].Focus();
				}
				else if (tbcmd.Text == "D" || tbcmd.Text == "d")
				{
					try
					{
						// 데이터베이스에서 삭제
						conn.CreateTable<Client>();
						conn.Execute("DELETE FROM Client WHERE Code = ?;", int.Parse(tb4.Text));
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
						Client client = new Client();
						int target;

						// 프로퍼티를 초기화 했으므로 (C# High-Version)
						// 만일의 상황을 대비해 이렇게 코딩함
						if (int.TryParse(tb4.Text, out target))
						{
							client.Code = target;
						}
						if (tbDetail1.Text != "")
						{
							client.Name = tbDetail1.Text;
						}
						if (tbDetail2.Text != "")
						{
							client.Phone = tbDetail2.Text;
						}
						if (int.TryParse(tbDetail3.Text, out target))
						{
							client.CurrentLeftMoney = target;
						}
						if (int.TryParse(tbDetail4.Text, out target))
						{
							client.PercentCode = target;
						}
						if (tbDetail51.Text != "00" && tbDetail52.Text != "00" && tbDetail53.Text != "00")
							client.LastTransactionDate = new DateTime(int.Parse(tbDetail51.Text), int.Parse(tbDetail52.Text), int.Parse(tbDetail53.Text));
						if (tbDetail61.Text != "00" && tbDetail62.Text != "00" && tbDetail63.Text != "00")
							client.LastMoneyComeDate = new DateTime(int.Parse(tbDetail61.Text), int.Parse(tbDetail62.Text), int.Parse(tbDetail63.Text));
						if (tbDetail71.Text != "00" && tbDetail72.Text != "00" && tbDetail73.Text != "00")
							client.LastReturnDate = new DateTime(int.Parse(tbDetail71.Text), int.Parse(tbDetail72.Text), int.Parse(tbDetail73.Text));
						if (int.TryParse(tbDetail8.Text, out target))
						{
							client.TodaySellMoney = target;
						}
						if (int.TryParse(tbDetail9.Text, out target))
						{
							client.TodayDepositMoney = target;
						}
						if (int.TryParse(tbDetail10.Text, out target))
						{
							client.TodayReturnMoney = target;
						}

						conn.CreateTable<Client>();
						conn.Insert(client);
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
				foreach (TextBox tb in textBoxes)
					tb.Text = "";
				tbcmd.Text = "";
				tb4.Text = "";
				tb4.Focus();
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
	}
}
