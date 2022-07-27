﻿using Dongjin.Classes;
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
using System.Windows.Shapes;

namespace Dongjin.Windows.MenuWindow.CheckWork
{
	public partial class LeftMoneyStatusWindow : Window
	{
		private int _departmentCode;
		private int _finalSumMoney;

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
					ShowDatagrid();
				}
				if (_departmentCode == 9)
				{

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
			public int ClientCode { get; set;}
			public string ClientName { get; set; }
			public int PercentCode { get; set; }
			public int CurrentLeftMoney { get; set; }
			public DateTime FinalDepositDate { get; set; }
			public DateTime FinalTransactionDate { get; set; }
			public string Phone { get; set; }
		}

		private void ShowDatagrid()
		{
			try
			{
				_finalSumMoney = 0;

				DB.Conn.CreateTable<Client>();
				DB.Conn.CreateTable<ClientLedger>();

				var query =
				from a in DB.Conn.Table<Client>()
				join b in DB.Conn.Table<ClientLedger>()
				on a.ClientCode equals b.ClientCode
				orderby b.TransactionDate descending
				select new { a.ClientCode, a.ClientName, a.PercentCode, b.CurrentLeftMoney, a.FinalDepositDate, a.FinalTransactionDate, a.Phone };

				List<LeftMoneyStatus> lms = new List<LeftMoneyStatus>();
				foreach (var item in query)
				{
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

				DG.ItemsSource = null;
				DG.ItemsSource = lms;
				FinalSumPriceTB.Text = String.Format("{0:#,0}", _finalSumMoney);
			}
			catch (Exception)
			{
				MessageBox.Show("표에 미수금현황을 불러오는데 실패하였습니다.", "DB 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
