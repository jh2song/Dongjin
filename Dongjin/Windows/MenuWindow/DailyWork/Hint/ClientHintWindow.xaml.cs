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

namespace Dongjin.Windows.MenuWindow.DailyWork.Hint
{
	/// <summary>
	/// Interaction logic for ClientHintWindow.xaml
	/// </summary>
	public partial class ClientHintWindow : Window
	{
		private List<Client> _clientsListView;

		public ClientHintWindow()
		{
			InitializeComponent();

			_clientsListView = new List<Client>();
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void clientsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}
}
