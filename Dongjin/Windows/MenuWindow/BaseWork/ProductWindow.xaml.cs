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

namespace Dongjin.Windows.MenuWindow.BaseWork
{
	/// <summary>
	/// ProductWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ProductWindow : Window
	{
		public ProductWindow()
		{
			InitializeComponent();

			tb1.Text = DateTime.Now.Year.ToString().Substring(2, 2);
			tb2.Text = DateTime.Now.Month.ToString("00");
			tb3.Text = DateTime.Now.Day.ToString("00");

			tb4.Focus();
		}

		private void TB4_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (tb4.Text == "")
					Close();
				else
					tb4.Text = "";
			}

			if (e.Key == Key.Enter)
			{
				string ret = CompanyWindow.GetNameByCode(tb4.Text);
				if (ret == "")
				{
					tb4.Text = "";
				}
				else
				{
					tb5.Text = ret;
					tb6.Focus();
				}
			}
		}

		private void TB6_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (tb6.Text == "")
				{
					tb5.Text = "";
					tb4.Focus();
				}
				else
					tb6.Text = "";
			}

			if (e.Key == Key.Enter)
			{
				
			}
		}
	}
}
