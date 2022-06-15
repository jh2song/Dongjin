﻿using Dongjin.Classes;
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
using SQLite;
using Dongjin.Table;

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

		// 비밀번호 수정
		private void label1_KeyDown(object sender, KeyEventArgs e)
		{
			RenderInitialize();

			if (e.Key == Key.Tab)
			{
				index++;
				controls[index].Focus();
			}
			else if (e.Key == Key.Enter)
			{
				UpdatePassword();
			}
		}

		// 로그인
		private void label2_KeyDown(object sender, KeyEventArgs e)
		{
			RenderInitialize();

			if (e.Key == Key.Tab)
			{
				index++;
				controls[index].Focus();
			}
			else if (e.Key == Key.Enter)
			{
				Login();			
			}
		}

		// 작업 종료
		private void label3_KeyDown(object sender, KeyEventArgs e)
		{
			RenderInitialize();

			if (e.Key == Key.Tab)
			{
				index = 0;
				controls[index].Focusable = true;
				controls[index].Focus();
			}
			else if (e.Key == Key.Enter)
			{
				Close();
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			passwordBox0.Focus();
		}

		private async void Login()
		{
			try
			{
				var options = new SQLiteConnectionString(App.databasePath, true, key: passwordBox0.Password);
				DBAsyncConnectClass.Conn = new SQLiteAsyncConnection(options);
				await DBAsyncConnectClass.Conn.CreateTableAsync<TEST>();
				new MenuWindow.MenuWindow().Show();
				Close();
			}
			catch (SQLiteException ex)
			{
				await DBAsyncConnectClass.Conn.CloseAsync();
				Console.WriteLine(ex);
				MessageBox.Show("비밀번호가 잘못되었습니다.", "로그인 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		bool passwordValidCheck = false;
		private async void UpdatePassword()
		{
			try
			{
				if (passwordValidCheck == false)
				{
					var options = new SQLiteConnectionString(App.databasePath, true, key: passwordBox0.Password);
					DBAsyncConnectClass.Conn = new SQLiteAsyncConnection(options);
					await DBAsyncConnectClass.Conn.CreateTableAsync<TEST>();
					
					passwordValidCheck = true;
					MessageBox.Show("비밀번호가 확인되었습니다. 수정할 비밀번호를 입력 후 다시 진행해 주십시오.", "로그인 확인", MessageBoxButton.OK, MessageBoxImage.Information);
					passwordBox0.Clear();
				}
				else
				{
					var options = new SQLiteConnectionString(App.databasePath, true, key: passwordBox0.Password);
					DBAsyncConnectClass.Conn = new SQLiteAsyncConnection(options);
					await DBAsyncConnectClass.Conn.ExecuteAsync("PRAGMA rekey=" + passwordBox0.Password);
					await DBAsyncConnectClass.Conn.CreateTableAsync<TEST>();
					MessageBox.Show("비밀번호 수정 완료", "비밀번호 수정 완료", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			catch (SQLiteException ex)
			{
				await DBAsyncConnectClass.Conn.CloseAsync();
				Console.WriteLine(ex);
				MessageBox.Show("비밀번호가 잘못되었습니다.", "로그인 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}