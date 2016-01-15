using System;

namespace CNE
{
	public class ListItem
	{
		public int ID { get; set; }
		public string Value { get; set; }

		public ListItem ()
		{
		}

		public ListItem(int id, string value)
		{
			ID = id;
			Value = value;
		}
	}
}

