﻿using Dongjin.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Dongjin
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private List<LabelClass> labels = new List<LabelClass>();
		private List<Grid> grids = new List<Grid>();

		private bool underVisible = false;
		private int topIndex = -1;
		private int underIndex = 0;

		public MainWindow()
		{
			InitializeComponent();

			DateRender();
			SetList();

		}

		// 초기화
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{

			if (topIndex < 0 || topIndex >= 6)
				return;

			if (e.Key == Key.Right || e.Key == Key.Left)
			{
				labels[topIndex].TopLabel.Background = Brushes.Black;
				labels[topIndex].TopLabel.Foreground = Brushes.Yellow;
			}

			if (labels[topIndex].UnderLabels == null)
				return;

			labels[topIndex].UnderLabels[underIndex].Background = Brushes.Black;
			labels[topIndex].UnderLabels[underIndex].Foreground = Brushes.Pink;

			if (grids[topIndex] == null)
				return;

			grids[topIndex].Visibility = Visibility.Hidden;
		}

		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			switch(e.Key)
			{
				case (Key.Enter):
					underVisible = !underVisible;
					break;

				case (Key.Right):
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
					if (underIndex < 0)
						underIndex = labels[topIndex].UnderLabels.Count - 1;

					break;

				case (Key.Down):
					if (underVisible == false)
						break;
					if (topIndex < 0 || topIndex >= 6)
						break;

					underIndex++;
					if (underIndex >= labels[topIndex].UnderLabels.Count)
						underIndex = 0;

					break;
			}

			MenuRender();
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
			top1.UnderLabels = new List<Label>() { underLabel11, underLabel12, underLabel13, underLabel14 };
			labels.Add(top1);

			LabelClass top2 = new LabelClass();
			top2.TopLabel = topLabel2;
			top2.UnderLabels = new List<Label>() { underLabel21, underLabel22 };
			labels.Add(top2);

			LabelClass top3 = new LabelClass();
			top3.TopLabel = topLabel3;
			top3.UnderLabels = new List<Label>() { underLabel31, underLabel32, underLabel33 };
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

	}
}
