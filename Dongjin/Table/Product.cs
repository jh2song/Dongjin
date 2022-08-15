using SQLite;

namespace Dongjin.Table
{
	public class Product
	{
		// 제품코드/바코드
		[PrimaryKey]
		public string ProductCode { get; set; }
		// 브랜드코드
		public int BrandCode { get; set; }
		// 제품명
		public string ProductName { get; set; } = "";
		// 단가
		public int Price { get; set; } = 0;
		// 재고량
		public int LeftAmount { get; set; } = 0;
		// 사입단가
		public int BuyingPrice { get; set; } = 0;
		// 재고총액
		public int TotalDepositMoney { get; set; } = 0;
	}
}
