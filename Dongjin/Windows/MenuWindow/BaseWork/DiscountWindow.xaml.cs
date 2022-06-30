using Dongjin.Classes;
using Dongjin.Table;
using System;
using System.Collections.Generic;
using System.Text;
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
	/// DiscountWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class DiscountWindow : Window
	{
		List<Discount> _discounts;

		public DiscountWindow()
		{
			InitializeComponent();

			CodeTB.Focus();
		}

		private void CodeTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (CodeTB.Text == "")
					Close();
				else
					CodeTB.Text = "";
			}

			if (e.Key == Key.Enter)
			{
				ShowTable();
			}
		}

		private void ShowTable()
		{
			DB.Conn.CreateTable<Discount>();

			string codeText = CodeTB.Text;
			_discounts =
				DB.Conn.Table<Discount>().Where(d => d.DiscountCode.Equals(codeText)).ToList();

			if (_discounts.Count == 0)
			{
				DiscountNameTB.Focusable = true;
				DiscountNameTB.Focus();
				return;
			}


			DiscountNameTB.Text = _discounts[0].DiscountName;

			DG.ItemsSource = _discounts;
		}

		private void DiscountNameTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (DiscountNameTB.Text == "")
				{
					DiscountNameTB.Focusable = false;
					CodeTB.Focus();
				}
				else
					DiscountNameTB.Text = "";
			}

			if (e.Key == Key.Enter && DiscountNameTB.Text != "")
			{
				DiscountNameTB.Focusable = false;
				SaveNewDiscount();
			}
		}

		private void SaveNewDiscount()
		{
			try
			{
				DB.Conn.CreateTable<Brand>();

				List<Brand> brands = DB.Conn.Table<Brand>().ToList();

				foreach (Brand brand in brands)
				{
					Discount discount = new Discount();

					discount.DiscountCode = int.Parse(CodeTB.Text);
					discount.DiscountName = DiscountNameTB.Text;
					discount.BrandCode = brand.BrandCode;
					discount.BrandName = brand.BrandName;

					_discounts.Add(discount);
					DG.Items.Add(discount);
				}
			}
			catch (Exception)
			{
				MessageBox.Show("입력이 잘못되었습니다", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
