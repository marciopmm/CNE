using System;
using Newtonsoft.Json;

namespace CNE
{
	public class Usuario
	{
		[JsonProperty(PropertyName="idUsuario")]
		public uint IdUsuario {get;set;}

		[JsonProperty(PropertyName="nome")]
		public string Nome {get;set;}

		[JsonProperty(PropertyName="sobrenome")]
		public string Sobrenome {get;set;}

		[JsonProperty(PropertyName="sexo")]
		public char Sexo {get;set;}

		[JsonProperty(PropertyName="cpfCnpj")]
		public string Cpf {get;set;}

		[JsonProperty(PropertyName="email")]
		public string Email {get;set;}

		[JsonProperty(PropertyName="senha")]
		public string Senha {get;set;}

		[JsonProperty(PropertyName="dtNascimento")]
		public DateTime DataNascimento {get;set;}

		[JsonProperty(PropertyName="idPerfil")]
		public byte IdPerfil {get;set;}

		public Usuario ()
		{
		}
	}
}

