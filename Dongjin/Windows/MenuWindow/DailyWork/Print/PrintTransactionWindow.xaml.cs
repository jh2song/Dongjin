using Dongjin.Classes;
using Dongjin.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Markup;
using static Dongjin.Windows.MenuWindow.DailyWork.TransactionWindow;
using System.IO;

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
        List<Document> _AllDatagrid;
        private string _LBTotalSellingCount;
        private TransactionWindow _callingWindow;
        PrintMonth _printMonth;

        public PrintTransactionWindow(string printOption, DateTime transactionDate, Client foundClient, List<Document> AllDatagrid,
           PrintMonth printMonth, string LBTotalSellingCount, TransactionWindow callingWindow)
        {
            InitializeComponent();

            _printOption = printOption;
            _transactionDate = transactionDate;
            _foundClient = foundClient;
            _AllDatagrid = AllDatagrid;
            _LBTotalSellingCount = LBTotalSellingCount;
            _callingWindow = callingWindow;

            // _DG 수량*단가=금액
            // 으로 수정
            foreach (var item in _AllDatagrid.ToList())
            {
                item.Price = item.DiscountPrice / item.ProductCount;
            }

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
            DGPrint1.ItemsSource = _AllDatagrid.Where(dc => dc.Choice == 1
            && dc.AppendOption0 == 1 
            && dc.AppendOption1 == 0 
            && dc.AppendOption2 == 0)
                .ToList();

            DGPrint3.ItemsSource = _AllDatagrid.Where(dc => dc.Choice == 3
            && dc.AppendOption0 == 1
            && dc.AppendOption1 == 0
            && dc.AppendOption2 == 0)
                .ToList();
        }

		private void SetOnly1()
		{
           DGPrint1.ItemsSource = _AllDatagrid.Where(dc => dc.Choice == 1
           && dc.AppendOption0 == 0
           && dc.AppendOption1 == 1
           && dc.AppendOption2 == 0)
               .ToList();

            DGPrint3.ItemsSource = _AllDatagrid.Where(dc => dc.Choice == 3
           && dc.AppendOption0 == 0
           && dc.AppendOption1 == 1
           && dc.AppendOption2 == 0)
               .ToList();
        }

		private void SetOnly2()
		{
            DGPrint1.ItemsSource = _AllDatagrid.Where(dc => dc.Choice == 1
            && dc.AppendOption0 == 0
            && dc.AppendOption1 == 0
            && dc.AppendOption2 == 1)
                .ToList();

            DGPrint3.ItemsSource = _AllDatagrid.Where(dc => dc.Choice == 3
           && dc.AppendOption0 == 0
           && dc.AppendOption1 == 0
           && dc.AppendOption2 == 1)
               .ToList();
        }

		private void SetAll()
		{
            DGPrint1.ItemsSource = _AllDatagrid.Where(dc => dc.Choice == 1).ToList();
            DGPrint3.ItemsSource = _AllDatagrid.Where(dc => dc.Choice == 3).ToList();
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

            // 판매수량 계산
            LBSellingCount.Content = _LBTotalSellingCount;
            // string numbersOnly = new string(_LBTotalSellingCount.Where(char.IsDigit).ToArray());
            // LBSellingCount.Content = string.Format("{0:#,0}", int.Parse(numbersOnly));
            // 판매수량 끝

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

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            //PrintDialog pd = new PrintDialog();
            //FlowDocument fd = SV.Document;

            //if (pd.ShowDialog() != null)
            //{
            //    fd.PageHeight = pd.PrintableAreaHeight;
            //    fd.PageWidth = pd.PrintableAreaWidth;
            //    fd.PagePadding = new Thickness(20);
            //    fd.ColumnGap = 0;
            //    fd.ColumnWidth = pd.PrintableAreaWidth;

            //    IDocumentPaginatorSource dps = fd;
            //    pd.PrintDocument(dps.DocumentPaginator, "flow doc");
            //    this.Close();
            //    _callingWindow.Focus();
            //    _callingWindow.WindowState = WindowState.Maximized;
            //    _callingWindow.ProductCodeTB.Focus();
            //}

            Print(SPcontents);

            this.Close();
            _callingWindow.Focus();
            _callingWindow.WindowState = WindowState.Maximized;
            _callingWindow.ProductCodeTB.Focus();
        }

        public static void Print(FrameworkElement toPrint)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() != null)
            {
                var capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
                var pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
                var visibleSize = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
                var fixedDoc = new FixedDocument();

                //If the toPrint visual is not displayed on screen we neeed to measure and arrange it  
                toPrint.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                toPrint.Arrange(new Rect(new Point(0, 0), toPrint.DesiredSize));
                //  
                var size = toPrint.DesiredSize;
                //Will assume for simplicity the control fits horizontally on the page  
                double yOffset = 0;
                while (yOffset < size.Height)
                {
                    double leftOffset = (size.Width - visibleSize.Width) / 2;
                    var vb = new VisualBrush(toPrint)
                    {
                        Stretch = Stretch.None,
                        AlignmentX = AlignmentX.Left,
                        AlignmentY = AlignmentY.Top,
                        ViewboxUnits = BrushMappingMode.Absolute,
                        TileMode = TileMode.None,
                        Viewbox = new Rect(leftOffset, yOffset, size.Width, visibleSize.Height),
                        Viewport = new Rect(0, 0, visibleSize.Width, visibleSize.Height),
                        ViewportUnits = BrushMappingMode.Absolute
                    };

                    var pageContent = new PageContent();
                    var page = new FixedPage();
                    ((IAddChild)pageContent).AddChild(page);
                    fixedDoc.Pages.Add(pageContent);
                    page.Width = pageSize.Width;
                    page.Height = pageSize.Height;
                    var canvas = new Canvas();
                    FixedPage.SetLeft(canvas, capabilities.PageImageableArea.OriginWidth);
                    FixedPage.SetTop(canvas, capabilities.PageImageableArea.OriginHeight);
                    canvas.Width = visibleSize.Width;
                    canvas.Height = visibleSize.Height;
                    canvas.Background = vb;
                    page.Children.Add(canvas);
                    yOffset += visibleSize.Height;
                }
                
                IDocumentPaginatorSource dps = fixedDoc;
                printDialog.PrintDocument(dps.DocumentPaginator, "전표 출력");        
            }
        }
    }
}
