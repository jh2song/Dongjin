using System;
using System.Collections.Generic;
using System.Text;

namespace Dongjin.Table
{
	class Transaction
	{
		public int Choice { get; set; }
		public DateTime TransactionDate { get; set; }
		public int ClientCode { get; set; }

		public int ProductCode { get; set; }
		public int ProductCount { get; set; }
		public int DiscountedPrice { get; set; }
		public int Price { get; set; }
		public int AddOption { get; set; }
		public int CurrentLeftMoney { get; set; }
	}
}
