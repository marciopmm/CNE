using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CNE
{
	public partial class EvaluationPage : ContentPage
	{
		private Empregado _empregado;

		public EvaluationPage (Empregado empregado)
		{
			InitializeComponent ();

			_empregado = empregado;

			lblNome.Text = empregado.Nome;
			sldEstrelas.Minimum = 0;
			sldEstrelas.Maximum = 5;
			sldEstrelas.Value = empregado.Estrelas;
			imgEstrelas.Source = string.Format ("star{0}.png", empregado.Estrelas);
			//TODO: Obter avaliação anterior, se existente.

			btnEnviar.Clicked += async (sender, e) => {
				//TODO: Chamar API para enviar avaliação

				DisplayAlert("Pronto!", "Sua avaliação foi enviada com sucesso!", "OK");
				await Navigation.PopAsync();
			};

			sldEstrelas.ValueChanged += (sender, e) => {
				imgEstrelas.Source = string.Format ("star{0}.png", e.NewValue.ToString("N0"));
			};
		}
	}
}

