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
    /// Interaction logic for PrintLMSWindow.xaml
    /// </summary>
    public partial class PrintLMSWindow : Window
    {
        public PrintLMSWindow(DataGrid DG)
        {
            InitializeComponent();

            DGPrint.ItemsSource = DG.Items;
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            FlowDocument fd = FDSV.Document;

            if (pd.ShowDialog() != null)
            {
                fd.PageHeight = pd.PrintableAreaHeight;
                fd.PageWidth = pd.PrintableAreaWidth;
                fd.PagePadding = new Thickness(50);
                fd.ColumnGap = 0;
                fd.ColumnWidth = pd.PrintableAreaWidth;

                IDocumentPaginatorSource dps = fd;
                pd.PrintDocument(dps.DocumentPaginator, "flow doc");
                this.Close();
            }
        }
    }
}
