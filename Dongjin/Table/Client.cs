using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Dongjin.Table
{
	class Client
	{
		[PrimaryKey]
		public int Code { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public int CurrentLeftMoney { get; set; }
		public int PercentCode { get; set; }
		public DateTime LastTransactionDate { get; set; }
		public DateTime LastMoneyComeDate { get; set; }
		public DateTime LastReturnDate { get; set; }
		public int TodaySellMoney { get; set; }
		public int TodayDepositMoney { get; set; }
		public int TodayReturnMoney { get; set; }
	}
}
