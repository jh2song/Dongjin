using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dongjin.Table
{
	class Discount
	{
		// PK를 위한 코드
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		// 할인율코드
		public int DiscountCode { get; set; }
		// 할인율명
		public string DiscountName { get; set; }
		// 브랜드코드
		public int BrandCode { get; set; }
		// 브랜드명
		public string BrandName { get; set; }
		// 할인율
		public double DiscountRate { get; set; } = 100.00;
	}
}
