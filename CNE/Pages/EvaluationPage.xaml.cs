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
			imgEstrelas.Source = string.Format ("star{0:N0}.png", empregado.Estrelas);
			//TODO: Obter avaliação anterior, se existente.

			btnEnviar.Clicked += async (sender, e) => {
				try 
				{
					var rest = new RestService();
					var result = await rest.SendEvaluation(
						empregado.IdEmpregado,
						swtContrataria.IsToggled,
						Convert.ToInt32(sldEstrelas.Value),
						txtComentario.Text);

					empregado.Estrelas = result.Estrelas;
					empregado.QtdAvalicaoes = result.QtdAvaliacoes;
					empregado.Comentarios = result.Comentarios;

					await DisplayAlert("Pronto!", "Sua avaliação foi enviada com sucesso!", "OK");
					var page = await Navigation.PopAsync();
					Navigation.RemovePage(page);
				}
				catch (Exception ex)
				{
					await DisplayAlert("Ops... :(", "Não foi possível salvar sua avaliação devido a um problema: " + ex.Message, "OK");
				}
			};

			sldEstrelas.ValueChanged += (sender, e) => {
				imgEstrelas.Source = string.Format ("star{0:N0}.png", e.NewValue);
			};
		}
	}
}

