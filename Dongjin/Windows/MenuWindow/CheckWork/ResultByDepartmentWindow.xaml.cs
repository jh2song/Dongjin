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
	/// Interaction logic for ResultByDepartmentWindow.xaml
	/// </summary>
	public partial class ResultByDepartmentWindow : Window
	{
		private int _choice;

		public ResultByDepartmentWindow()
		{
			InitializeComponent();
		}

		private void ChoiceTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
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

		private void ChoiceTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && ChoiceTB.Text == "")
			{
				Close();
			}

			if (e.Key == Key.Escape && ChoiceTB.Text != "")
			{
				ChoiceTB.Text = "";
			}

			if (e.Key == Key.Enter && int.TryParse(ChoiceTB.Text, out _choice))
			{
				if (_choice == 1 || _choice == 2)
				{
					YearTB.Focus();
				}
			}
		}

		private void YearTB_KeyDown(object sender, KeyEventArgs e)
		{

		}

		private void MonthTB_KeyDown(object sender, KeyEventArgs e)
		{

		}

		private void DayTB_KeyDown(object sender, KeyEventArgs e)
		{

		}
	}
}
