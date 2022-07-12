using System;
using System.Collections.Generic;
using System.Text;

namespace Dongjin.Table
{
	class LeftMoneyLedger
	{
		// 처리일자
		public DateTime NowDate { get; set; } = DateTime.MinValue;
		// 거래처코드
		public int ClientCode { get; set; }

		// 거래일자
		public DateTime TransactionDate { get; set; }
		// 전일 미수
		public int YesterdayLeftMoney { get; set; }
		// 금일 판매
		public int TodaySellMoney { get; set; }
		// 금일 환입
		public int TodayRefundMoney { get; set; }
		// 금일 입금
		public int TodayDepositMoney { get; set; }
		// 현재 미수
		public int CurrentLeftMoney { get; set; }
		// 금액
		public int Price { get; set; }
	}
}
