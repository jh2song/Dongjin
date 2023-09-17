using Dongjin.Classes;
using Dongjin.Table;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
				ClientsListView.ItemsSource = _clients;
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
				var filteredList = _clients.Where(c => c.ClientCode.ToString().StartsWith(SearchTB.Text)
				|| c.ClientName.Contains(SearchTB.Text)
				);

				ClientsListView.ItemsSource = filteredList;
			}
		}

		private void ClientsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Client selectedClient = (Client)ClientsListView.SelectedItem;

			if (selectedClient == null)
				return;

			Dongjin.Windows.MenuWindow.DailyWork.TransactionWindow.returnClientCode
				= selectedClient.ClientCode;
			Close();
		}

	}
}
