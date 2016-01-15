using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CNE
{
	public class GoogleGeocodingResponse
	{
		[JsonProperty(PropertyName="results")]
		public IEnumerable<GoogleAddress> Results { get; set; }

		[JsonProperty(PropertyName="status")]
		public string Status { get; set; }
	}
}

