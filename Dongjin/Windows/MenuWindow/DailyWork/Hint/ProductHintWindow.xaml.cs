﻿using Dongjin.Classes;
using Dongjin.Table;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Dongjin.Windows.MenuWindow.DailyWork.Hint
{
	/// <summary>
	/// Interaction logic for ProductHintWindow.xaml
	/// </summary>
	public partial class ProductHintWindow : Window
	{
		private List<Product> _products = new List<Product>();

		public ProductHintWindow()
		{
			InitializeComponent();

			SearchTB.Focus();

			ReadDatabase();
		}

		private void ReadDatabase()
		{
			DB.Conn.CreateTable<Product>();

			_products = DB.Conn.Table<Product>().OrderBy(c => c.ProductCode).ToList();

			if (_products != null)
			{
				ProductsListView.ItemsSource = _products;
			}
		}

		private void SearchTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && SearchTB.Text != "")
			{
				SearchTB.Text = "";
			}
			else if (e.Key == Key.Escape && SearchTB.Text == "")
			{
				Close();
			}
			else
			{
				var filteredList = _products.Where(c => c.ProductCode.StartsWith(SearchTB.Text)
				|| c.ProductName.StartsWith(SearchTB.Text)
				);

				ProductsListView.ItemsSource = filteredList;
			}
		}

		private void ProductsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Product selectedProduct = (Product)ProductsListView.SelectedItem;

			if (selectedProduct == null)
				return;

			TransactionWindow.returnProductCode
				= selectedProduct.ProductCode;
			Close();
		}
	}
}
