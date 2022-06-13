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

		bool pwChangeTrigger = false;
		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (index == 1) // 비밀번호 수정
				{
					if (App.pwHash == SHA256Class.SHA256Hash(pw))
					{
						MessageBox.Show("ESC를 눌러 새로운 암호를 입력하세요.", "암호 변경", MessageBoxButton.OK, MessageBoxImage.Information);
						pwChangeTrigger = true;
					}
					else if (pwChangeTrigger == true)
					{
						App.pwHash = SHA256Class.SHA256Hash(pw);
						System.IO.File.WriteAllText(App.passwordPath, App.pwHash);
						MessageBox.Show("암호가 변경되었습니다. ESC로 빠져 나가십시오.", "암호 변경", MessageBoxButton.OK, MessageBoxImage.Information);
						pwChangeTrigger = false;
					}
					else
					{
						MessageBox.Show("기존 암호가 다릅니다. ESC를 눌러서 다시 입력하세요.", "암호 오류", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
				else if (index == 2) // 로그인
				{
					if (App.pwHash == SHA256Class.SHA256Hash(pw))
					{
						new MenuWindow.MenuWindow().ShowDialog();
						DBConnectClass.DBConnect(App.databasePath);
						Close();
					}
					else
					{
						MessageBox.Show("암호가 다릅니다. ESC를 눌러서 다시 입력하세요.", "암호 오류", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}
				else if (index == 3) // 작업 종료
					Close();
			}
			else if (e.Key == Key.Tab)
			{
				index++;
				if (index > 3)
					index = 0;
			}
			else if (e.Key == Key.Escape)
			{
				pw = "";
				label1.Content = "";
			}
			else if (index == 0)
			{
				pw += e.Key.ToString();
				label1.Content += " ";
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

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				App.pwHash = System.IO.File.ReadAllText(App.passwordPath);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);

				System.IO.File.WriteAllText(App.passwordPath, SHA256Class.SHA256Hash(Key.D1.ToString() + Key.D2.ToString() + Key.D3.ToString() + Key.D4.ToString()));
				App.pwHash = System.IO.File.ReadAllText(App.passwordPath);
				MessageBox.Show("비밀번호 파일이 없습니다. 기본 비밀번호는 '1234'입니다. 비밀번호를 수정해주세요. ESC로 빠져 나가십시오.", "비밀번호 변경 요청", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}