using System;
using Newtonsoft.Json;

namespace CNE
{
	public class Especialidade
	{
		[JsonProperty(PropertyName="idTipoEspecialidade")]
		public short IdTipoEspecialidade { get; set; }

		[JsonProperty(PropertyName="tipoEspecialidade")]
		public string TipoEspecialidade { get; set; }

		[JsonProperty(PropertyName="idTipoRemuneracao")]
		public short IdTipoRemuneracao { get; set; }

		[JsonProperty(PropertyName="tipoRemuneracao")]
		public string TipoRemuneracao { get; set; }

		[JsonProperty(PropertyName="remuneracao")]
		public decimal Remuneracao { get; set; }

		public Especialidade ()
		{
		}
	}
}

