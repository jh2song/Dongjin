using Dongjin.Classes;
using Dongjin.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dongjin.Controls
{
	/// <summary>
	/// Interaction logic for ClientControl.xaml
	/// </summary>
	public partial class ClientControl : UserControl
	{
		public Client Client
		{
			get { return (Client)GetValue(ClientProperty); }
			set { SetValue(ClientProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Contact.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ClientProperty =
			DependencyProperty.Register("Contact", typeof(Client), typeof(ClientControl), new PropertyMetadata(new Client() { ClientCode = 1234, ClientName = "테스트", Phone = "(123) 456 7890" }, SetText));

		private static void SetText(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ClientControl control = d as ClientControl;

			if (control != null)
			{
				control.ClientCodeTB.Text = (e.NewValue as Client).ClientCode.ToString();
				control.ClientNameTB.Text = (e.NewValue as Client).ClientName;
				control.PhoneTB.Text = (e.NewValue as Client).Phone;
				control.CurrentLeftMoneyTB.Text = GetCurrentLeftMoney((e.NewValue as Client).ClientCode).ToString();
			}
		}

		public ClientControl()
		{
			InitializeComponent();
		}

		// 현재미수금을 구하는 DML 코드
		private static int GetCurrentLeftMoney(int clientCode)
		{
			try
			{
				DB.Conn.CreateTable<ClientLedger>();
				var _lastClientLedger = DB.Conn.Table<ClientLedger>().ToList().Where(cl => cl.ClientCode == clientCode)
					.OrderByDescending(cl => cl.TransactionDate).FirstOrDefault();

				if (_lastClientLedger != null)
					return _lastClientLedger.CurrentLeftMoney;

			}
			catch (Exception)
			{
				MessageBox.Show("현재미수금을 불러오는데 실패하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			return 0;
		}
	}
}
