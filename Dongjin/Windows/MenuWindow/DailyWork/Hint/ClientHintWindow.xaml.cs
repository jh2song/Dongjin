using Dongjin.Classes;
using Dongjin.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Dongjin.Windows.MenuWindow.DailyWork.Hint
{
	/// <summary>
	/// Interaction logic for ClientHintWindow.xaml
	/// </summary>
	public partial class ClientHintWindow : Window
	{
		private List<Client> _clients;

		public ClientHintWindow()
		{
			InitializeComponent();

			SearchTB.Focus();

			_clients = new List<Client>();

			ReadDatabase();
		}

		private void ReadDatabase()
		{
			DB.Conn.CreateTable<Client>();

			_clients = (DB.Conn.Table<Client>().ToList()).OrderBy(c => c.ClientCode).ToList();

			if (_clients != null)
			{
				clientsListView.ItemsSource = _clients;
			}
		}

		private void SearchTB_KeyDown(object sender, KeyEventArgs e)
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
				var filteredList = _clients.Where(c => c.ClientCode.ToString().StartsWith(SearchTB.Text));

				clientsListView.ItemsSource = filteredList;
			}
		}

		private void clientsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Client selectedClient = (Client)clientsListView.SelectedItem;

			if (selectedClient == null)
				return;

			Dongjin.Windows.MenuWindow.DailyWork.TransactionWindow.returnClientCode
				= selectedClient.ClientCode;
			Close();
		}

	}
}
