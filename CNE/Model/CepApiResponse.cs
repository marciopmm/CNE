using System;
using Newtonsoft.Json;

namespace CNE
{
	public class CepApiResponse
	{
		[JsonProperty(PropertyName="status")]
		public bool Ativo { get; set;}

		[JsonProperty(PropertyName="code")]
		public string Cep { get; set;}

		[JsonProperty(PropertyName="state")]
		public string UF { get; set;}

		[JsonProperty(PropertyName="city")]
		public string Cidade { get; set;}

		[JsonProperty(PropertyName="district")]
		public string Bairro { get; set;}

		[JsonProperty(PropertyName="address")]
		public string Logradouro { get; set;}

		public CepApiResponse ()
		{			
		}
	}
}

