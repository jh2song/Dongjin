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
		private readonly List<TextBox> tbList = new List<TextBox>();
		private int focusIdx;

		public ClientsWindow()
		{
			InitializeComponent();

			tbList.Add(tb1);
			tbList.Add(tb2);
			tbList.Add(tb3);
			tbList.Add(tb4);

			focusIdx = 0;
			tbList[focusIdx].Focus();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				Close();
			}
			else if (e.Key == Key.Enter)
			{
				if (tbList[focusIdx] == tb1)
				{
					tbList[focusIdx].Text = DateTime.Today.Year.ToString().Substring(2, 2);
					focusIdx++;
					tbList[focusIdx].Focus();
				}
				else if (tbList[focusIdx] == tb2)
				{
					tbList[focusIdx].Text = DateTime.Now.Month.ToString("00");
					focusIdx++;
					tbList[focusIdx].Focus();
				}
				else if (tbList[focusIdx] == tb3)
				{
					tbList[focusIdx].Text = DateTime.Now.Day.ToString("00");
					focusIdx++;
					tbList[focusIdx].Focus();
				}
			}
		}
	}
}
