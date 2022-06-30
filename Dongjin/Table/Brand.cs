using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dongjin.Table
{
	class Brand
	{
		[PrimaryKey]
		public int BrandCode { get; set; }
		public string BrandName { get; set; }
	}
}
