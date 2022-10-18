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
using System.Windows.Shapes;
using static Dongjin.Windows.MenuWindow.DailyWork.TransactionWindow;

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
        List<Document> _DG;
        PrintMonth _printMonth;

        public PrintTransactionWindow(string printOption, DateTime transactionDate, Client foundClient, List<Document> DG,
           PrintMonth printMonth)
        {
            InitializeComponent();

            _printOption = printOption;
            _transactionDate = transactionDate;
            _foundClient = foundClient;
            _DG = DG;
            _printMonth = printMonth;

            SetDate();
            SetClientInfo();
            SetDataGrid();
            SetDetails();
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
			switch (_printOption)
			{
                case "P":
                    SetOnly0();
                    break;
                case "P1":
                    SetOnly1();
                    break;
                case "P2":
                    SetOnly2();
                    break;
                case "P0":
                    SetAll();
                    break;
			}
        }

		private void SetOnly0()
		{
            _DG = _DG.Where(dc => dc.AppendOption0 == 1 
            && dc.AppendOption1 == 0 
            && dc.AppendOption2 == 0)
                .ToList();

            DGPrint.ItemsSource = _DG;
		}

		private void SetOnly1()
		{
            _DG = _DG.Where(dc => dc.AppendOption0 == 0
           && dc.AppendOption1 == 1
           && dc.AppendOption2 == 0)
               .ToList();

            DGPrint.ItemsSource = _DG;
        }

		private void SetOnly2()
		{
            _DG = _DG.Where(dc => dc.AppendOption0 == 0
          && dc.AppendOption1 == 0
          && dc.AppendOption2 == 1)
              .ToList();

            DGPrint.ItemsSource = _DG;
        }

		private void SetAll()
		{
            DGPrint.ItemsSource = _DG;
        }

        private void SetDetails()
        {
            // 당일분
            SetTodayJobs();

            // 월집계
            LBPrevMonthLeftMoney.Content = _printMonth.PrevMonthLeftMoney;
            LBThisMonthSellingMoney.Content = _printMonth.ThisMonthSellingMoney;
            LBThisMonthRefundMoney.Content = _printMonth.ThisMonthRefundMoney;
            LBThisMonthDepositMoney.Content = _printMonth.ThisMonthDepositMoney;

            // 공지사항
            SetAlarm();
        }

		private void SetTodayJobs()
		{
            DB.Conn.CreateTable<ClientLedger>();
            var clientLedger = 
            DB.Conn.Table<ClientLedger>()
                .Where(cl => cl.ClientCode == _foundClient.ClientCode
            && cl.TransactionDate == _transactionDate).ToList().FirstOrDefault();

            LBPrevDayLeftMoney.Content = String.Format("{0:#,0}", (clientLedger.CurrentLeftMoney
                - clientLedger.TodaySellMoney
                + clientLedger.TodayRefundMoney
                + clientLedger.TodayDepositMoney));

            DB.Conn.CreateTable<Document>();
            LBSellingCount.Content = String.Format("{0:#,0}", DB.Conn.Table<Document>().Where(d => d.TransactionDate == _transactionDate
            && d.ClientCode == _foundClient.ClientCode).Sum(d => d.ProductCount));

            LBSellingMoney.Content = String.Format("{0:#,0}", clientLedger.TodaySellMoney);

            LBCurrentLeftMoney.Content = String.Format("{0:#,0}", clientLedger.CurrentLeftMoney);
        }

		private void SetAlarm()
		{
            DB.Conn.CreateTable<Alarm>();

            Alarm alarmObj = DB.Conn.Table<Alarm>().ToList().FirstOrDefault();

            if (alarmObj != null)
            {
                LBAlarm1.Content = alarmObj.AlarmString1;
                LBAlarm2.Content = alarmObj.AlarmString2;
            }
        }
	}
}
