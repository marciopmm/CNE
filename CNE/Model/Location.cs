using System;
using Newtonsoft.Json;

namespace CNE
{
	public class Location
	{
		[JsonProperty(PropertyName="lat")]
		public double Latitude { get; set; }

		[JsonProperty(PropertyName="lng")]
		public double Longitude { get; set; }
	}
}

