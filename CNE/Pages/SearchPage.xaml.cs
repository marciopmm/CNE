using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Diagnostics;

namespace CNE
{
	public partial class SearchPage : ContentPage
	{
		public SearchPage ()
		{
			InitializeComponent ();
		}

		public SearchPage (string cep, int tipo, int distancia) : this()
		{
			// Chamar os dados da API CNE passando estes 3 parametros 
			var service = new RestService ();
			var empregadosList = service.List (cep, tipo, distancia);

			lstPesquisa.ItemsSource = empregadosList;
			lstPesquisa.ItemTemplate = new DataTemplate (typeof(TextCell));
			lstPesquisa.ItemTemplate.SetBinding (TextCell.TextProperty, "Nome"); 
			lstPesquisa.ItemTemplate.SetBinding (TextCell.DetailProperty, "TextoEspecialidades"); 
			
			lstPesquisa.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) => {
				Empregado empregado = (Empregado)e.SelectedItem;
				await new RestService().SetVisualization(empregado.IdUsuario);
				await Navigation.PushAsync(new DetailsPage(empregado));
			};
		}
		 
	}
}

