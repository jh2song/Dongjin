﻿using Dongjin.Classes;
using Dongjin.Windows.MenuWindow.BaseWork;
using Dongjin.Windows.MenuWindow.CheckWork;
using Dongjin.Windows.MenuWindow.DailyWork;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Dongjin.Windows.MenuWindow
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MenuWindow : Window
	{
		private List<LabelClass> labels = new List<LabelClass>();
		private List<Grid> grids = new List<Grid>();

		private bool underVisible = false;
		private int topIndex = -1;
		private int underIndex = 0;

		public MenuWindow()
		{
			InitializeComponent();

			DateRender();

			SetList();

			TestFunction();
		}

		public void TestFunction()
		{
			DB.Conn.CreateTable<Table.Client>();
			// await DB.Conn.ExecuteAsync($"DELETE FROM Client WHERE Code = {1001};");
			// await DB .Conn.CloseAsync();
		}

		// 기존에 그린것을 초기화
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			// topIndex가 범위안에 있는지 체크
			if (topIndex < 0 || topIndex >= 6)
				return;

			// 좌, 우 키를 눌렀을 때 그 전 레이블을 초기화하는 작업
			if (e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Tab)
			{
				labels[topIndex].TopLabel.Background = Brushes.Black;
				labels[topIndex].TopLabel.Foreground = Brushes.Yellow;
				if (grids[topIndex] != null)
					grids[topIndex].Visibility = Visibility.Hidden;
			}

			if (labels[topIndex].UnderLabels == null)
				return;

			labels[topIndex].UnderLabels[underIndex].Background = Brushes.Black;
			labels[topIndex].UnderLabels[underIndex].Foreground = Brushes.Pink;
		}

		// 다시 그리기
		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case (Key.Escape):
					underVisible = false;
					break;

				case (Key.Enter):
					if (underVisible == true)
					{
						underVisible = false;
						NextSession();
					}
					else
						underVisible = true;
					break;

				case (Key.Right):
				case (Key.Tab):
					topIndex++;
					if (topIndex >= 6)
						topIndex = 0;
					underIndex = 0;

					break;

				case (Key.Left):
					topIndex--;
					if (topIndex < 0)
						topIndex = 5;
					underIndex = 0;

					break;

				case (Key.Up):
					if (underVisible == false)
						break;
					if (topIndex < 0 || topIndex >= 6)
						break;

					underIndex--;
					if (labels[topIndex].UnderLabels == null)
						break;

					if (underIndex < 0)
						underIndex = labels[topIndex].UnderLabels.Count - 1;

					break;

				case (Key.Down):
					if (underVisible == false)
						break;
					if (topIndex < 0 || topIndex >= 6)
						break;

					underIndex++;
					if (labels[topIndex].UnderLabels == null)
						break;
					if (underIndex >= labels[topIndex].UnderLabels.Count)
						underIndex = 0;

					break;
			}

			MenuRender();
		}

		private void NextSession()
		{
			if (topIndex == 0 && underIndex == 0) // 거래처등록
			{
				new ClientsWindow().Show();
			}

			if (topIndex == 0 && underIndex == 1) // 제품등록
			{
				new ProductWindow().Show();
			}
			
			if (topIndex == 0 && underIndex == 2) // 할인율등록
			{
				new DiscountWindow().Show();
			}

			if (topIndex == 0 && underIndex == 3) // 브랜드등록
			{
				new BrandWindow().Show();
			}

			if (topIndex == 1 && underIndex == 0) // 입출고전표
			{
				new TransactionWindow().Show();
			}

			if (topIndex == 2 && underIndex == 0) // 미수금 현황
			{
				new LeftMoneyStatusWindow().Show();
			}

			if (topIndex == 2 && underIndex == 1) // 거래처 원장
			{
				new ClientLedgerWindow().Show();
			}

			if (topIndex == 2 && underIndex == 2) // 과별거래실적
			{
				new ResultByDepartmentWindow().Show();
			}

			if (topIndex == 2 && underIndex == 3) // 사입가 수정
			{
				new BuyingPercentUpdateWindow().Show();
			}

			if (topIndex == 5 && underIndex == 0) // 작업마침
			{
				Close();
			}
		}

		private void MenuRender()
		{
			if (topIndex < 0 || topIndex >= 6)
				return;

			labels[topIndex].TopLabel.Background = Brushes.Green;
			labels[topIndex].TopLabel.Foreground = Brushes.Black;

			if (underVisible)
			{
				if (grids[topIndex] != null)
					grids[topIndex].Visibility = Visibility.Visible;

				if (labels[topIndex].UnderLabels == null)
					return;
				labels[topIndex].UnderLabels[underIndex].Background = Brushes.White;
				labels[topIndex].UnderLabels[underIndex].Foreground = Brushes.Black;
			}
			else
			{
				if (grids[topIndex] != null)
					grids[topIndex].Visibility = Visibility.Hidden;
			}
		}

		private void DateRender()
		{
			var fd = new FlowDocument();

			fd.TextAlignment = TextAlignment.Right;

			Paragraph paragraph = new Paragraph();

			paragraph.Inlines.Add(new Run("오늘은 ") { Foreground = Brushes.White });
			paragraph.Inlines.Add(new Run(DateTime.Now.Year.ToString().Substring(2, 2)) { Foreground = Brushes.Red });
			paragraph.Inlines.Add(new Run("년 ") { Foreground = Brushes.White });
			paragraph.Inlines.Add(new Run(DateTime.Now.Month.ToString("00")) { Foreground = Brushes.Red });
			paragraph.Inlines.Add(new Run("월 ") { Foreground = Brushes.White });
			paragraph.Inlines.Add(new Run(DateTime.Now.Day.ToString("00")) { Foreground = Brushes.Red });
			paragraph.Inlines.Add(new Run("일 ") { Foreground = Brushes.White });
			paragraph.Inlines.Add(new Run(DateTime.Now.ToString("ddd")) { Foreground = Brushes.Red });
			paragraph.Inlines.Add(new Run("요일") { Foreground = Brushes.White });

			fd.Blocks.Add(paragraph);

			rtbDate.Document = fd;
		}

		private void SetList()
		{
			LabelClass top1 = new LabelClass();
			top1.TopLabel = topLabel1;
			top1.UnderLabels = new List<Label>() { underLabel11, underLabel12, underLabel13, underLabel14};
			labels.Add(top1);

			LabelClass top2 = new LabelClass();
			top2.TopLabel = topLabel2;
			top2.UnderLabels = new List<Label>() { underLabel21 };
			labels.Add(top2);

			LabelClass top3 = new LabelClass();
			top3.TopLabel = topLabel3;
			top3.UnderLabels = new List<Label>() { underLabel31, underLabel32, underLabel33, underLabel34 };
			labels.Add(top3);

			LabelClass top4 = new LabelClass();
			top4.TopLabel = topLabel4;
			top4.UnderLabels = null;
			labels.Add(top4);

			LabelClass top5 = new LabelClass();
			top5.TopLabel = topLabel5;
			top5.UnderLabels = null;
			labels.Add(top5);

			LabelClass top6 = new LabelClass();
			top6.TopLabel = topLabel6;
			top6.UnderLabels = new List<Label>() { underLabel61, underLabel62 };
			labels.Add(top6);

			grids.Add(underGrid1);
			grids.Add(underGrid2);
			grids.Add(underGrid3);
			grids.Add(null);
			grids.Add(null);
			grids.Add(underGrid6);
		}

		// block bubbling
		private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (Bubble.bubble)
			{
				e.Handled = true;
				Bubble.bubble = false;
				return;
			}
		}

		private void Window_Activated(object sender, EventArgs e)
		{
			Bubble.bubble = true;
		}
	}
}