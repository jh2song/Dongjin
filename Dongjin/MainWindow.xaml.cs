using Dongjin.Classes;
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
		private List<Border> borders = new List<Border>();

		private bool underVisible = false;

		public MainWindow()
		{
			InitializeComponent();

			DateRender();

			borders.Add(borderUnder1);
			borders.Add(borderUnder2);
			borders.Add(borderUnder3);
			borders.Add(new Border());
			borders.Add(new Border());
			borders.Add(borderUnder6);

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

		int topIndex = 0;
		//int underIndex = 0;
		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			switch(e.Key)
			{
				case (Key.Enter):
					underVisible = !underVisible;
					break;
				case (Key.Left):
					topIndex = (topIndex - 1 + 6) % 6;
					break;
				case (Key.Right):
					topIndex = (topIndex + 1) % 6;
					break;

				case (Key.Down):

					break;
				case (Key.Up):

					break;
			}

			
		}
	}
}
