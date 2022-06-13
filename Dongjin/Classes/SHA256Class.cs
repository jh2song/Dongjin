using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Dongjin.Classes
{
	class SHA256Class
	{
		public static string SHA256Hash(string data)
		{
			SHA256 sha = new SHA256Managed();
			byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in hash)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}
	}
}
