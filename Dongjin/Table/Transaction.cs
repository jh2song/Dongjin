using System;
using System.Collections.Generic;
using System.Text;

namespace Dongjin.Table
{
	class Transaction
	{
		// 전표구분
		public int Choice { get; set; }
		// 처리일
		public DateTime TransactionDate { get; set; }
		// 거래처코드
		public int ClientCode { get; set; }

		// 多
		// 제품코드
		public int ProductCode { get; set; }
		// 수량
		public int ProductCount { get; set; }
		// 금액
		public int DiscountedPrice { get; set; }
		// 정가액
		public int Price { get; set; }
		// 추가옵션 (0: 기본 / 1: 추가1 / 2: 추가2)
		public int AddOption { get; set; }

		// 현재 미수
		public int CurrentLeftMoney { get; set; }
	}
}
