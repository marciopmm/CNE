using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CNE
{
	public partial class SearchPage : ContentPage
	{
		decimal _latitude, _logitude;

		public SearchPage ()
		{
			InitializeComponent ();
		}

		public SearchPage (string cep, string tipo, int distancia) : this()
		{
			// Chamar os dados da API CNE passando estes 3 parametros 
			var service = new RestService();
			var list = service.List(cep, tipo, distancia);

			lstPesquisa.ItemsSource = list;
			lstPesquisa.ItemTemplate = new DataTemplate (typeof(TextCell));
			lstPesquisa.ItemTemplate.SetBinding (TextCell.TextProperty, "Nome"); 
			lstPesquisa.ItemTemplate.SetBinding (TextCell.DetailProperty, "ListaEspecialidades"); 
		}
	}
}

