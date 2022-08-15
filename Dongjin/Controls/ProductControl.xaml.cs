using Dongjin.Table;
using System.Windows;
using System.Windows.Controls;

namespace Dongjin.Controls
{
	/// <summary>
	/// Interaction logic for ProductControl.xaml
	/// </summary>
	public partial class ProductControl : UserControl
	{
		public Product Product
		{
			get { return (Product)GetValue(ProductProperty); }
			set { SetValue(ProductProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Contact.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ProductProperty =
			DependencyProperty.Register("Product", typeof(Product), typeof(ProductControl), new PropertyMetadata(new Product() {
				BrandCode = 1234,
				BuyingPrice = 0,
				LeftAmount = 0,
				Price = 0,
				ProductCode = "1234",
				ProductName = "테스트",
				TotalDepositMoney = 0,
			}, SetText));

		private static void SetText(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ProductControl control = d as ProductControl;

			if (control != null)
			{
				control.ProductCodeLB.Content = "제품코드/바코드: " + (e.NewValue as Product).ProductCode;
				control.ProductNameLB.Content = "제품명: " + (e.NewValue as Product).ProductName;
			}
		}

		public ProductControl()
		{
			InitializeComponent();
		}
	}
}
