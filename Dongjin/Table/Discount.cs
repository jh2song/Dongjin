using SQLite;
using System;

namespace Dongjin.Table
{
	class Discount : IEquatable<Discount>, IComparable<Discount>
	{
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			Discount objAsDiscount = obj as Discount;
			if (objAsDiscount == null) return false;
			else return Equals(objAsDiscount);
		}

		public override int GetHashCode()
		{
			return BrandCode;
		}

		public override string ToString()
		{
			return "ToString isn't recommended";
		}

		public bool Equals(Discount other)
		{
			if (other == null) return false;
			return (this.BrandCode.Equals(other.BrandCode));
		}

		// Default comparer for Part type.
		public int CompareTo(Discount compareDiscount)
		{
			// A null value means that this object is greater.
			if (compareDiscount == null)
				return 1;

			else
				return this.BrandCode.CompareTo(compareDiscount.BrandCode);
		}

		// PK를 위한 코드
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		// 할인율코드
		public int DiscountCode { get; set; }
		// 할인율명
		public string DiscountName { get; set; }
		// 브랜드코드
		public int BrandCode { get; set; }
		// 브랜드명
		public string BrandName { get; set; }
		// 할인율
		public double DiscountRate { get; set; } = 100.00;
	}
}
