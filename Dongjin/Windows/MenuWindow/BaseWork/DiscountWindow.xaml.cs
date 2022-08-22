using Dongjin.Classes;
using Dongjin.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Dongjin.Windows.MenuWindow.BaseWork
{
	/// <summary>
	/// DiscountWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class DiscountWindow : Window
	{
		List<Discount> _discounts;
		private bool _noInDB;

		public DiscountWindow()
		{
			InitializeComponent();

			CodeTB.Focus();
		}

		private void CodeTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
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

			_discounts.Sort();

			if (_discounts.Count == 0)
			{
				_noInDB = true;
				DiscountNameTB.Focusable = true;
				DiscountNameTB.Focus();
				return;
			}

			_noInDB = false;
			DiscountNameTB.Text = _discounts[0].DiscountName;
			DiscountNameTB.Select(DiscountNameTB.Text.Length, 0);

			DiscountNameTB.Focusable = true;
			DiscountNameTB.Focus();
			DG.ItemsSource = _discounts;
		}

		private void DiscountNameTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				DG.ItemsSource = null;
				if (DiscountNameTB.Text == "")
				{
					CodeTB.Focus();
					DiscountNameTB.Focusable = false;
				}
				else
					DiscountNameTB.Text = "";
			}

			if (e.Key == Key.Enter && DiscountNameTB.Text != "")
			{
				if (_noInDB)
					SaveNewDiscount();
				else
					UpdateDiscountName();
			}
		}

		private void UpdateDiscountName()
		{
			try
			{
				DB.Conn.CreateTable<Discount>();
				DB.Conn.Execute($"UPDATE Discount SET DiscountName = '{DiscountNameTB.Text}' WHERE DiscountCode = {int.Parse(CodeTB.Text)};");
				MessageBox.Show("할인율 이름이 변경되었습니다.", "할인율 변경", MessageBoxButton.OK);
				Bubble.bubble = true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				// 메시지 박스
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
				}
				_discounts.Sort();
				DB.Conn.CreateTable<Discount>();
				DB.Conn.InsertAll(_discounts);

				DG.ItemsSource = _discounts;
				_noInDB = false;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				MessageBox.Show("입력이 잘못되었습니다", "입력 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void DG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
			try
			{
				int y = e.Row.GetIndex();
				int x = e.Column.DisplayIndex;

				Discount updatedDiscount = _discounts[y];
				switch (x)
				{
					case 0:
						updatedDiscount.DiscountCode = int.Parse((e.EditingElement as TextBox).Text);
						break;
					case 1:
						updatedDiscount.DiscountName = (e.EditingElement as TextBox).Text;
						break;
					case 2:
						updatedDiscount.DiscountRate = double.Parse((e.EditingElement as TextBox).Text);
						break;
				}

				DB.Conn.Update(updatedDiscount);
				DiscountNameTB.Focus();
				DiscountNameTB.Select(DiscountNameTB.Text.Length, 0);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				// 메시지박스 출력
				MessageBox.Show("표에 문제가 발생하였습니다.", "표 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			finally
			{
				Bubble.bubble = true;
			}
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key != Key.F5)
				return;

			try
			{
				DB.Conn.Execute($"DELETE FROM Discount WHERE DiscountCode = '{CodeTB.Text}'");
				DG.ItemsSource = null;
				CodeTB.Text = DiscountNameTB.Text = "";
				CodeTB.Focus();
			}
			catch (Exception)
			{
				MessageBox.Show("삭제가 실패하였습니다.", "삭제 DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// block bubbling
		private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (Bubble.bubble)
			{
				e.Handled = true;
				Bubble.bubble = false;
				return;
			}
		}
	}
}
