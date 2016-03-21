using System;
using Newtonsoft.Json;

namespace CNE
{
	public class LoginResponse
	{
		[JsonProperty(PropertyName="token")]
		public string Token { get; set; }

		[JsonProperty(PropertyName="isEmpregado")]
		public bool IsEmpregado { get; set; }

		[JsonProperty(PropertyName="alterarSenha")]
		public bool AlterarSenha { get; set; }

		[JsonProperty(PropertyName="qtdVisualizacoes")]
		public int QtdVisualizacoes { get; set; }

		public LoginResponse ()
		{
		}
	}
}

