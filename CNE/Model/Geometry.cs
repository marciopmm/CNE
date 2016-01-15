using System;
using Newtonsoft.Json;

namespace CNE
{
	public class Geometry
	{
		[JsonProperty(PropertyName="location")]
		public Location Location { get; set; }

		[JsonProperty(PropertyName="location_type")]
		public string LocationType { get; set; }

		[JsonProperty(PropertyName="viewport")]
		public Viewport Viewport { get; set; }
	}
}

