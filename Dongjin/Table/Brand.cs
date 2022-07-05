using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dongjin.Table
{
	class Brand : IEquatable<Brand>, IComparable<Brand>
	{
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			Brand objAsBrand = obj as Brand;
			if (objAsBrand == null) return false;
			else return Equals(objAsBrand);
		}

		public override int GetHashCode()
		{
			return BrandCode;
		}

		public override string ToString()
		{
			return "ToString isn't recommended";
		}

		public bool Equals(Brand other)
		{
			if (other == null) return false;
			return (this.BrandCode.Equals(other.BrandCode));
		}

		// Default comparer for Part type.
		public int CompareTo(Brand compareBrand)
		{
			// A null value means that this object is greater.
			if (compareBrand == null)
				return 1;

			else
				return this.BrandCode.CompareTo(compareBrand.BrandCode);
		}


		[PrimaryKey]
		public int BrandCode { get; set; }
		public string BrandName { get; set; }
		public double BuyingPercent { get; set; } = 100.00;
	}
}
