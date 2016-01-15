using System;
using Newtonsoft.Json;

namespace CNE
{
	public class Viewport
	{
		[JsonProperty(PropertyName="northeast")]
		public Location Northeast { get; set; }

		[JsonProperty(PropertyName="southwest")]
		public Location Southwest { get; set; }
	}
}

