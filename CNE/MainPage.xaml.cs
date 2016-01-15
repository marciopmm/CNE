using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CNE
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();

			pckTipo.Items.Add (new ListItem(1, "Arrumadeira"));
			pckTipo.Items.Add (new ListItem(1, "Babá"));
			pckTipo.Items.Add (new ListItem(1, "Babá Folguista"));
			pckTipo.Items.Add (new ListItem(1, "Caseiro"));
			pckTipo.Items.Add (new ListItem(1, "Copeira"));
			pckTipo.Items.Add (new ListItem(1, "Cozinheira"));
			pckTipo.Items.Add (new ListItem(1, "Cuidador de Animais"));
			pckTipo.Items.Add (new ListItem(1, "Cuidador de Idosos"));
			pckTipo.Items.Add (new ListItem(1, "Diarista"));
			pckTipo.Items.Add (new ListItem(1, "Empregada Doméstica"));
			pckTipo.Items.Add (new ListItem(1, "Governanta / Mordomo"));
			pckTipo.Items.Add (new ListItem(1, "Home Organizer"));
			pckTipo.Items.Add (new ListItem(1, "Motorista"));

			pckDistancia.Items.Add ("5");
			pckDistancia.Items.Add ("10");
			pckDistancia.Items.Add ("25");
			pckDistancia.Items.Add ("50");
			pckDistancia.Items.Add ("100");

			btnProcurar.Clicked += async (object sender, EventArgs e) => {
				bool valid = true;

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
					await Navigation.PushAsync(new SearchPage(
												txtCep.Text, 
						pckTipo.Items[pckTipo.SelectedIndex], 
						int.Parse(pckDistancia.Items[pckDistancia.SelectedIndex])));
				}
			};
		}

	}
}

