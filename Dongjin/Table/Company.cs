﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dongjin.Table
{
	class Company
	{
		[PrimaryKey]
		public int Code { get; set; }
		public string Name { get; set; }
	}
}
