using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using System.Net.Http.Headers;

namespace CNE
{
	public class RestService
	{
		public RestService ()
		{
		}

		public LoginResponse Login (string email, string pwd)
		{
			string jsonData = "{ \"email\": \"" + email + "\", \"senha\": \"" + pwd + "\" }";
			StringContent content = new StringContent (jsonData, Encoding.UTF8, "application/json");

			var client = new HttpClient ();
			client.BaseAddress = new Uri( Constants.ApiUrl );

			var response = client.PostAsync ("Sessao", content).Result;

			var json = response.Content.ReadAsStringAsync ().Result;
			var result = JsonConvert.DeserializeObject<LoginResponse> (json);

			return result;
		}

		public async Task<CepApiResponse> GetAddressFromApi(string cep)
		{
			var client = new HttpClient ();
			client.BaseAddress = new Uri( Constants.CepApiUrl );

			var response = await client.GetAsync ("cep.json?code=" + cep);

			var json = await response.Content.ReadAsStringAsync ();
			var result = JsonConvert.DeserializeObject<CepApiResponse> (json);

			return result;
		}

		public bool CheckSession(string sessionID)
		{
			using (var client = new HttpClient ()) {
				client.BaseAddress = new Uri (Constants.ApiUrl);
				var response = client.GetAsync ("Sessao/" + sessionID).Result;

				return response.IsSuccessStatusCode;
			}
		}

		public IEnumerable List(string cep, int tipo, int distancia)
		{
			using (var client = new HttpClient ()) {
				client.BaseAddress = new Uri (Constants.ApiUrl);
				client.DefaultRequestHeaders.Clear ();
				client.DefaultRequestHeaders.Add("X-Auth-Token", 
					((LoginResponse)App.Current.Properties ["Session"]).Token);

				var response = client.GetAsync ("Empregado?cep=" +
				              cep + "&tipo=" + tipo.ToString () +
					"&distancia=" + distancia.ToString ()).Result;

				if (response.IsSuccessStatusCode) {
					string strResponse = response.Content.ReadAsStringAsync ().Result;
					string arrayString = Regex.Match (strResponse, "\"results\": ?(\\[.*\\])").Groups [1].Value;

					//var json = response.Content.ReadAsStringAsync ().Result;
					var result = JsonConvert.DeserializeObject<IEnumerable<Empregado>> (arrayString);

					return result;
				} else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
					App.Current.Logout ();
				}

				return null;
			}
		}

		public async Task RegisterEmployerAsync(Usuario user)
		{
			string json = JsonConvert.SerializeObject (user);
			StringContent content = new StringContent (json, Encoding.UTF8, "application/json");

			using (HttpClient client = new HttpClient ()) {
				client.BaseAddress = new Uri(Constants.ApiUrl);
				client.DefaultRequestHeaders.Clear ();
				client.DefaultRequestHeaders.Add("X-Auth-Token", 
					((LoginResponse)App.Current.Properties ["Session"]).Token);

				var response = await client.PostAsync ("Usuario", content);

				if (!response.IsSuccessStatusCode) {
					string strContent = await response.Content.ReadAsStringAsync ();

					Regex rxMessage = new Regex ("\"Message\": ?\"([^\"]+)\"", RegexOptions.IgnoreCase);
					Match m = rxMessage.Match (strContent);

					if (m.Success)
						throw new Exception (m.Groups[1].Value);
					else
						throw new Exception (response.StatusCode.ToString());
				}
			}
		}

		public async Task RegisterEmployeeAsync(Empregado empregado)
		{
			string json = JsonConvert.SerializeObject (empregado);
			StringContent content = new StringContent (json, Encoding.UTF8, "application/json");

			using (HttpClient client = new HttpClient ()) {
				client.BaseAddress = new Uri(Constants.ApiUrl);

				if (empregado.IdEmpregado > 0)
					client.DefaultRequestHeaders.Add ("X-Auth-Token", 
						((LoginResponse)App.Current.Properties ["Session"]).Token);

				var response = empregado.IdEmpregado == 0 ?
								await client.PostAsync ("Empregado", content) :
								await client.PutAsync ("Empregado", content);
						
				if (!response.IsSuccessStatusCode) {
					string strContent = await response.Content.ReadAsStringAsync ();

					Regex rxMessage = new Regex ("\"Message\": ?\"([^\"]+)\"", RegexOptions.IgnoreCase);
					Match m = rxMessage.Match (strContent);

					if (m.Success)
						throw new Exception (m.Groups[1].Value);
					else
						throw new Exception (response.StatusCode.ToString());
				}
			}
		}

		public Empregado GetEmployee(string token)
		{
			using (HttpClient client = new HttpClient ()) {
				client.BaseAddress = new Uri(Constants.ApiUrl);
				client.DefaultRequestHeaders.Clear ();
				client.DefaultRequestHeaders.Add("X-Auth-Token", token);
				
				var response = client.GetAsync ("Empregado").Result;
				string strContent = response.Content.ReadAsStringAsync ().Result;

				if (!response.IsSuccessStatusCode) {
					Regex rxMessage = new Regex ("\"Message\": ?\"([^\"]+)\"", RegexOptions.IgnoreCase);
					Match m = rxMessage.Match (strContent);

					if (m.Success)
						throw new Exception (m.Groups [1].Value);
					else
						throw new Exception (response.StatusCode.ToString ());
				}

				return JsonConvert.DeserializeObject<Empregado> (strContent);
			}
		}

