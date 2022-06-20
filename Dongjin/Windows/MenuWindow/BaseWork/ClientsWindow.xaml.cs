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
using Dongjin.Classes;


namespace Dongjin.Windows.MenuWindow.BaseWork
{
	/// <summary>
	/// ClientsWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ClientsWindow : Window
	{

		public ClientsWindow()
		{
			InitializeComponent();


			tb1.Text = DateTime.Now.Year.ToString().Substring(2, 2);
			tb2.Text = DateTime.Now.Month.ToString("00");
			tb3.Text = DateTime.Now.Day.ToString("00");

			// 거래처코드에 포커싱
			tb4.Focus();
		}

		private void SetList()
		{
		}

		private void TB4_KeyUp(object sender, KeyEventArgs e)
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
				if (IsThereInTable())
				{
					// 명령어 칸으로 이동
				}
				else
				{
					// 1. 상호 부터 새로 입력
				}
			}
		}

		private bool IsThereInTable()
		{
			throw new NotImplementedException();
		}
	}
}
