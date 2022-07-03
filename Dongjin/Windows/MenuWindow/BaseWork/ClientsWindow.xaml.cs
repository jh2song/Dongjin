using System;
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
		private bool UpdateCommanding = false;

		public ClientsWindow()
		{
			InitializeComponent();

			conn = DB.Conn;
			syscContext = SynchronizationContext.Current;

			SetDate();

			// 거래처코드에 포커싱
			tb4.Focus();
			SetList();
		}

		private void SetDate()
		{
			tb1.Text = DateTime.Now.Year.ToString().Substring(2, 2);
			tb2.Text = DateTime.Now.Month.ToString("00");
			tb3.Text = DateTime.Now.Day.ToString("00");
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
				tbDetail1.Focus();
			}
			else
			{
				tbDetail1.Text = client[0].Name;
				tbDetail2.Text = client[0].Phone;
				tbDetail3.Text = client[0].CurrentLeftMoney.ToString();
				tbDetail4.Text = client[0].PercentCode.ToString();
				string target = client[0].LastTransactionDate.Year.ToString("0000");
				tbDetail51.Text = target.Substring(2, 2);
				tbDetail52.Text = client[0].LastTransactionDate.Month.ToString("00");
				tbDetail53.Text = client[0].LastTransactionDate.Day.ToString("00");
				target = client[0].LastMoneyComeDate.Year.ToString("0000");
				tbDetail61.Text = target.Substring(2, 2);
				tbDetail62.Text = client[0].LastMoneyComeDate.Month.ToString("00");
				tbDetail63.Text = client[0].LastMoneyComeDate.Day.ToString("00");
				target = client[0].LastReturnDate.Year.ToString("0000");
				tbDetail71.Text = target.Substring(2, 2);
				tbDetail72.Text = client[0].LastReturnDate.Month.ToString("00");
				tbDetail73.Text = client[0].LastReturnDate.Day.ToString("00");
				tbDetail8.Text = client[0].TodaySellMoney.ToString();
				tbDetail9.Text = client[0].TodayDepositMoney.ToString();
				tbDetail10.Text = client[0].TodayReturnMoney.ToString();
				tbDetail11.Text = client[0].MonthReleaseMoney.ToString();
				tbDetail12.Text = client[0].MonthDepositMoney.ToString();
				tbDetail13.Text = client[0].MonthReturnMoney.ToString();
				tbDetail14.Text = client[0].PrevMonthLeftMoney.ToString();

				tbcmd.Focus();
			}
		}

		private void tbDetail1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (UpdateCommanding)
				{
					tbcmd.Text = "";
					tbcmd.Focus();
				}
				else
				{
					tbDetail2.Focus();
				}
			}

			if (e.Key == Key.Escape)
			{
				if (tbDetail1.Text != "")
					tbDetail1.Text = "";
				else
				{
					if (UpdateCommanding)
						tbcmd.Focus();
					else
						tb4.Focus();
				}
			}
		}

		private void tbDetail2_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (UpdateCommanding)
				{
					tbcmd.Text = "";
					tbcmd.Focus();
				}
				else
				{
					tbDetail3.Focus();
				}
			}

			if (e.Key == Key.Escape)
			{
				if (tbDetail2.Text != "")
					tbDetail2.Text = "";
				else
				{
					if (UpdateCommanding)
						tbcmd.Focus();
					else
						tbDetail1.Focus();
				}
			}
		}

		private void tbDetail3_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (UpdateCommanding)
				{
					tbcmd.Text = "";
					tbcmd.Focus();
				}
				else
				{
					tbDetail4.Focus();
				}
			}

			if (e.Key == Key.Escape)
			{
				if (tbDetail3.Text != "")
					tbDetail3.Text = "";
				else
				{
					if (UpdateCommanding)
						tbcmd.Focus();
					else
						tbDetail2.Focus();
				}
			}
		}

		private void tbDetail4_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (UpdateCommanding)
				{
					tbcmd.Text = "";
					tbcmd.Focus();
				}
				else
				{
					InitializeDetails();
					tbcmd.Focus();
				}
			}

			if (e.Key == Key.Escape)
			{
				if (tbDetail4.Text != "")
					tbDetail4.Text = "";
				else
				{
					if (UpdateCommanding)
						tbcmd.Focus();
					else
						tbDetail3.Focus();
				}
			}
		}

		private void InitializeDetails()
		{
			if (onDBByCode == false)
			{
				tbDetail51.Text = "01";
				tbDetail52.Text = "01";
				tbDetail53.Text = "01";
				tbDetail61.Text = "01";
				tbDetail62.Text = "01";
				tbDetail63.Text = "01";
				tbDetail71.Text = "01";
				tbDetail72.Text = "01";
				tbDetail73.Text = "01";
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
				tbDetail4.Text = "0";

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
					UpdateCommanding = true;
					tbcmd.Text = "";
					switch (updateNumber)
					{
						case 1:
							tbDetail1.Focus();
							tbDetail1.Select(tbDetail1.Text.Length, 0);
							break;
						case 2:
							tbDetail2.Focus();
							tbDetail2.Select(tbDetail2.Text.Length, 0);
							break;
						case 3:
							tbDetail3.Focus();
							tbDetail3.Select(tbDetail3.Text.Length, 0);
							break;
						case 4:
							tbDetail4.Focus();
							tbDetail4.Select(tbDetail4.Text.Length, 0);
							break;
						case 5:
							tbDetail51.Focus();
							tbDetail51.Select(tbDetail51.Text.Length, 0);
							break;
						case 6:
							tbDetail61.Focus();
							tbDetail61.Select(tbDetail61.Text.Length, 0);
							break;
						case 7:
							tbDetail71.Focus();
							tbDetail71.Select(tbDetail71.Text.Length, 0);
							break;
						case 8:
							tbDetail8.Focus();
							tbDetail8.Select(tbDetail8.Text.Length, 0);
							break;
						case 9:
							tbDetail9.Focus();
							tbDetail9.Select(tbDetail9.Text.Length, 0);
							break;
						case 10:
							tbDetail10.Focus();
							tbDetail10.Select(tbDetail10.Text.Length, 0);
							break;
					}
				}
				else if (tbcmd.Text == "D" || tbcmd.Text == "d")
				{
					try
					{
						// 데이터베이스에서 삭제
						conn.CreateTable<Client>();
						int parsedCode;
						if (int.TryParse(tb4.Text, out parsedCode))
							conn.Execute($"DELETE FROM Client WHERE Code = {parsedCode};");
						
						onDBByCode = false;
						UpdateCommanding = false;
						foreach (TextBox tb in textBoxes)
							tb.Text = "";
						tbcmd.Text = "";
						tb4.Text = "";
						tb4.Focus();
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
						client.LastTransactionDate = new DateTime(int.Parse(tbDetail51.Text), int.Parse(tbDetail52.Text), int.Parse(tbDetail53.Text));
						client.LastMoneyComeDate = new DateTime(int.Parse(tbDetail61.Text), int.Parse(tbDetail62.Text), int.Parse(tbDetail63.Text));
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
						var targetClient = conn.Find<Client>(client.Code);
						if (targetClient == null)
							conn.Insert(client);
						else
							conn.Update(client);

						UpdateCommanding = false;
						onDBByCode = false;
						foreach (TextBox tb in textBoxes)
							tb.Text = "";
						tbcmd.Text = "";
						tb4.Text = "";
						tb4.Focus();
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

			if (e.Key == Key.Escape)
			{
				if (tbDetail51.Text != "")
					tbDetail51.Text = "";
				else
					tbcmd.Focus();
			}
		}

		private void tbDetail52_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail53.Focus();

			if (e.Key == Key.Escape)
			{
				if (tbDetail52.Text != "")
					tbDetail52.Text = "";
				else
					tbDetail51.Focus();
			}
		}

		private void tbDetail53_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbcmd.Focus();

			if (e.Key == Key.Escape)
			{
				if (tbDetail53.Text != "")
					tbDetail53.Text = "";
				else
					tbDetail52.Focus();
			}
		}

		private void tbDetail61_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail62.Focus();

			if (e.Key == Key.Escape)
			{
				if (tbDetail61.Text != "")
					tbDetail61.Text = "";
				else
					tbcmd.Focus();
			}
		}

		private void tbDetail62_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail63.Focus();

			if (e.Key == Key.Escape)
			{
				if (tbDetail62.Text != "")
					tbDetail62.Text = "";
				else
					tbDetail61.Focus();
			}
		}

		private void tbDetail63_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbcmd.Focus();

			if (e.Key == Key.Escape)
			{
				if (tbDetail63.Text != "")
					tbDetail63.Text = "";
				else
					tbDetail62.Focus();
			}
		}

		private void tbDetail71_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail72.Focus();

			if (e.Key == Key.Escape)
			{
				if (tbDetail71.Text != "")
					tbDetail71.Text = "";
				else
					tbcmd.Focus();
			}
		}

		private void tbDetail72_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbDetail73.Focus();

			if (e.Key == Key.Escape)
			{
				if (tbDetail72.Text != "")
					tbDetail72.Text = "";
				else
					tbDetail71.Focus();
			}
		}

		private void tbDetail73_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbcmd.Focus();

			if (e.Key == Key.Escape)
			{
				if (tbDetail73.Text != "")
					tbDetail73.Text = "";
				else
					tbDetail72.Focus();
			}
		}

		private void tbDetail8_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbcmd.Focus();

			if (e.Key == Key.Escape)
			{
				if (tbDetail8.Text != "")
					tbDetail8.Text = "";
				else
					tbcmd.Focus();
			}
		}

		private void tbDetail9_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbcmd.Focus();

			if (e.Key == Key.Escape)
			{
				if (tbDetail9.Text != "")
					tbDetail9.Text = "";
				else
					tbcmd.Focus();
			}
		}

		private void tbDetail10_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				tbcmd.Focus();

			if (e.Key == Key.Escape)
			{
				if (tbDetail10.Text != "")
					tbDetail10.Text = "";
				else
					tbcmd.Focus();
			}
		}
	}
}
