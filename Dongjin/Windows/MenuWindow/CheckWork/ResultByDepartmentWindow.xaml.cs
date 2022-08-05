﻿using Dongjin.Classes;
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
	/// <summary>
	/// Interaction logic for ResultByDepartmentWindow.xaml
	/// </summary>
	public partial class ResultByDepartmentWindow : Window
	{
		private int _choice;
		private DateTime _dateTime;

		public ResultByDepartmentWindow()
		{
			InitializeComponent();

			ChoiceTB.Focus();
		}

		private void ChoiceTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
		}

		private void YearTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
		}

		private void MonthTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
		}

		private void DayTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = RegexClass.NotNumericBackspace(e.Text);
		}

		private void ChoiceTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && ChoiceTB.Text == "")
			{
				Close();
			}

			if (e.Key == Key.Escape && ChoiceTB.Text != "")
			{
				ChoiceTB.Text = "";
			}

			if (e.Key == Key.Enter && int.TryParse(ChoiceTB.Text, out _choice))
			{
				if (_choice == 1 || _choice == 2)
				{
					YearTB.Focus();
				}
			}
		}

		private void YearTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape && YearTB.Text == "")
			{
				ChoiceTB.Text = "";
				ChoiceTB.Focus();
			}

			if (e.Key == Key.Escape && YearTB.Text != "")
			{
				YearTB.Text = "";
			}

			if (e.Key == Key.Enter)
			{
				if (YearTB.Text.Length < 2)
					YearTB.Text = DateTime.Now.Year.ToString().Substring(2, 2);

				MonthTB.Focus();
			}
		}

		private void MonthTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (MonthTB.Text == "")
				{
					YearTB.Text = "";
					YearTB.Focus();
				}
				else
				{
					MonthTB.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (MonthTB.Text.Length < 2)
					MonthTB.Text = DateTime.Now.Month.ToString("00");

				DayTB.Focus();
			}
		}

		private void DayTB_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (DayTB.Text == "")
				{
					MonthTB.Text = "";
					MonthTB.Focus();
				}
				else
				{
					DayTB.Text = "";
				}
			}

			if (e.Key == Key.Enter)
			{
				if (DayTB.Text.Length < 2)
					DayTB.Text = DateTime.Now.Day.ToString("00");

				if (VerifyDate() == false)
				{
					YearTB.Text = MonthTB.Text = DayTB.Text = "";
					YearTB.Focus();

					return;
				}

				if (_choice == 1)
				{
					
				}

				if (_choice == 2)
				{

				}

				DayTB.Select(DayTB.Text.Length, 0);
			}
		}

		private bool VerifyDate()
		{
			try
			{
				_dateTime = new DateTime(int.Parse("20" + YearTB.Text), int.Parse(MonthTB.Text), int.Parse(DayTB.Text));
				return true;
			}
			catch (Exception)
			{
				MessageBox.Show("날짜가 잘못되었습니다.", "날짜 오류", MessageBoxButton.OK, MessageBoxImage.Error);
			}

			return false;
		}
	}
}
