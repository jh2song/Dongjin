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
using Dongjin.Windows.MenuWindow;
using System.IO;
using System.Security.Cryptography;

namespace Dongjin.Windows.LoginWindow
{
	/// <summary>
	/// LoginWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class LoginWindow : Window
	{
		private List<Control> controls = new List<Control>();
		private int index = 0;

		public LoginWindow()
		{
			InitializeComponent();

			controls.Add(passwordBox0);
			controls.Add(label1);
			controls.Add(label2);
			controls.Add(label3);
		}

		private void RenderUpdate()
		{
			// 그리기 갱신
			controls[index].Foreground = Brushes.Black;
			controls[index].Background = Brushes.White;
		}

		private void RenderInitialize()
		{
			// 그리기 초기화
			if (index >= 0 && index <= 3)
			{
				controls[index].Foreground = Brushes.White;
				controls[index].Background = Brushes.Black;
			}
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			RenderUpdate();
		}

		private void PB0_KeyDown(object sender, KeyEventArgs e)
		{
			RenderInitialize();

			if (e.Key == Key.Tab)
			{
				index = 1;
				passwordBox0.Focusable = false;
				controls[index].Focus();
			}
			else if (e.Key == Key.Escape)
			{
				passwordBox0.Clear();
			}
			else if (e.Key == Key.Enter)
			{
				Login();
			}
		}

		private void Login()
		{
			throw new NotImplementedException();
		}

		private void label1_KeyDown(object sender, KeyEventArgs e)
		{
			RenderInitialize();

			if (e.Key == Key.Tab)
			{
				index++;
				controls[index].Focus();
			}
			else if (e.Key == Key.Enter) // 비밀번호 수정
			{

			}
		}

		private void label2_KeyDown(object sender, KeyEventArgs e)
		{
			RenderInitialize();

			if (e.Key == Key.Tab)
			{
				index++;
				controls[index].Focus();
			}
			else if (e.Key == Key.Enter) // 로그인
			{

			}
		}

		private void label3_KeyDown(object sender, KeyEventArgs e)
		{
			RenderInitialize();

			if (e.Key == Key.Tab)
			{
				index = 0;
				controls[index].Focusable = true;
				controls[index].Focus();
			}
			else if (e.Key == Key.Enter) // 작업 종료
			{
				Close();
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			passwordBox0.Focus();
		}
	}
}