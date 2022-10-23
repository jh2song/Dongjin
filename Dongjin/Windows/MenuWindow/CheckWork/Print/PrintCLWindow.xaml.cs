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

        public PrintCLWindow(DataGrid DG)
        {
            InitializeComponent();

            _DG = DG;
            SetDataGrid();
        }

        private void SetDataGrid()
        {
            
        }
    }
}
