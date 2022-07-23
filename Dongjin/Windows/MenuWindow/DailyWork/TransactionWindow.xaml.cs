using Dongjin.Classes;
using Dongjin.Table;
using Dongjin.Windows.MenuWindow.BaseWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
		private bool bubble;
		private int appendChoice;
		private string productName;
		private int productCount;
		private Product productObject;
		private double productDiscountRate;
		private DateTime transactionDate;
		private int currentLeftMoney;
		private int clientCode;

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
					ClientNameTB.Text = "";
					ClientCodeTB.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				e.Handled = true;

				transactionDate = new DateTime(int.Parse("20" + YearTB.Text), int.Parse(MonthTB.Text), int.Parse(DayTB.Text));

				if (choice == 2)
				{
					var check = MessageBox.Show("(덤)전표가 맞습니까?", "확인", MessageBoxButton.OKCancel, MessageBoxImage.Question);
					bubble = true;
					if (check == MessageBoxResult.Cancel)
					{
						return;
					}
				}
				else if (choice == 3)
				{
					var check = MessageBox.Show("환입전표가 맞습니까?", "확인", MessageBoxButton.OKCancel, MessageBoxImage.Question);
					bubble = true;
					if (check == MessageBoxResult.Cancel)
					{
						return;
					}
				}

				
				if (!int.TryParse(ClientCodeTB.Text, out clientCode))
				{
					return;
				}

				// false: 거래처코드가 거래처 DB에 등록되어있지 않음
				// true: 거래처코드가 거래처 DB에 등록되어있음
				if (!ShowNameByCode(ClientCodeTB.Text))
					return;

				// do - something
				PrevMonthLeftMoneyTB.Text = String.Format("{0:#,0}", GetPrevMonthLeftMoney());
				MonthSellMoneyTB.Text = String.Format("{0:#,0}", GetMonthSellMoney());
				MonthDepositMoneyTB.Text = String.Format("{0:#,0}", GetMonthDepositMoney());
				MonthRefundMoneyTB.Text = String.Format("{0:#,0}", GetMonthRefundMoney());
				LastTransactionDateTB.Text = foundClient.FinalTransactionDate.Year.ToString("0000").Substring(2, 2) + "/" 
					+ foundClient.FinalTransactionDate.Month.ToString("00") + "/" 
					+ foundClient.FinalTransactionDate.Day.ToString("00");

				currentLeftMoney = int.Parse(PrevMonthLeftMoneyTB.Text.Replace(",",""))
					+ int.Parse(MonthSellMoneyTB.Text.Replace(",", ""))
					- int.Parse(MonthDepositMoneyTB.Text.Replace(",", ""))
					- int.Parse(MonthRefundMoneyTB.Text.Replace(",", ""));
				CurrentLeftMoneyTB.Text = String.Format("{0:#,0}", (currentLeftMoney));

				if (IsOnDBByChoiceDateCode()) // 전표가 이미 남아있는 상황
				{
					// Datagrid를 표시해야함
					try
					{
						DB.Conn.CreateTable<Transaction>();
						var list = DB.Conn.Table<Transaction>().Where(t => t.Choice == choice &&
																			t.TransactionDate == transactionDate &&
																			t.ClientCode == clientCode).ToList();
						DG.ItemsSource = null;
						DG.ItemsSource = list;
						DG.Visibility = Visibility.Visible;

						int totalCount = 0;
						int totalDiscountPrice = 0;
						int totalPrice = 0;
						foreach (var element in list)
						{
							totalCount += element.ProductCount;
							totalDiscountPrice += element.DiscountPrice;
							totalPrice += element.Price;
						}

						TotalCountTB.Text = String.Format("{0:#,0}", totalCount);
						TotalDiscountPriceTB.Text = String.Format("{0:#,0", totalDiscountPrice);
						TotalPriceTB.Text = String.Format("{0:#,0", totalPrice);

					}
					catch (Exception ex)
					{
						Debug.WriteLine(ex.ToString());
					}
					// 추가 선택을 고르게함
					AppendStackPanel.Visibility = Visibility.Visible;
					AppendTB.Focus();
				}
				else // 새로 전표 찍어야 되는 상황
				{
					appendChoice = 0;
					InputProductAndOptionGrid.Visibility = Visibility.Visible;
					ProductCodeTB.Focus();
				}
			}
		}

		private int GetMonthSellMoney()
		{
			int ret = 0;
			try
			{
				DB.Conn.CreateTable<ClientLedger>();
				var list = DB.Conn.Table<ClientLedger>().ToList().Where(cl => cl.TransactionDate.Year == transactionDate.Year &&
														cl.TransactionDate.Month == transactionDate.Month);

				foreach (var element in list)
				{
					ret += element.TodaySellMoney;
				}
			}
			catch (Exception)
			{
				MessageBox.Show("당월판매액을 불러오는데 실패하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			return ret;
		}

		private int GetMonthDepositMoney()
		{
			int ret = 0;
			try
			{
				DB.Conn.CreateTable<ClientLedger>();
				var list = DB.Conn.Table<ClientLedger>().ToList().Where(cl => cl.TransactionDate.Year == transactionDate.Year &&
														cl.TransactionDate.Month == transactionDate.Month);

				foreach (var element in list)
				{
					ret += element.TodayDepositMoney;
				}
			}
			catch (Exception)
			{
				MessageBox.Show("당월입금액을 불러오는데 실패하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			return ret;
		}

		private int GetMonthRefundMoney()
		{
			int ret = 0;
			try
			{
				DB.Conn.CreateTable<ClientLedger>();
				var list = DB.Conn.Table<ClientLedger>().ToList().Where(cl => cl.TransactionDate.Year == transactionDate.Year &&
														cl.TransactionDate.Month == transactionDate.Month);

				foreach (var element in list)
				{
					ret += element.TodayRefundMoney;
				}
			}
			catch (Exception)
			{
				MessageBox.Show("당월환입액을 불러오는데 실패하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			return ret;
		}

		private bool IsOnDBByChoiceDateCode()
		{
			DB.Conn.CreateTable<Transaction>();

			// 쿼리를 위한 준비물
			int choice = int.Parse(ChoiceTB.Text);
			int code = int.Parse(ClientCodeTB.Text);

			List<Transaction> trans = DB.Conn.Table<Transaction>().Where(t => t.Choice == choice &&
			t.TransactionDate == transactionDate &&
			t.ClientCode == code
			).ToList();

			if (trans.Count > 0)
				return true;
			else
				return false;
		}

		private string GetPrevMonthLeftMoney()
		{
			try
			{
				DateTime today = transactionDate;
				DateTime month = new DateTime(today.Year, today.Month, 1);
				DateTime last = month.AddDays(-1);

				DB.Conn.CreateTable<ClientLedger>();

				var history = DB.Conn.Table<ClientLedger>()
					.Where(t => t.ClientCode == clientCode &&
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

		// block bubbling
		private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (bubble)
			{
				e.Handled = true;
				bubble = false;
				return;
			}
		}

		private void AppendTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (AppendTB.Text == "")
				{
					AppendStackPanel.Visibility = Visibility.Hidden;
					ClientCodeTB.Text = "";
					ClientCodeTB.Focus();
				}
				else
				{
					AppendTB.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				// do something
				if (AppendTB.Text == "")
					appendChoice = 0;
				else
					appendChoice = int.Parse(AppendTB.Text);

				AppendTB.Text = "";
				AppendStackPanel.Visibility = Visibility.Hidden;
				InputProductAndOptionGrid.Visibility = Visibility.Visible;
				ProductCodeTB.Focus();
			}
		}

		private void AppendTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotUnderboundNumber(e.Text, 1, 2);
		}

		private void ProductCodeTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (ProductCodeTB.Text == "")
				{
					PrintOptionLB.Visibility = Visibility.Visible;
					InputProductAndOptionGrid.Visibility = Visibility.Hidden;
					ClientCodeTB.Text = "";
					ClientNameTB.Text = "";
					ClientCodeTB.Focus();
					DG.ItemsSource = null;
					DG.Visibility = Visibility.Hidden;
				}
				else
					ProductCodeTB.Text = "";
			}

			if (e.Key == Key.Enter)
			{
				PrintOptionLB.Visibility = Visibility.Hidden;
				if (ProductCodeTB.Text == "P" || ProductCodeTB.Text == "p")
				{

					return;
				}
				if (ProductCodeTB.Text == "P1" || ProductCodeTB.Text == "p1")
				{

					return;
				}
				if (ProductCodeTB.Text == "P2" || ProductCodeTB.Text == "p2")
				{

					return;
				}
				if (ProductCodeTB.Text == "P0" || ProductCodeTB.Text == "p0")
				{

					return;
				}
				if (ProductCodeTB.Text == "I" || ProductCodeTB.Text == "i")
				{
					ProductCodeTB.Visibility = PrintOptionLB.Visibility = Visibility.Hidden;
					DepositLB.Visibility = DepositTB.Visibility = Visibility.Visible;
					DepositTB.Focus();
					return;
				}
				if (ProductCodeTB.Text == "DDD" || ProductCodeTB.Text == "ddd")
				{
					List<Transaction> prevData = DG.ItemsSource as List<Transaction>;
					foreach (Transaction data in prevData)
					{
						DB.Conn.CreateTable<Transaction>();
						DB.Conn.Delete<Transaction>(data.ID);

						DB.Conn.CreateTable<ClientLedger>();
						var targetCl = DB.Conn.Table<ClientLedger>().ToList().Where(cl => cl.ClientCode == clientCode &&
																	cl.TransactionDate == transactionDate).FirstOrDefault();
						DB.Conn.Delete<ClientLedger>(targetCl);
					}

					DG.ItemsSource = null;
					ProductCodeTB.Text = "";
					return;
				}

				if (IsOnProductDBByCode(ProductCodeTB.Text))
				{
					ProductNameTB.Visibility = Visibility.Visible;

					// 수량
					ProductCountTB.Visibility = Visibility.Visible;
					ProductCountTB.Focus();
				}
				else // 제품을 전표에 입력했는데 제품이 없는 경우
				{
					var check = MessageBox.Show("제품이 데이터베이스에 없습니다. 등록하러 가시겠습니까?", "DB에 없음", MessageBoxButton.OKCancel, MessageBoxImage.Question);
					bubble = true;
					if (check == MessageBoxResult.Cancel)
					{
						return;
					}

					new ProductWindow().Show();
					return;
				}
			}
		}

		private void DepositTB_KeyUp(object sender, KeyEventArgs e)
		{
			int depositMoney;

			if (e.Key == Key.Escape && DepositTB.Text == "")
			{ 
				ProductCodeTB.Visibility = PrintOptionLB.Visibility = Visibility.Visible;
				DepositLB.Visibility = DepositTB.Visibility = Visibility.Hidden;
			}

			if (e.Key == Key.Escape && DepositTB.Text != "")
			{
				DepositTB.Text = "";
			}

			if (e.Key == Key.Enter && int.TryParse(DepositTB.Text, out depositMoney))
			{
				CurrentLeftMoneyTB.Text = String.Format("{0:#,0", (int.Parse(CurrentLeftMoneyTB.Text.Replace(",","")) - depositMoney));
				MonthDepositMoneyTB.Text = String.Format("{0:#,0", (int.Parse(MonthDepositMoneyTB.Text.Replace(",", "")) + depositMoney));
				DepositDB(depositMoney);
				ProductCodeTB.Visibility = PrintOptionLB.Visibility = Visibility.Visible;
				DepositLB.Visibility = DepositTB.Visibility = Visibility.Hidden;
				ProductCodeTB.Focus();
			}
		}

		private void DepositDB(int depositMoney)
		{
			try
			{
				DB.Conn.CreateTable<ClientLedger>();
				ClientLedger cl = DB.Conn.Table<ClientLedger>().Where(cl => cl.ClientCode == clientCode
																	&& cl.TransactionDate == transactionDate).FirstOrDefault();

				if (cl == null)
				{
					cl = new ClientLedger();
					cl.ClientCode = clientCode;
					cl.TransactionDate = transactionDate;
					cl.TodayDepositMoney = depositMoney;
					cl.CurrentLeftMoney -= depositMoney;
					DB.Conn.Insert(cl);
				}
				else
				{
					cl.TodayDepositMoney += depositMoney;
					cl.CurrentLeftMoney -= depositMoney;
					DB.Conn.Update(cl);
				}
			}
			catch (Exception)
			{
				MessageBox.Show("입금을 처리하는데 실패하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void DepositTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
		}

		private bool IsOnProductDBByCode(string productCode)
		{
			DB.Conn.CreateTable<Product>();
			productObject = DB.Conn.Find<Product>(p => p.ProductCode == productCode);

			if (productObject != null)
			{
				productName = ProductNameTB.Text = productObject.ProductName;
				return true;
			}
			else
				return false;
		}

		private void ProductCountTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (ProductCountTB.Text == "")
				{
					ProductCountTB.Visibility = Visibility.Hidden;
					ProductCodeTB.Text = "";
					ProductNameTB.Text = "";
					ProductCodeTB.Focus();
				}
				else
				{
					ProductCountTB.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (ProductCountTB.Text == "")
				{
					return;
				}
				else
				{
					productCount = int.Parse(ProductCountTB.Text);
				}

				DiscountPercentStackPanel.Visibility = Visibility.Visible;
				DiscountPercentTB.Focus();
			}
		}

		private void DiscountPercentTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (DiscountPercentTB.Text == "")
				{
					DiscountPercentStackPanel.Visibility= Visibility.Hidden;
					ProductCodeTB.Text = ProductCountTB.Text = DiscountPercentTB.Text = "";
					DiscountPercentStackPanel.Visibility = Visibility.Hidden;
					ProductCountTB.Visibility = Visibility.Hidden;
					ProductCountTB.Focus();
				}
				else
				{
					DiscountPercentTB.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (double.TryParse(DiscountPercentTB.Text, out productDiscountRate))
				{
					// Datagrid를 표시해야 함
					DG.Visibility = Visibility.Visible;
					UpdateDB();
					
					DiscountPercentStackPanel.Visibility = Visibility.Hidden;
					ProductCountTB.Visibility = Visibility.Hidden;
					PrintOptionLB.Visibility = Visibility.Visible;
					ProductCodeTB.Text = ProductNameTB.Text = ProductCountTB.Text = DiscountPercentTB.Text = "";
					ProductCodeTB.Focus();
				}
				else
				{
					return;
				}
			}
		}

		private void DiscountPercentTB_GotFocus(object sender, RoutedEventArgs e)
		{
			try
			{
				var query = DB.Conn.Query<Discount>(
					@"SELECT a.DiscountRate
					FROM Discount a, Product b
					WHERE a.BrandCode = b.BrandCode
					AND b.ProductCode = ?", productObject.ProductCode
					).ToArray();
				
				productDiscountRate = query[0].DiscountRate;
				DiscountPercentTB.Text = productDiscountRate.ToString("F2");
				DiscountPercentTB.Select(DiscountPercentTB.Text.Length, 0);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				// mes box 
			}
		}

		private void ProductCountTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspaceMinus(e.Text);
		}

		private void DiscountPercentTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspaceDot(e.Text);
		}

		private void UpdateDB()
		{
			try
			{
				// Transaction 테이블 수정
				DB.Conn.CreateTable<Transaction>();

				Transaction ts = new Transaction();
				ts.Choice = choice;
				ts.TransactionDate = transactionDate;
				ts.ClientCode = clientCode;
				ts.ProductCode = productObject.ProductCode;
				ts.ProductName = productObject.ProductName;
				ts.ProductCount = productCount;
				ts.Price = productObject.Price * productCount;
				ts.DiscountPrice = (int)((double)ts.Price * productDiscountRate / 100.00);
				switch (appendChoice)
				{
					case 0:
						ts.AppendOption0 = 1;
						break;
					case 1:
						ts.AppendOption1 = 1;
						break;
					case 2:
						ts.AppendOption2 = 1;
						break;
				}

				Transaction realTransaction = DB.Conn.Table<Transaction>().ToList().Where(t => t.Choice == choice &&
																t.TransactionDate == transactionDate &&
																t.ClientCode == clientCode &&
																t.ProductCode == productObject.ProductCode &&
																t.AppendOption0 == ts.AppendOption0 &&
																t.AppendOption1 == ts.AppendOption1 &&
																t.AppendOption2 == ts.AppendOption2).FirstOrDefault();

				if (realTransaction == null)
					DB.Conn.Insert(ts);
				else
				{
					// 이미 있는거에 추가된거를 업데이트
					realTransaction.DiscountPrice += ts.DiscountPrice;
					realTransaction.Price += ts.Price;
					realTransaction.ProductCount += ts.ProductCount;

					DB.Conn.Update(realTransaction);
				}

				// LeftMoneyLedger 테이블 수정
				DB.Conn.CreateTable<ClientLedger>();
				ClientLedger cl = DB.Conn.Table<ClientLedger>().Where(cl => cl.ClientCode == clientCode
																	&& cl.TransactionDate == transactionDate).FirstOrDefault();
				if (cl == null)
				{
					cl = new ClientLedger();
					cl.ClientCode = clientCode;
					cl.TransactionDate = transactionDate;
				}

				if (choice == 1) // 출고
				{
					cl.TodaySellMoney += ts.ProductCount * ts.DiscountPrice;
					cl.CurrentLeftMoney += ts.ProductCount * ts.DiscountPrice;

					MonthSellMoneyTB.Text = String.Format("{0:#,0}", (int.Parse(MonthSellMoneyTB.Text.Replace(",", "")) + ts.ProductCount * ts.DiscountPrice));
					CurrentLeftMoneyTB.Text = String.Format("{0:#,0}", cl.CurrentLeftMoney);

					var affected = DB.Conn.Update(cl);
					if (affected == 0)
						DB.Conn.Insert(cl);
				}
				if (choice == 2) // 덤
				{
					// nothing what i do
				}
				if (choice == 3) // 환입
				{
					cl.TodayRefundMoney += ts.ProductCount * ts.DiscountPrice;
					cl.CurrentLeftMoney -= ts.ProductCount * ts.DiscountPrice;

					CurrentLeftMoneyTB.Text = String.Format("{0:#,0}", cl.CurrentLeftMoney);
					MonthRefundMoneyTB.Text = String.Format("{0:#,0}", (int.Parse(MonthRefundMoneyTB.Text.Replace(",", "")) + ts.ProductCount * ts.DiscountPrice));

					var affected = DB.Conn.Update(cl);
					if (affected == 0)
						DB.Conn.Insert(cl);
				}

				// Datagrid에 표시
				DB.Conn.CreateTable<Transaction>();
				var list = DB.Conn.Table<Transaction>().ToList().Where(t => t.Choice == choice &&
																				t.ClientCode == clientCode &&
																				t.TransactionDate == transactionDate)
					.OrderBy(t => t.ProductCode);
				DG.ItemsSource = list;

				int totalCount = 0;
				int totalDiscountPrice = 0;
				int totalPrice = 0;
				foreach(var element in list)
				{
					totalCount += element.ProductCount;
					totalDiscountPrice += element.DiscountPrice;
					totalPrice += element.Price;
				}

				TotalCountTB.Text = String.Format("{0:#,0}", totalCount);
				TotalDiscountPriceTB.Text = String.Format("{0:#,0}", totalDiscountPrice);
				TotalPriceTB.Text = String.Format("{0:#,0}", totalPrice);

			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				MessageBox.Show("표에 데이터를 입력하는데 실패하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			return;
		}

	}
}
