using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;

namespace CNE
{
	public class RestService
	{
		public RestService ()
		{
		}

		public async Task<GoogleGeocodingResponse> GetAddressFromGoogle(string cep)
		{
			var client = new HttpClient ();
			client.BaseAddress = new Uri( Constants.GoogleGeocodingApiUrl );

			var response = await client.GetAsync ("?address=" +
			               cep + "&key=" +
			               Constants.GoogleApiKey +
			               "&callback");

			var json = response.Content.ReadAsStringAsync ().Result;
			var result = JsonConvert.DeserializeObject<GoogleGeocodingResponse> (json);

			return result;
		}

		public IEnumerable List(string cep, int tipo, int distancia)
		{
			using (var client = new HttpClient ()) {
				client.BaseAddress = new Uri (Constants.ApiUrl);

				var response = client.GetStringAsync ("Empregado?cep=" +
				              cep + "&tipo=" + tipo.ToString () +
				              "&distancia=" + distancia.ToString ()).Result;

				string arrayString = Regex.Match (response, "\"results\": ?(\\[.*\\])").Groups [1].Value;

				//var json = response.Content.ReadAsStringAsync ().Result;
				var result = JsonConvert.DeserializeObject<IEnumerable<Empregado>> (arrayString);

				return result;
			}
		}

		public async Task RegisterEmployerAsync(Usuario user)
		{
			string json = JsonConvert.SerializeObject (user);
			StringContent content = new StringContent (json, Encoding.UTF8, "application/json");

			using (HttpClient client = new HttpClient ()) {
				client.BaseAddress = new Uri(Constants.ApiUrl);

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
	}
}

