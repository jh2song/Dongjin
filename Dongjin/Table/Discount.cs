using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dongjin.Table
{
	class Discount
	{
		// 할인율코드
		[PrimaryKey]
		public int Code { get; set; }
		// 할인율명
		public int Name { get; set; }
		
	}
}
