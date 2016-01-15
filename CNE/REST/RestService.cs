using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
			client.BaseAddress = new Uri( ConfigReader.GetGoogleGeocodingApiUrl ());

			var response = await client.GetAsync ("?address=" +
			               cep + "&key=" +
			               ConfigReader.GetGoogleApiKey () +
			               "&callback");

			var json = response.Content.ReadAsStringAsync ().Result;
			var result = JsonConvert.DeserializeObject<GoogleGeocodingResponse> (json);

			return result;
		}

		public async Task<IEnumerable<Empregado>> List(string cep, string tipo, int distancia)
		{
			var client = new HttpClient ();
			client.BaseAddress = new Uri( Constants.ApiUrl );

			var response = await client.GetAsync ("?cep=" +
				cep + "&tipoEspecialidade=" + tipo +
				"&distancia=" + distancia.ToString());

			var json = response.Content.ReadAsStringAsync ().Result;
			var result = JsonConvert.DeserializeObject<IEnumerable<Empregado>> (json);

			return result;
		}
	}
}

