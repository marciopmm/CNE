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

		public LoginResponse ()
		{
		}
	}
}

