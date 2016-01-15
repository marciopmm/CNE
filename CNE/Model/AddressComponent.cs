using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CNE
{
	public class AddressComponent
	{
		[JsonProperty(PropertyName="long_name")]
		public string LongName { get; set; }

		[JsonProperty(PropertyName="short_name")]
		public string ShortName { get; set; }

		[JsonProperty(PropertyName="types")]
		public IEnumerable<string> Types { get; set; }
	}
}

