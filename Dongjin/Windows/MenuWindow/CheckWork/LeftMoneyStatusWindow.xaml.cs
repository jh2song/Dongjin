using Dongjin.Classes;
using Dongjin.Table;
using Dongjin.Windows.MenuWindow.CheckWork.Print;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Dongjin.Windows.MenuWindow.CheckWork
{
	public partial class LeftMoneyStatusWindow : Window
	{
		private int _departmentCode;
		private int? _finalSumMoney;

		public LeftMoneyStatusWindow()
		{
			InitializeComponent();

			DepartmentCodeTB.Focus();
		}

		private void DepartmentCodeTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (DepartmentCodeTB.Text == "")
				{
					Close();
				}
				if (DepartmentCodeTB.Text != "")
				{
					DepartmentCodeTB.Text = "";
				}
			}

			// 1~8 
			if (e.Key == Key.Enter && int.TryParse(DepartmentCodeTB.Text, out _departmentCode))
			{
				if (_departmentCode >= 1 && _departmentCode <= 8)
				{
					DG.BorderBrush = Brushes.White;
					ShowDatagrid();
				}
				if (_departmentCode == 9)
				{
					ShowSumDatagrid();
				}
			}

			// P1-P8
			if (e.Key == Key.Enter && 
				(DepartmentCodeTB.Text.Length == 2 && 
				(DepartmentCodeTB.Text[0] == 'p' || DepartmentCodeTB.Text[0] == 'P') &&
				int.TryParse(DepartmentCodeTB.Text.Substring(1), out _departmentCode)))
			{
				if (_departmentCode < 1 || _departmentCode > 8)
					return;


			}

		}

		// Datagrid에 띄울 클래스 생성
		class LeftMoneyStatus
		{
			public int? ClientCode { get; set; }
			public string ClientName { get; set; }
			public int? PercentCode { get; set; }
			public int? CurrentLeftMoney { get; set; }
			public DateTime? FinalDepositDate { get; set; }
			public DateTime? FinalTransactionDate { get; set; }
			public string Phone { get; set; }
		}

		private void ShowDatagrid()
		{
			try
			{
				_finalSumMoney = 0;

				DB.Conn.CreateTable<Client>();
				DB.Conn.CreateTable<ClientLedger>();

				var query = DB.Conn.Query<LeftMoneyStatus>(
@"
SELECT a.ClientCode, a.ClientName, a.PercentCode, b.CurrentLeftMoney, a.FinalDepositDate, a.FinalTransactionDate, a.Phone
FROM Client AS a, ClientLedger AS b, 
	(SELECT ClientCode, MAX(TransactionDate) AS MaxTransactionDate 
	FROM ClientLedger 
	GROUP BY ClientCode) AS c					
WHERE a.ClientCode = b.ClientCode AND b.ClientCode = c.ClientCode AND b.TransactionDate = c.MaxTransactionDate;
"
					).ToList();

				List<LeftMoneyStatus> lms = new List<LeftMoneyStatus>();
				foreach (var item in query)
				{
					if (item.ClientCode / 1000 != _departmentCode)
						continue;

					LeftMoneyStatus element = new LeftMoneyStatus();
					element.ClientCode = item.ClientCode;
					element.ClientName = item.ClientName;
					element.PercentCode = item.PercentCode;
					element.CurrentLeftMoney = item.CurrentLeftMoney;
					element.FinalDepositDate = item.FinalDepositDate;
					element.FinalTransactionDate = item.FinalTransactionDate;
					element.Phone = item.Phone;
					lms.Add(element);
					_finalSumMoney += item.CurrentLeftMoney;
				}

				lms = lms.OrderBy(t => t.ClientCode).ToList();
				DG.ItemsSource = null;
				DG.ItemsSource = lms;
				FinalSumPriceTB.Text = String.Format("{0:#,0}", _finalSumMoney);
			}
			catch (Exception)
			{
				MessageBox.Show("표에 미수금현황을 불러오는데 실패하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		
		private void ShowSumDatagrid()
		{
			try
			{
				List<LeftMoneyStatus> bindingList = new List<LeftMoneyStatus>();
				DB.Conn.CreateTable<ClientLedger>();
				var all = DB.Conn.Query<ClientLedger>(
@"
SELECT a.ClientCode, a.CurrentLeftMoney
FROM ClientLedger a, 
	(
	SELECT ClientCode, MAX(TransactionDate) as MaxTransactionDate
	FROM ClientLedger
	GROUP BY ClientCode
	) b
WHERE a.ClientCode = b.ClientCode AND a.TransactionDate = b.MaxTransactionDate;
"
					).ToList();

				_finalSumMoney = 0;
				for (int i = 1; i <= 8; i++)
				{
					// 과별로 나눔
					var splited = all.Where(cl => cl.ClientCode / 1000 == i).ToList();
					LeftMoneyStatus status = new LeftMoneyStatus();

					int leftMoneyGroupByClientCode = 0;
					foreach (var item in splited)
					{
						leftMoneyGroupByClientCode += item.CurrentLeftMoney;
					}

					status.ClientName = "제 " + i.ToString() + " 과";
					status.CurrentLeftMoney = leftMoneyGroupByClientCode;
					bindingList.Add(status);

					_finalSumMoney += leftMoneyGroupByClientCode;
				}

				
				DG.ItemsSource = null;
				DG.ItemsSource = bindingList;
				
				FinalSumPriceTB.Text = String.Format("{0:#,0}", _finalSumMoney);
			}
			catch (Exception)
			{
				MessageBox.Show("과별합계를 불러오는데 실패하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
			if (e.Key == Key.F5)
				new PrintLMSWindow(DG).Show();

			return;
		}
    }
}
