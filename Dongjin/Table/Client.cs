using System;
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
		// 1. 상호
		public string ClientName { get; set; } = "";
		// 2. 전화번호
		public string Phone { get; set; } = "";
		// 3. 현재미수금
		public int CurrentLeftMoney { get; set; } = 0;
		// 4. 단가(%)구분
		public int PercentCode { get; set; } = 0;
		// 5. 최종거래일
		public DateTime FinalTransactionDate { get; set; } = DateTime.MinValue;
		// 6. 최종입금일
		public DateTime FinalDepositDate { get; set; } = DateTime.MinValue;
		// 7. 최종환입일
		public DateTime FinalRefundDate { get; set; } = DateTime.MinValue;
		// 8. 당일판매액
		public int TodaySellMoney { get; set; } = 0;
		// 9. 당일입금액
		public int TodayDepositMoney { get; set; } = 0;
		// 10. 당일환입액
		public int TodayReturnMoney { get; set; } = 0;
		// 11. 당월판매액
		public int MonthSellMoney { get; set; } = 0;
		// 12. 당월입금액
		public int MonthDepositMoney { get; set; } = 0;
		// 13. 당월환입액
		public int MonthRetundMoney { get; set; } = 0;
	}
}
