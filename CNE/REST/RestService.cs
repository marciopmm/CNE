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
					(string)App.Current.Properties ["SessionID"]);

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
					(string)App.Current.Properties ["SessionID"]);

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

				var response = await client.PostAsync ("Empregado", content);

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

		public async Task<bool> VerificarEmailDisponivel(string email)
		{
			using (HttpClient client = new HttpClient ()) {
				client.BaseAddress = new Uri (Constants.ApiUrl);

				var response = await client.GetAsync ("Usuario/VerificarEmailDisponivel?email=" + email);
				return response.IsSuccessStatusCode;
			}		
		}
	}
}

