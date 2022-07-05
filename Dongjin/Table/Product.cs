﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dongjin.Table
{
	class Product
	{
		// 제품코드/바코드
		[PrimaryKey]
		public string ProductCode { get; set; }
		// 회사코드
		public int CompanyCode { get; set; }
		// 제품명
		public string Name { get; set; } = "";
		// 단가
		public int Price { get; set; } = 0;
		// 재고량
		public int LeftAmount { get; set; } = 0;
		// 사입단가
		public int BuyingPrice { get; set; } = 0;
		// 재고총액
		public int TotalDepositMoney { get; set; } = 0;
	}
}
