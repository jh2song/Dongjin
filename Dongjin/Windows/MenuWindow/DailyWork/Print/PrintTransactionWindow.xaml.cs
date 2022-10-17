using Dongjin.Table;
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

namespace Dongjin.Windows.MenuWindow.DailyWork.Print
{
    /// <summary>
    /// Interaction logic for PrintTransactionWindow.xaml
    /// </summary>
    public partial class PrintTransactionWindow : Window
    {
        string _printOption;
        DateTime _transactionDate;
        Client _foundClient;
        DataGrid _DG;

        public PrintTransactionWindow(string printOption, DateTime transactionDate, Client foundClient, DataGrid DG)
        {
            InitializeComponent();

            _printOption = printOption;
            _transactionDate = transactionDate;
            _foundClient = foundClient;
            _DG = DG;

            SetDate();
            SetClientInfo();
            SetDataGrid();
        }

		private void SetDate()
		{
            LBDate.Content = $"{_transactionDate.Year.ToString("0000")}년 {_transactionDate.Month.ToString("00")}월 {_transactionDate.Day.ToString("00")}일";
        }

        private void SetClientInfo()
        {
            string outClientInfo = "";
            outClientInfo += "거래처코드: ";
            outClientInfo += _foundClient.ClientCode.ToString() + "   ";
            outClientInfo += "전화: ";
            outClientInfo += _foundClient.Phone.ToString() + "   ";
            outClientInfo += "거래처명: ";
            outClientInfo +=  _foundClient.ClientName.ToString();
            LBClientInfo.Content = outClientInfo;
        }

        class DGPrintClass
        {
            public string ProductName { get; set; }
            public int ProductCount { get; set; }
            public int Price { get; set; }
            public int DiscountPrice { get; set; }
            public int AppendOption1 { get; set; }
            public int AppendOption2 { get; set; }
        }

        private void SetDataGrid()
        {
			
        }
    }
}
