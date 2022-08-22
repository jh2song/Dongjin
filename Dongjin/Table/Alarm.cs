using SQLite;

namespace Dongjin.Table
{
	class Alarm
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string AlarmString1 { get; set; } = "";
		public string AlarmString2 { get; set; } = "";
	}
}