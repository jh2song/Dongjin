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

		private void YearTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && YearTB.Text == "")
			{
				Close();
			}
			if (e.Key == Key.Escape && YearTB.Text != "")
			{
				YearTB.Text = "";
			}

			if (e.Key == Key.Enter && YearTB.Text.Length < 2)
			{
				YearTB.Text = DateTime.Now.Year.ToString().Substring(2, 2);
				MonthTB.Focus();
			}
			if (e.Key == Key.Enter && YearTB.Text.Length == 2)
			{
				MonthTB.Focus();
			}
		}

		private void MonthTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && MonthTB.Text == "")
			{
				YearTB.Text = "";
				YearTB.Focus();
			}
			if (e.Key == Key.Escape && MonthTB.Text != "")
			{
				MonthTB.Text = "";
			}

			if (e.Key == Key.Enter && MonthTB.Text.Length < 2)
			{
				MonthTB.Text = DateTime.Now.Month.ToString("00");
				DayTB.Focus();
			}
			if (e.Key == Key.Enter && MonthTB.Text.Length == 2)
			{
				DayTB.Focus();
			}
		}

		private void DayTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && DayTB.Text == "")
			{
				MonthTB.Text = "";
				MonthTB.Focus();
			}
			if (e.Key == Key.Escape && DayTB.Text != "")
			{
				DayTB.Text = "";
			}

			if (e.Key == Key.Enter && DayTB.Text.Length < 2)
			{
				DayTB.Text = DateTime.Now.Day.ToString("00");
				ClientCodeTB.Focus();
			}
			if (e.Key == Key.Enter && DayTB.Text.Length == 2)
			{
				ClientCodeTB.Focus();
			}
		}

		private void ClientCodeTB_KeyDown(object sender, KeyEventArgs e)
		{

		}
	}
}
