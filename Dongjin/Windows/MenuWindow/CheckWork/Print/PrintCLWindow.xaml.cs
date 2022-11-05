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

namespace Dongjin.Windows.MenuWindow.CheckWork.Print
{
    /// <summary>
    /// Interaction logic for PrintCLWindow.xaml
    /// </summary>
    public partial class PrintCLWindow : Window
    {
        private DataGrid _DG;
        private string _sumSell;
        private string _sumRefund;
        private string _sumDeposit;

        public PrintCLWindow(DataGrid DG, string sumSell, string sumRefund, string sumDeposit)
        {
            InitializeComponent();

            _DG = DG;
            _sumSell = sumSell;
            _sumRefund = sumRefund;
            _sumDeposit = sumDeposit;

            SetDataGrid();
        }

        private void SetDataGrid()
        {
            
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
