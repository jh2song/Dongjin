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
		public string Name { get; set; } = "";
		public string Phone { get; set; } = "";
		public int CurrentLeftMoney { get; set; } = 0;
		public int PercentCode { get; set; } = 0;
		public DateTime LastTransactionDate { get; set; } = DateTime.MinValue;
		public DateTime LastMoneyComeDate { get; set; } = DateTime.MinValue;
		public DateTime LastReturnDate { get; set; } = DateTime.MinValue;
		public int TodaySellMoney { get; set; } = 0;
		public int TodayDepositMoney { get; set; } = 0;
		public int TodayReturnMoney { get; set; } = 0;

		public static implicit operator Client(List<Client> v)
		{
			throw new NotImplementedException();
		}
	}
}
