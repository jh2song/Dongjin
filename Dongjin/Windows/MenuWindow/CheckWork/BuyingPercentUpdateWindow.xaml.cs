using Dongjin.Classes;
using Dongjin.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dongjin.Windows.MenuWindow.CheckWork
{
	/// <summary>
	/// Interaction logic for BuyingPercentUpdateWindow.xaml
	/// </summary>
	public partial class BuyingPercentUpdateWindow : Window
	{
		private List<Brand> brands;

		public BuyingPercentUpdateWindow()
		{
			InitializeComponent();

			ShowTable();
		}

		private void ShowTable()
		{
			try
			{
				DB.Conn.CreateTable<Brand>();

				brands = DB.Conn.Table<Brand>().ToList();
				brands.Sort();

				DG.ItemsSource = brands;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				MessageBox.Show("표를 불러오는데 실패했습니다.", "표 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
				Close();
		}

		private void DG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
			try
			{
				int y = e.Row.GetIndex();
				int x = e.Column.DisplayIndex;

				Brand updateBrand = brands[y];
				switch (x)
				{
					case 0:
						updateBrand.BrandCode = int.Parse((e.EditingElement as TextBox).Text);
						break;
					case 1:
						updateBrand.BrandName = (e.EditingElement as TextBox).Text;
						break;
					case 2:
						updateBrand.BuyingPercent = double.Parse((e.EditingElement as TextBox).Text);
						break;
				}

				DB.Conn.Update(updateBrand);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				MessageBox.Show("표에 문제가 발생하였습니다.", "표 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
