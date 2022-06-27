using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dongjin.Table
{
	class Product
	{
		[PrimaryKey]
		public string Code { get; set; }
		public int CompanyCode { get; set; }
		public string Name { get; set; }
		public int Price { get; set; }
		public int DepositAmount { get; set; }
		public int BoughtMoney { get; set; }
	}
}
