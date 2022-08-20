using SQLite;

namespace Dongjin.Table
{
	class Alarm
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public string AlarmString { get; set; } = "";
	}
}