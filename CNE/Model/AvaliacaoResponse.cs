using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CNE
{
	public class AvaliacaoResponse
	{
		[JsonProperty(PropertyName="qtdAvaliacoes")]
		public int QtdAvaliacoes { get; set;}

		[JsonProperty(PropertyName="estrelas")]
		public byte Estrelas { get; set; }

		[JsonProperty(PropertyName="comentarios")]
		public IEnumerable<string> Comentarios{ get; set; }

		public AvaliacaoResponse ()
		{
		}
	}
}

