using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CNE
{
	public class GoogleAddress
	{
		[JsonProperty(PropertyName="address_components")]
		public IEnumerable<AddressComponent> AddressComponents { get; set; }

		[JsonProperty(PropertyName="formatted_address")]
		public string FormattedAddress { get; set; }

		[JsonProperty(PropertyName="geometry")]
		public Geometry Geometry { get; set; }

		[JsonProperty(PropertyName="place_id")]
		public string PlaceId { get; set; }

		[JsonProperty(PropertyName="types")]
		public IEnumerable<string> Types { get; set; }
	}
}

