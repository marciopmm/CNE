using System;
using Newtonsoft.Json;

namespace CNE
{
	public class Endereco
	{
		[JsonProperty(PropertyName="idEndereco")]
		public uint IdEndereco { get; set; }

		[JsonProperty(PropertyName="logradouro")]
		public string Logradouro { get; set; }

		[JsonProperty(PropertyName="numero")]
		public string Numero { get; set; }

		[JsonProperty(PropertyName="complemento")]
		public string Complemento { get; set; }

		[JsonProperty(PropertyName="bairro")]
		public string Bairro { get; set; }

		[JsonProperty(PropertyName="cidade")]
		public string Cidade { get; set; }

		[JsonProperty(PropertyName="estado")]
		public string Estado { get; set; }

		[JsonProperty(PropertyName="cep")]
		public string Cep { get; set; }

		[JsonProperty(PropertyName="latitude")]
		public double Latitude { get; set; }

		[JsonProperty(PropertyName="longitude")]
		public double Longitude { get; set; }	

	}
}

