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

namespace Dongjin.Windows.MenuWindow.DailyWork
{
	/// <summary>
	/// Interaction logic for TransactionWindow.xaml
	/// </summary>
	public partial class TransactionWindow : Window
	{
		public TransactionWindow()
		{
			InitializeComponent();

			ChoiceTB.Focus();
		}

		private void ChoiceTB_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (ChoiceTB.Text == "")
				{
					Close();
				}
				else
				{
					ChoiceTB.Text = "";
				}
			}

		}
	}
}
