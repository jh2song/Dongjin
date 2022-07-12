﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Dongjin.Table
{
	class Client
	{
		// 거래처코드
		[PrimaryKey]
		public int ClientCode { get; set; }
		// 상호
		public string ClientName { get; set; } = "";
		// 전화번호
		public string Phone { get; set; } = "";
		// 현재미수금
		public int CurrentLeftMoney { get; set; } = 0;
		// 단가(%)구분
		public int PercentCode { get; set; } = 0;
		// 당일판매액
		public int TodaySellMoney { get; set; } = 0;
		// 당일입금액
		public int TodayDepositMoney { get; set; } = 0;
		// 당일환입액
		public int TodayReturnMoney { get; set; } = 0;
		// 당월판매액
		public int MonthSellMoney { get; set; } = 0;
		// 당월입금액
		public int MonthDepositMoney { get; set; } = 0;
		// 당월환입액
		public int MonthReturnMoney { get; set; } = 0;
	}
}