		public async Task<bool> VerificarEmailDisponivel(string email)
		{
			using (HttpClient client = new HttpClient ()) {
				client.BaseAddress = new Uri (Constants.ApiUrl);

				var response = await client.GetAsync ("Usuario/VerificarEmailDisponivel?email=" + email.ToLower());
				return response.IsSuccessStatusCode;
			}		
		}

		public async Task<ChangePasswordResponse> ChangePassword(string email)
		{
			using (HttpClient client = new HttpClient ()) {
				client.BaseAddress = new Uri (Constants.ApiUrl);

				var response = await client.GetAsync ("Usuario/RecuperarSenha?email=" + email.ToLower());
				ChangePasswordResponse result = new ChangePasswordResponse();

				if (response.IsSuccessStatusCode) {
					result.IsSuccess = true;
				} else {
					result.IsSuccess = false;

					switch (response.StatusCode) {
						case System.Net.HttpStatusCode.NotFound:
							result.Message = "O email não foi encontrado em nossos registros. Por favor, verifique a ortografia.";
							break;
						case System.Net.HttpStatusCode.InternalServerError:
							result.Message = "Não foi possível realizar sua solicitação devido a um problema no servidor. Entre em contato com nossa equipe através do email 'suporte@cne.net.br'";
							break;
						default: 
						result.Message = "Não foi possível realizar sua solicitação devido a um problema não identificado. Entre em contato com nossa equipe através do email 'suporte@cne.net.br'";
						break;
					}
				}

				return result;
			}	
		}

		public async Task<AvaliacaoResponse> SendEvaluation(uint empregadoId, bool contrataria, int estrelas, string comentario)
		{
			string json = string.Format ("{{ \"idEmpregado\": {0}, \"contratariaNovamente\": \"{1}\", \"estrelas\": {2}, \"comentario\": \"{3}\" }}",
				empregadoId,
				contrataria,
				estrelas,
				comentario);
			StringContent content = new StringContent (json, Encoding.UTF8, "application/json");

			using (HttpClient client = new HttpClient ()) {
				client.BaseAddress = new Uri(Constants.ApiUrl);
				client.DefaultRequestHeaders.Clear ();
				client.DefaultRequestHeaders.Add("X-Auth-Token",
					((LoginResponse)App.Current.Properties ["Session"]).Token);

				var response = await client.PostAsync ("Avaliacao", content);
				string strContent = await response.Content.ReadAsStringAsync ();

				if (!response.IsSuccessStatusCode) {
					Regex rxMessage = new Regex ("\"Message\": ?\"([^\"]+)\"", RegexOptions.IgnoreCase);
					Match m = rxMessage.Match (strContent);

					if (m.Success)
						throw new Exception (m.Groups [1].Value);
					else
						throw new Exception (response.StatusCode.ToString ());
				} else {					
					return JsonConvert.DeserializeObject<AvaliacaoResponse>(strContent);
				}
			}
		}

		public async Task SetNewPassword(string pwd)
		{
			string json = string.Format ("{{ \"senha\": \"{0}\" }}", pwd);				
			StringContent content = new StringContent (json, Encoding.UTF8, "application/json");

			using (HttpClient client = new HttpClient ()) {
				client.BaseAddress = new Uri(Constants.ApiUrl);
				client.DefaultRequestHeaders.Clear ();
				client.DefaultRequestHeaders.Add("X-Auth-Token",
					((LoginResponse)App.Current.Properties ["Session"]).Token);

				var response = await client.PutAsync ("Usuario/NovaSenha", content);
				string strContent = await response.Content.ReadAsStringAsync ();

				if (!response.IsSuccessStatusCode) {
					Regex rxMessage = new Regex ("\"Message\": ?\"([^\"]+)\"", RegexOptions.IgnoreCase);
					Match m = rxMessage.Match (strContent);

					if (m.Success)
						throw new Exception (m.Groups [1].Value);
					else
						throw new Exception (response.StatusCode.ToString ());
				}
			}
		}

		public async Task SetVisualization(uint idUsuario)
		{
			string json = string.Format ("{{ \"idUsuario\": \"{0}\" }}", idUsuario);				
			StringContent content = new StringContent (json, Encoding.UTF8, "application/json");

			using (HttpClient client = new HttpClient ()) {
				client.BaseAddress = new Uri(Constants.ApiUrl);
				client.DefaultRequestHeaders.Clear ();
				client.DefaultRequestHeaders.Add("X-Auth-Token",
					((LoginResponse)App.Current.Properties ["Session"]).Token);

				var response = await client.PostAsync ("Usuario/SalvarNovaVisualizacao", content);
				string strContent = await response.Content.ReadAsStringAsync ();

				if (!response.IsSuccessStatusCode) {
					Regex rxMessage = new Regex ("\"Message\": ?\"([^\"]+)\"", RegexOptions.IgnoreCase);
					Match m = rxMessage.Match (strContent);

					if (m.Success)
						throw new Exception (m.Groups [1].Value);
					else
						throw new Exception (response.StatusCode.ToString ());
				}
			}
		}
	}
}

