using Dongjin.Classes;
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

namespace Dongjin.Windows.MenuWindow.CheckWork
{
	/// <summary>
	/// Interaction logic for ClientLedgerWindow.xaml
	/// </summary>
	public partial class ClientLedgerWindow : Window
	{
		public ClientLedgerWindow()
		{
			InitializeComponent();

			YearTB.Focus();
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

		private void ClientCodeTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
		}
	}
}
