using Dongjin.Classes;
using Dongjin.Table;
using Dongjin.Windows.MenuWindow.BaseWork;
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

				// false: 거래처코드가 거래처 DB에 등록되어있지 않음
				// true: 거래처코드가 거래처 DB에 등록되어있음
				if (!ShowNameByCode(ClientCodeTB.Text))
					return;

				// do - something
				PrevMonthLeftMoneyTB.Text = GetPrevMonthLeftMoney();
				MonthSellMoneyTB.Text = foundClient.MonthSellMoney.ToString();
				MonthDepositMoneyTB.Text = foundClient.MonthDepositMoney.ToString();
				MonthReturnMoneyTB.Text = foundClient.MonthRetundMoney.ToString();
				LastTransactionDateTB.Text = foundClient.FinalTransactionDate.Year.ToString("0000").Substring(2, 2) + "/" 
					+ foundClient.FinalTransactionDate.Month.ToString("00") + "/" 
					+ foundClient.FinalTransactionDate.Day.ToString("00");

				currentLeftMoney = int.Parse(PrevMonthLeftMoneyTB.Text)
					+ int.Parse(MonthSellMoneyTB.Text)
					- int.Parse(MonthDepositMoneyTB.Text)
					- int.Parse(MonthReturnMoneyTB.Text);
				CurrentLeftMoneyTB.Text = (currentLeftMoney).ToString();

				clientCode = int.Parse(ClientCodeTB.Text);
				if (IsOnDBByChoiceDateCode()) // 전표가 이미 남아있는 상황
				{
					// Datagrid를 표시해야함
					try
					{
						DB.Conn.CreateTable<Transaction>();
						DG.ItemsSource = null;
						DG.ItemsSource = DB.Conn.Table<Transaction>().Where(t => t.Choice == choice &&
																			t.TransactionDate == transactionDate &&
																			t.ClientCode == clientCode).ToList();
						DG.Visibility = Visibility.Visible;
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
				DB.Conn.CreateTable<Transaction>();

				int inputClientCode = int.Parse(ClientCodeTB.Text);

				DateTime today = transactionDate;
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
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
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
					DiscountPercentTB.Visibility = Visibility.Hidden;
					DiscountPercentLB.Visibility = Visibility.Hidden;
					ProductCountTB.Text = "";
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
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
		}

		private void DiscountPercentTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspaceDot(e.Text);
		}

		private void UpdateDB()
		{
			try
			{
				Transaction transaction = new Transaction();
				transaction.Choice = choice;
				transaction.TransactionDate = transactionDate;
				transaction.ClientCode = clientCode;
				transaction.ProductCode = productObject.ProductCode;
				transaction.ProductName = productObject.ProductName;
				transaction.ProductCount = productCount;
				transaction.Price = productObject.Price;
				transaction.DiscountedPrice = (int)((double)transaction.Price * productDiscountRate / 100.00);
				switch (appendChoice)
				{
					case 0:
						transaction.AppendOption0 = 1;
						break;
					case 1:
						transaction.AppendOption1 = 1;
						break;
					case 2:
						transaction.AppendOption2 = 1;
						break;
				}
				transaction.CurrentLeftMoney = currentLeftMoney + transaction.DiscountedPrice;

				DB.Conn.CreateTable<Transaction>();
				var isThere = DB.Conn.Table<Transaction>().Where(t => t.Choice == transaction.Choice &&
												t.TransactionDate == transaction.TransactionDate &&
												t.ClientCode == transaction.ClientCode &&
												t.ProductCode == transaction.ProductCode &&
												t.DiscountedPrice == transaction.DiscountedPrice &&
												t.AppendOption0 == transaction.AppendOption0 &&
												t.AppendOption1 == transaction.AppendOption1 &&
												t.AppendOption2 == transaction.AppendOption2).FirstOrDefault();

				if (isThere != null) // 이미 똑같은 경우의 제품이 Datagrid와 DB에 올라와 있음
				{
					isThere.ProductCount += transaction.ProductCount;
					DB.Conn.Update(isThere);
				}
				else
				{
					DB.Conn.Insert(transaction);
				}

				DG.ItemsSource = null;
				DG.ItemsSource = DB.Conn.Table<Transaction>().Where(t => t.Choice == choice &&
																			t.TransactionDate == transactionDate &&
																			t.ClientCode == clientCode).ToList();
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
