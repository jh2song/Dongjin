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

namespace Dongjin.Windows.MenuWindow.BaseWork
{
	/// <summary>
	/// ProductWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ProductWindow : Window
	{
		private bool isUpdateing = false;

		public ProductWindow()
		{
			InitializeComponent();

			SetDate();

			tb4.Focus();
		}

		private void SetDate()
		{
			tb1.Text = DateTime.Now.Year.ToString().Substring(2, 2);
			tb2.Text = DateTime.Now.Month.ToString("00");
			tb3.Text = DateTime.Now.Day.ToString("00");
		}

		private void tb4_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
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
				string ret = BrandWindow.GetNameByCode(tb4.Text);
				if (ret == "")
				{
					tb4.Text = "";
				}
				else
				{
					tb5.Text = ret;
					tb6.Focus();
				}
			}
		}

		private void TB6_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (tb6.Text == "")
				{
					tb5.Text = "";
					tb4.Focus();
					tb4.Select(tb4.Text.Length, 0);
				}
				else
					tb6.Text = "";
			}

			if (e.Key == Key.Enter && tb6.Text != "")
			{
				if (IsOnTheProduct(tb6.Text))
				{
					ShowDetail(tb6.Text);
					SPOnTheProduct.Visibility = Visibility.Visible;
					isUpdateing = false;
					TBCmdOn.Focus();
				}
				else 
				{
					tbDetail1.Focus();
				}
			}
		}

		private void ShowDetail(string code)
		{
			try
			{
				DB.Conn.CreateTable<Product>();

				List<Product> products = DB.Conn.Table<Product>().Where(c => c.ProductCode.Equals(code)).ToList();

				tbDetail1.Text = products[0].Name;
				tbDetail2.Text = String.Format("{0:#,0}", products[0].Price);
				tbDetail3.Text = products[0].LeftAmount.ToString();
				tbDetail4.Text = String.Format("{0:#,0}", products[0].BuyingPrice);
				tbDetail5.Text = String.Format("{0:#,0}", products[0].TotalDepositMoney);
			}
			catch (Exception ex)
			{
				MessageBox.Show("데이터베이스에서 불러오는데 실패하였습니다", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
				Debug.WriteLine(ex);
			}
		}

		private bool IsOnTheProduct(string code)
		{
			DB.Conn.CreateTable<Product>();

			List<Product> products = DB.Conn.Table<Product>().Where(c => c.ProductCode.Equals(code)).ToList();

			if (products.Count == 0)
				return false;
			else
				return true;
		}

		private void TBDetail1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (tbDetail1.Text == "")
				{
					tb6.Focus();
					tb6.Select(tb6.Text.Length, 0);
				}
				else
				{
					tbDetail1.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (!isUpdateing)
					tbDetail2.Focus();
				else if (SPOnTheProduct.Visibility == Visibility.Visible)
					TBCmdOn.Focus();
				else
					TBCmdOff.Focus();
			}
		}

		private void TBDetail2_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (tbDetail2.Text == "")
				{
					tbDetail1.Focus();
					tbDetail1.Select(tbDetail1.Text.Length, 0);
				}
				else
				{
					tbDetail2.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (tbDetail2.Text == "")
					tbDetail2.Text = "0";
				if (!isUpdateing)
					tbDetail3.Focus();
				else if (SPOnTheProduct.Visibility == Visibility.Visible)
					TBCmdOn.Focus();
				else
					TBCmdOff.Focus();

			}
		}

		private void TBDetail3_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (tbDetail3.Text == "")
				{
					tbDetail2.Focus();
					tbDetail2.Select(tbDetail2.Text.Length, 0);
				}
				else
				{
					tbDetail3.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (tbDetail3.Text == "")
					tbDetail3.Text = "0";

				if (!isUpdateing)
				{
					tbDetail4.Text = CalculateBuyingPrice(tb4.Text, tbDetail2.Text);
					tbDetail4.Select(tbDetail4.Text.Length, 0);
					tbDetail4.Focus();
				}
				else if (SPOnTheProduct.Visibility == Visibility.Visible)
				{
					tbDetail5.Text = (int.Parse(tbDetail3.Text) * int.Parse(tbDetail4.Text)).ToString();
					TBCmdOn.Focus();
				}
				else
				{
					tbDetail5.Text = (int.Parse(tbDetail3.Text) * int.Parse(tbDetail4.Text)).ToString();
					TBCmdOff.Focus();
				}
			}
		}

		private string CalculateBuyingPrice(string brandCodeStr, string priceStr)
		{
			int brandCode = int.Parse(brandCodeStr);
			int ret = int.Parse(priceStr);

			try
			{
				DB.Conn.CreateTable<Brand>();
				var list = DB.Conn.Table<Brand>().Where(b => b.BrandCode.Equals(brandCode)).ToList();

				ret = (int)((double)ret * (list[0].BuyingPercent / 100.0));
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				MessageBox.Show("사입단가를 자동으로 계산하는데 실패하였습니다.", "브랜드 DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			return ret.ToString();
		}

		private void TBDetail4_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (tbDetail4.Text == "")
				{
					tbDetail3.Focus();
					tbDetail3.Select(tbDetail3.Text.Length, 0);
				}
				else
				{
					tbDetail4.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (tbDetail4.Text == "")
					tbDetail4.Text = "0";
				tbDetail5.Text = (int.Parse(tbDetail3.Text) * int.Parse(tbDetail4.Text)).ToString();

				if (!isUpdateing)
				{
					SPOffTheProduct.Visibility = Visibility.Visible;
					TBCmdOff.Focus();
				}
				else if (SPOnTheProduct.Visibility == Visibility.Visible)
					TBCmdOn.Focus();
				else
					TBCmdOff.Focus();

			}
		}

		private void TBCmdOn_KeyDown(object sender, KeyEventArgs e)
		{
			int cmd;

			if (e.Key == Key.Enter)
			{
				if (int.TryParse(TBCmdOn.Text, out cmd) == true) // 항목 수정
				{
					TBCmdOn.Text = "";
					switch (cmd)
					{
						case 1:
							isUpdateing = true;
							tbDetail1.Select(tbDetail1.Text.Length, 0);
							tbDetail1.Focus();
							break;
						case 2:
							isUpdateing = true;
							tbDetail2.Select(tbDetail2.Text.Length, 0);
							tbDetail2.Focus();
							break;
						case 3:
							isUpdateing = true;
							tbDetail3.Select(tbDetail3.Text.Length, 0);
							tbDetail3.Focus();
							break;
						case 4:
							isUpdateing = true;
							tbDetail4.Select(tbDetail4.Text.Length, 0);
							tbDetail4.Focus();
							break;
					}
				}
				else if (TBCmdOn.Text == "D" || TBCmdOn.Text == "d") // 삭제
				{
					DeleteProductDB(tb6.Text);
					TBCmdOn.Text = "";
					EraseDetail();
					SPOnTheProduct.Visibility = Visibility.Hidden;
					isUpdateing = false;
					tb6.Focus();
				}
				else // 계속
				{
					SaveProductDB();
					EraseDetail();
					TBCmdOn.Text = "";
					SPOnTheProduct.Visibility = Visibility.Hidden;
					isUpdateing = false;
					tb6.Focus();
				}
			}

			if (e.Key == Key.Escape)
			{
				if (TBCmdOn.Text != "")
					TBCmdOn.Text = "";
				else // 무시(ESC)
				{
					EraseDetail();
					SPOnTheProduct.Visibility = Visibility.Hidden;
					isUpdateing = false;
					tb6.Focus();
				}
			}

			if (e.Key == Key.E)
				Close();

		}

		private void SaveProductDB()
		{
			try
			{
				Product product = new Product();
				int target;

				product.ProductCode = tb6.Text;
				if (int.TryParse(tb4.Text, out target))
				{
					product.BrandCode = target;
				}
				product.Name = tbDetail1.Text;
				if (int.TryParse(tbDetail2.Text.Replace(",", ""), out target))
				{
					product.Price = target;
				}
				if (int.TryParse(tbDetail3.Text, out target))
				{
					product.LeftAmount = target;
				}
				if (int.TryParse(tbDetail4.Text.Replace(",", ""), out target))
				{
					product.BuyingPrice = target;
				}
				if (int.TryParse(tbDetail5.Text.Replace(",", ""), out target))
				{
					product.TotalDepositMoney = target;
				}

				DB.Conn.CreateTable<Product>();
				var targetProduct = DB.Conn.Find<Product>(product.ProductCode);
				if (targetProduct == null)
					DB.Conn.Insert(product);
				else
					DB.Conn.Update(product);
			}
			catch (Exception ex)
			{
				MessageBox.Show("데이터베이스 저장에 오류가 발생하였습니다", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
				Debug.WriteLine(ex.ToString());
			}
		}

		private void EraseDetail()
		{
			tbDetail1.Text = tbDetail2.Text = tbDetail3.Text = tbDetail4.Text =
				tbDetail5.Text = "";
		}

		private void DeleteProductDB(string text)
		{
			try
			{
				DB.Conn.CreateTable<Product>();
				DB.Conn.Execute($"DELETE FROM Product WHERE ProductCode = '{tb6.Text}'");
			}
			catch (Exception ex)
			{
				MessageBox.Show("데이터베이스 삭제에 오류가 발생하였습니다", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
				Debug.WriteLine(ex.ToString());
			}
		}

		private void TBCmdOff_KeyDown(object sender, KeyEventArgs e)
		{
			int cmd;

			if (e.Key == Key.Enter)
			{
				if (int.TryParse(TBCmdOff.Text, out cmd) == true) // 항목 수정
				{
					TBCmdOff.Text = "";
					switch (cmd)
					{
						case 1:
							isUpdateing = true;
							tbDetail1.Select(tbDetail1.Text.Length, 0);
							tbDetail1.Focus();
							break;
						case 2:
							isUpdateing = true;
							tbDetail2.Select(tbDetail2.Text.Length, 0);
							tbDetail2.Focus();
							break;
						case 3:
							isUpdateing = true;
							tbDetail3.Select(tbDetail3.Text.Length, 0);
							tbDetail3.Focus();
							break;
						case 4:
							isUpdateing = true;
							tbDetail4.Select(tbDetail4.Text.Length, 0);
							tbDetail4.Focus();
							break;
					}
				}
				else // 계속
				{
					SaveProductDB();
					EraseDetail();
					TBCmdOff.Text = "";
					SPOffTheProduct.Visibility = Visibility.Hidden;
					isUpdateing = false;
					tb6.Focus();
				}
			}

			if (e.Key == Key.Escape)
			{
				if (TBCmdOff.Text != "")
					TBCmdOff.Text = "";
				else // 무시(ESC)
				{
					EraseDetail();
					SPOffTheProduct.Visibility = Visibility.Hidden;
					isUpdateing = false;
					tb6.Focus();
				}
			}

			if (e.Key == Key.E)
				Close();
		}

	}
}
