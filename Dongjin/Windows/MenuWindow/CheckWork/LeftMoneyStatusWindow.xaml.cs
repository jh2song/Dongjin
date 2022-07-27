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

namespace Dongjin.Windows.MenuWindow.CheckWork
{
	public partial class LeftMoneyStatusWindow : Window
	{
		private int _departmentCode;

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
	}
}
