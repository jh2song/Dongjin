using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Dongjin.Classes
{
	class RegexClass
	{
		static public bool NotNumericBackspace(string str)
		{
			Regex regex = new Regex("[^0-9\b]+");
			return regex.IsMatch(str);
		}

		static public bool NotNumericBackspaceComma(string str)
		{
			Regex regex = new Regex("[^0-9\b,]+");
			return regex.IsMatch(str);
		}

		static public bool NotUnderboundNumber(string str, int start, int end)
		{
			string regexPattern = "";
			for (int i = start; i <= end; i++)
				regexPattern += i.ToString();

			Regex regex = new Regex($"[^{regexPattern}\b]+");
			return regex.IsMatch(str);
		}

		internal static bool NotNumericBackspaceDot(string str)
		{
			Regex regex = new Regex("[^0-9\b.]+");
			return regex.IsMatch(str);
		}
	}
}
