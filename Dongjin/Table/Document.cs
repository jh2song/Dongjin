﻿using SQLite;
using System;

namespace Dongjin.Table
{
	public class Document
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		// 전표구분 1. 출고 / 2. 덤 / 3. 환입
		public int Choice { get; set; }
		// 처리일
		public DateTime TransactionDate { get; set; }
		// 거래처코드
		public int ClientCode { get; set; }

		// 多
		// 제품코드/바코드
		public string ProductCode { get; set; }
		// 제품명
		public string ProductName { get; set; }
		// 수량
		public int ProductCount { get; set; }
		// 금액
		public int DiscountPrice { get; set; }
		// 정가액
		public int Price { get; set; }
		// 추가옵션 (0: 기본 / 1: 추가1 / 2: 추가2)
		public int AppendOption0 { get; set; } = 0;
		public int AppendOption1 { get; set; } = 0;
		public int AppendOption2 { get; set; } = 0;

		// 현재 미수
		
	}
}
