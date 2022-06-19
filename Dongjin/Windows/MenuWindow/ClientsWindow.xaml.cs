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

namespace Dongjin.Windows.MenuWindow
{
	/// <summary>
	/// ClientsWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ClientsWindow : Window
	{
		private readonly List<TextBox> TopTBList = new List<TextBox>();
		private readonly List<TextBox> MiddleTBList = new List<TextBox>();
		private int focusIdx;

		public ClientsWindow()
		{
			InitializeComponent();

			SetList();
			
			focusIdx = 0;
			TopTBList[focusIdx].Focus();
		}

		private void SetList()
		{
			TopTBList.Add(tb1);
			TopTBList.Add(tb2);
			TopTBList.Add(tb3);
			TopTBList.Add(tb4);

			MiddleTBList.Add(tbDetail1);
			MiddleTBList.Add(tbDetail2);
			MiddleTBList.Add(tbDetail3);
			MiddleTBList.Add(tbDetail4);
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Close();
			}
			else if (e.Key == Key.Enter)
			{
				if (TopTBList[focusIdx] == tb1)
				{
					TopTBList[focusIdx].Text = DateTime.Today.Year.ToString().Substring(2, 2);
					focusIdx++;
					TopTBList[focusIdx].Focus();
				}
				else if (TopTBList[focusIdx] == tb2)
				{
					TopTBList[focusIdx].Text = DateTime.Now.Month.ToString("00");
					focusIdx++;
					TopTBList[focusIdx].Focus();
				}
				else if (TopTBList[focusIdx] == tb3)
				{
					TopTBList[focusIdx].Text = DateTime.Now.Day.ToString("00");
					focusIdx++;
					TopTBList[focusIdx].Focus();
				}
			}
		}

		private void TB1_KeyUp(object sender, KeyEventArgs e)
		{
			if (tb1.Text.Length >= 2)
				tb2.Focus();
		}

		private void TB2_KeyUp(object sender, KeyEventArgs e)
		{
			if (tb2.Text.Length >= 2)
				tb3.Focus();
		}

		private void TB3_KeyUp(object sender, KeyEventArgs e)
		{
			if (tb3.Text.Length >= 2)
				tb4.Focus();
		}

		private void TB2_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Back && tb2.Text == "")
				tb1.Focus();
		}

		private void TB3_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Back && tb3.Text == "")
				tb2.Focus();
		}

		private void TB4_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Back && tb4.Text == "")
				tb3.Focus();
		}
	}
}
