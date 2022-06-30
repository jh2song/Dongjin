using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dongjin.Table
{
	class Discount
	{
		// 할인율코드
		[Indexed(Name = "CompositeKey", Order = 1, Unique = true)]
		public int DiscountCode { get; set; }
		// 할인율명
		public string DiscountName { get; set; }
		// 브랜드코드
		[Indexed(Name = "CompositeKey", Order = 2, Unique = true)]
		public int BrandCode { get; set; }
		// 브랜드명
		public string BrandName { get; set; }
		// 할인율
		public double DiscountRate { get; set; } = 100.00;
	}
}
