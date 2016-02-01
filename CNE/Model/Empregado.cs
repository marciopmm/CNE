using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CNE
{
	public class Empregado : Usuario
	{
		[JsonProperty(PropertyName="idEmpregado")]
		public uint IdEmpregado { get; set; }

		[JsonProperty(PropertyName="aceitaDormir")]
		public bool AceitaDormir { get; set; }

		[JsonProperty(PropertyName="qtdAvaliacoes")]
		public int QtdAvalicaoes { get; set;}

		[JsonProperty(PropertyName="estrelas")]
		public byte Estrelas { get; set;}

		[JsonProperty(PropertyName="endereco")]
		public Endereco Endereco {get;set;}

		[JsonProperty(PropertyName="telCelular")]
		public string TelCelular {get;set;}

		[JsonProperty(PropertyName="telResidencial")]
		public string TelResidencial {get;set;}

		[JsonProperty(PropertyName="especialidades")]
		public IEnumerable<Especialidade> Especialidades {get;set;}

		[JsonProperty(PropertyName="comentarios")]
		public IEnumerable<string> Comentarios { get; set; }

		[JsonProperty(PropertyName="textoEspecialidades")]
		public string TextoEspecialidades { get; set; }

		[JsonProperty(PropertyName="sobre")]
		public string Sobre { get; set; }

		public Empregado()
		{
			Especialidades = new List<Especialidade> ();
			Comentarios = new List<string> ();
		}
	}
}

