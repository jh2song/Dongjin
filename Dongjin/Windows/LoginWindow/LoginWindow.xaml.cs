﻿using Dongjin.Classes;
using Dongjin.Table;
using SQLite;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

			controls.Add(passwordBox1);
			controls.Add(label1);
			controls.Add(label2);
			controls.Add(label3);
			controls.Add(label4);
		}

		private void RenderUpdate()
		{
			// 그리기 갱신
			if (index == 0)
				return;

			controls[index].Foreground = Brushes.Black;
			controls[index].Background = Brushes.White;
		}

		private void RenderInitialize()
		{
			// 그리기 초기화
			if (index >= 1 && index <= 4)
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
				passwordBox1.Focusable = false;
				controls[index].Focus();
			}
			else if (e.Key == Key.Escape)
			{
				passwordBox1.Clear();
			}
			else if (e.Key == Key.Enter)
			{
				Login();
			}
		}

		private void label1_KeyDown(object sender, KeyEventArgs e)
		{
			RenderInitialize();

			if (e.Key == Key.Tab || e.Key == Key.Right)
			{
				index++;
				controls[index].Focus();
			}
			else if (e.Key == Key.Enter)
			{
				MessageBox.Show(passwordBox1.Password, "입력한 비밀번호를 확인하세요", MessageBoxButton.OK, MessageBoxImage.Information);
			}
			else if (e.Key == Key.Left)
			{
				index--;
				controls[index].Focusable = true;
				controls[index].Focus();
			}
		}

		// 비밀번호 수정
		private void Label2_KeyDown(object sender, KeyEventArgs e)
		{
			RenderInitialize();

			if (e.Key == Key.Tab || e.Key == Key.Right)
			{
				index++;
				controls[index].Focus();
			}
			else if (e.Key == Key.Enter)
			{
				UpdatePassword();
			}
			else if (e.Key == Key.Left)
			{
				index--;
				controls[index].Focus();
			}
		}

		// 로그인
		private void label3_KeyDown(object sender, KeyEventArgs e)
		{
			RenderInitialize();

			if (e.Key == Key.Tab || e.Key == Key.Right)
			{
				index++;
				controls[index].Focus();
			}
			else if (e.Key == Key.Enter)
			{
				Login();
			}
			else if (e.Key == Key.Left)
			{
				index--;
				controls[index].Focus();
			}
		}

		// 작업 종료
		private void label4_KeyDown(object sender, KeyEventArgs e)
		{
			RenderInitialize();

			if (e.Key == Key.Tab || e.Key == Key.Right)
			{
				index = 0;
				controls[index].Focusable = true;
				controls[index].Focus();
			}
			else if (e.Key == Key.Enter)
			{
				Close();
			}
			else if (e.Key == Key.Left)
			{
				index--;
				controls[index].Focus();
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			passwordBox1.Focus();
		}

		private void Login()
		{
			try
			{
				var options = new SQLiteConnectionString(App.databasePath, false, key: passwordBox1.Password);
				DB.Conn = new SQLiteConnection(options);
				DB.Conn.CreateTable<TEST>();
				new MenuWindow.MenuWindow().Show();
				Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine(ex);
				MessageBox.Show("비밀번호가 잘못되었습니다.", "로그인 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		bool passwordValidCheck = false;
		string previousPassword = "";
		private void UpdatePassword()
		{
			try
			{
				if (passwordValidCheck == false)
				{
					var options = new SQLiteConnectionString(App.databasePath, false, key: passwordBox1.Password);
					DB.Conn = new SQLiteConnection(options);
					DB.Conn.CreateTable<TEST>();

					passwordValidCheck = true;
					previousPassword = passwordBox1.Password;
					MessageBox.Show("비밀번호가 확인되었습니다. 수정할 비밀번호를 입력 후 다시 진행해 주십시오.", "로그인 확인", MessageBoxButton.OK, MessageBoxImage.Information);
					passwordBox1.Clear();

					RenderInitialize();
					index = 0;
					controls[index].Focusable = true;
					controls[index].Focus();
				}
				else
				{
					var options = new SQLiteConnectionString(App.databasePath, false, key: previousPassword);
					DB.Conn = new SQLiteConnection(options);
					DB.Conn.Execute(query: $"PRAGMA rekey=`{passwordBox1.Password}`;");
					DB.Conn.CreateTable<TEST>();

					passwordValidCheck = false;
					MessageBox.Show("비밀번호 수정 완료", "비밀번호 수정 완료", MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
			catch (SQLiteException ex)
			{
				DB.Conn.Close();
				Console.WriteLine(ex);
				MessageBox.Show("비밀번호가 잘못되었습니다.", "로그인 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}