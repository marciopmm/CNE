using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Diagnostics;

namespace CNE
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();

			pckTipo.Items.Add ("Arrumadeira");
			pckTipo.Items.Add ("Babá");
			pckTipo.Items.Add ("Babá Folguista");
			pckTipo.Items.Add ("Caseiro");
			pckTipo.Items.Add ("Copeira");
			pckTipo.Items.Add ("Cozinheira");
			pckTipo.Items.Add ("Cuidador de Animais");
			pckTipo.Items.Add ("Cuidador de Idosos");
			pckTipo.Items.Add ("Diarista");
			pckTipo.Items.Add ("Empregada Doméstica");
			pckTipo.Items.Add ("Governanta / Mordomo");
			pckTipo.Items.Add ("Home Organizer");
			pckTipo.Items.Add ("Motorista");

			pckDistancia.Items.Add ("5");
			pckDistancia.Items.Add ("10");
			pckDistancia.Items.Add ("25");
			pckDistancia.Items.Add ("50");
			pckDistancia.Items.Add ("100");

			btnSair.Clicked += async (sender, e) => {
				App.Current.Logout();
			};

			btnProcurar.Clicked += async (object sender, EventArgs e) => {
				bool valid = true;

				try
				{
					if (string.IsNullOrWhiteSpace(txtCep.Text) || txtCep.Text.Length < 8)
					{
						txtCep.BackgroundColor = Color.FromHex("FFFFBB");
						valid = false;
					}
					else
					{
						txtCep.BackgroundColor = Color.Default;
					}

					if (pckTipo.SelectedIndex < 0)
					{
						pckTipo.BackgroundColor = Color.FromHex("FFFFBB");
						valid = false;
					}
					else
					{
						pckTipo.BackgroundColor = Color.Default;
					}

					if (pckDistancia.SelectedIndex < 0)
					{
						pckDistancia.BackgroundColor = Color.FromHex("FFFFBB");
						valid = false;
					}
					else
					{
						pckDistancia.BackgroundColor = Color.Default;
					}

					if (valid) 
					{
						int tipo = 0;

						switch(pckTipo.Items[pckTipo.SelectedIndex])
						{
						case "Arrumadeira":
							tipo = 1;
							break;
						case "Babá":
							tipo = 2;
							break;
						case "Babá Folguista":
							tipo = 3;
							break;
						case "Caseiro":
							tipo = 4;
							break;
						case "Copeira":
							tipo = 5;
							break;
						case "Cozinheira":
							tipo = 6;
							break;
						case "Cuidador de Animais":
							tipo = 7;
							break;
						case "Cuidador de Idosos":
							tipo = 8;
							break;
						case "Diarista":
							tipo = 9;
							break;
						case "Empregada Doméstica":
							tipo = 10;
							break;
						case "Governanta / Mordomo":
							tipo = 11;
							break;
						case "Home Organizer":
							tipo = 12;
							break;
						case "Motorista":
							tipo = 13;
							break;
						}

						await Navigation.PushAsync(new SearchPage(txtCep.Text, tipo, 
							int.Parse(pckDistancia.Items[pckDistancia.SelectedIndex])));
					}
				}
				catch(Exception ex)
				{
					DisplayAlert("Desculpe ... :(", "Ocorreu um problema durante a solicitação de dados. A pesquisa não pode ser realizada.", "OK");
					Debug.WriteLine(ex.ToString());
				}
			};
		}

	}
}

