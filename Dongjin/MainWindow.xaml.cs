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
		public MainWindow()
		{
			InitializeComponent();

			/* 참고
			/* https://post.naver.com/viewer/postView.nhn?volumeNo=6903303&memberNo=11439725
			*/
			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 1);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object sender, EventArgs e)
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
	}
}
