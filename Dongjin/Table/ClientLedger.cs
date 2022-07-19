using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dongjin.Table
{
	class ClientLedger
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		// 거래처코드
		public int ClientCode { get; set; }
		// 거래일자
		public DateTime TransactionDate { get; set; } = DateTime.MinValue;
		
		// 전일 미수
		// --> DB 저장 X 직접 계산

		// 금일 판매
		public int TodaySellMoney { get; set; } = 0;
		// 금일 환입
		public int TodayRefundMoney { get; set; } = 0;
		// 금일 입금
		public int TodayDepositMoney { get; set; } = 0;
		// 현재 미수
		public int CurrentLeftMoney { get; set; } = 0;
		// 금액
		// --> 의미 없어서 필요 없음
	}
}
