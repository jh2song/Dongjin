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
		private List<Button> topButtons = new List<Button>();
		private List<List<Button>> underButtons = new List<List<Button>>();
		private List<Button> underButtons1 = new List<Button>();
		private List<Button> underButtons2 = new List<Button>();
		private List<Button> underButtons3 = new List<Button>();
		private List<Button> underButtons4 = new List<Button>();
		private List<Button> underButtons5 = new List<Button>();
		private List<Button> underButtons6 = new List<Button>();
		private List<Border> borders = new List<Border>();

		private bool underVisible = false;

		public MainWindow()
		{
			InitializeComponent();

			DateRender();

			topButtons.Add(topButton1);
			topButtons.Add(topButton2);
			topButtons.Add(topButton3);
			topButtons.Add(topButton4);
			topButtons.Add(topButton5);
			topButtons.Add(topButton6);

			underButtons1.Add(underButton11);
			underButtons1.Add(underButton12);
			underButtons1.Add(underButton13);
			underButtons1.Add(underButton14);

			underButtons2.Add(underButton21);
			underButtons2.Add(underButton22);

			underButtons3.Add(underButton31);
			underButtons3.Add(underButton32);
			underButtons3.Add(underButton33);

			underButtons6.Add(underButton61);
			underButtons6.Add(underButton62);

			underButtons.Add(underButtons1);
			underButtons.Add(underButtons2);
			underButtons.Add(underButtons3);
			underButtons.Add(underButtons4);
			underButtons.Add(underButtons5);
			underButtons.Add(underButtons6);

			borders.Add(borderUnder1);
			borders.Add(borderUnder2);
			borders.Add(borderUnder3);
			borders.Add(new Border());
			borders.Add(new Border());
			borders.Add(borderUnder6);

			topButtons[0].Focus();
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

			topButtons[topIndex].Focus();

			if (underVisible)
				borders[topIndex].BorderThickness = new Thickness(2, 2, 2, 2);
			if (underVisible)
			{
				foreach (Button b in underButtons[topIndex])
					b.Visibility = Visibility.Visible;
			}
		}
	}
}
