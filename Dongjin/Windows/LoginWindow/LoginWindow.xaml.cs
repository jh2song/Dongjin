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

namespace Dongjin.Windows.LoginWindow
{
	/// <summary>
	/// LoginWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class LoginWindow : Window
	{
		private List<Label> labels = new List<Label>();
		private int index = -1;
		private static string pw = "";

		public LoginWindow()
		{
			InitializeComponent();

			labels.Add(label1);
			labels.Add(label2);
			labels.Add(label3);
			labels.Add(label4);
		}
		
		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (index == 1)
				{

				}
				else if (index == 2)
				{

				}
				else if (index == 3)
					Close();
			}
			else if (e.Key == Key.Tab)
			{
				index++;
				if (index > 3)
					index = 0;
			}
			else if (index == 0)
			{
				pw += e.Key.ToString();
				label1.Content += "_ ";
			}

			// 그리기 갱신
			if (index >= 0 && index <= 3)
			{
				labels[index].Foreground = Brushes.Black;
				labels[index].Background = Brushes.White;
			}
		}

		// 그리기 초기화
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (index >= 0 && index <= 3)
			{
				labels[index].Foreground = Brushes.White;
				labels[index].Background = Brushes.Black;
			}
		}
	}
}
