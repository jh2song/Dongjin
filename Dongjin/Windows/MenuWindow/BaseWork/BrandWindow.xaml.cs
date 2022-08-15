using Dongjin.Classes;
using Dongjin.Table;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RegexClass = Dongjin.Classes.RegexClass;

namespace Dongjin.Windows.MenuWindow.BaseWork
{
	/// <summary>
	/// Interaction logic for CompanyWindow.xaml
	/// </summary>
	public partial class BrandWindow : Window
	{
		private SQLiteConnection _conn;
		public BrandWindow()
		{
			InitializeComponent();

			_conn = DB.Conn;
			TB1.Focus();
		}

		private void TB1_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
		}

		private void TB1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (TB1.Text.Length == 0)
					Close();
				else
					TB1.Text = "";
			}

			if (e.Key == Key.Enter)
			{
				TB2.Text = GetNameByCode(TB1.Text);

				if (TB2.Text == "")
				{
					TB2.Focus();
				}
				else
				{
					TBCmd.Focus();
				}
			}
		}

		private void TB2_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (TB2.Text == "")
				{
					TB1.Focus();
				}
				else
				{
					TB2.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				TBCmd.Focus();
			}
		}

		private void TBCmd_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (TBCmd.Text == "")
				{
					TB2.Focus();
					TB2.Select(TB2.Text.Length, 0);
				}
				else
				{
					TBCmd.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (TBCmd.Text == "D" || TBCmd.Text == "d")
				{
					DeleteDB();
					TB1.Text = TB2.Text = TBCmd.Text = "";
					TB1.Focus();
				}
				else
				{
					SaveDB();
					TB1.Text = TB2.Text = TBCmd.Text = "";
					TB1.Focus();
				}
			}
		}

		public static string GetNameByCode(string code)
		{
			DB.Conn.CreateTable<Brand>();

			List<Brand> company =
				DB.Conn.Table<Brand>().Where(c => c.BrandCode.Equals(code)).ToList();

			if (company.Count > 0)
			{ 
				return company[0].BrandName;
			}
			
			return "";
		}

		private void DeleteDB()
		{
			try
			{
				int parsedCode;
				if (int.TryParse(TB1.Text, out parsedCode))
				{
					////// 할인율에서 이 브랜드 쓰는거 삭제 ///////
					_conn.CreateTable<Discount>();
					_conn.Execute($"DELETE FROM Discount WHERE BrandCode = {parsedCode};");
					////////////////////////////////////////////

					_conn.CreateTable<Brand>();
					_conn.Execute($"DELETE FROM Brand WHERE BrandCode = {parsedCode};");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				MessageBox.Show("데이터베이스 삭제에 오류가 발생하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void SaveDB()
		{
			try
			{
				Brand brand = new Brand();
				brand.BrandCode = int.Parse(TB1.Text);
				brand.BrandName = TB2.Text;

				var targetBrand = _conn.Find<Brand>(brand.BrandCode);
				if (targetBrand == null)
				{
					InsertAllDiscount(brand);
					_conn.Insert(brand);
				}
				else
				{
					UpdateAllDiscount(brand);
					_conn.Update(brand);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				MessageBox.Show("데이터베이스 저장에 오류가 발생하였습니다", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void InsertAllDiscount(Brand brand)
		{
			try
			{
				_conn.CreateTable<Discount>();
				var discounts = _conn.Table<Discount>().ToList();

				var discountsGrouped = (from d in discounts
										group d by new { d.DiscountCode, d.DiscountName }
									   into grp
										select new
										{
											grp.Key.DiscountCode,
											grp.Key.DiscountName
										}).ToList();

				foreach (var discount in discountsGrouped)
				{
					Discount inserted = new Discount();
					inserted.DiscountCode = discount.DiscountCode;
					inserted.DiscountName = discount.DiscountName;
					inserted.BrandCode = brand.BrandCode;
					inserted.BrandName = brand.BrandName;
					_conn.Insert(inserted);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				MessageBox.Show("입력이 잘못되었습니다.", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void UpdateAllDiscount(Brand brand)
		{
			try
			{
				_conn.CreateTable<Discount>();
				var discounts = _conn.Table<Discount>().Where(d => d.BrandCode.Equals(brand.BrandCode)).ToList();

				foreach (var dc in discounts)
				{
					Discount discount = new Discount();
					discount.ID = dc.ID;
					discount.DiscountCode = dc.DiscountCode;
					discount.DiscountName = dc.DiscountName;
					discount.BrandCode = dc.BrandCode;
					discount.BrandName = brand.BrandName;
					discount.DiscountRate = dc.DiscountRate;
					_conn.Update(discount);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				MessageBox.Show("수정이 잘못되었습니다.", "수정 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

	}
}
