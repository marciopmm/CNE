using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Text;

namespace CNE
{
	public partial class DetailsPage : ContentPage
	{
		private Empregado _empregado;

		public DetailsPage (Empregado empregado)
		{
			InitializeComponent ();

			_empregado = empregado;

			lblNome.Text = empregado.Nome + " " + empregado.Sobrenome;
			lblQtdQualificacoes.Text = empregado.QtdAvalicaoes.ToString();
			imgEstrelas.Source = "star" + empregado.Estrelas.ToString () + ".png";
			lblCelular.Text = empregado.TelCelular;
			lblTelefone.Text = empregado.TelResidencial;

			string strEndereco = string.Format ("{0}, {1} {2}, {3}, {4}/{5}",
				                     empregado.Endereco.Logradouro,
				                     empregado.Endereco.Numero,
				                     empregado.Endereco.Complemento, 
				                     empregado.Endereco.Bairro,
				                     empregado.Endereco.Cidade,
				                     empregado.Endereco.Estado);

			lblEndereco.Text = strEndereco;

			StringBuilder sb = new StringBuilder ();
			foreach (Especialidade espec in empregado.Especialidades) {
				sb.AppendLine (espec.TipoEspecialidade);
			}
			lblEspecialidades.Text = sb.ToString();

			Color color = Color.FromHex ("333333");

			foreach (string c in empregado.Comentarios) {
				Label lbl = new Label ();
				lbl.HorizontalOptions = LayoutOptions.StartAndExpand;
				lbl.Text = c;

				if (color == Color.FromHex ("333333"))
					color = Color.FromHex ("777777");
				else
					color = Color.FromHex ("333333");

				lbl.TextColor = color;

				stkComentarios.Children.Add (lbl);
			}

			if (stkComentarios.Children.Count == 0) {
				Label lbl = new Label ();
				lbl.HorizontalOptions = LayoutOptions.StartAndExpand;
				lbl.Text = "Nenhum comentário.";
				lbl.TextColor = color;

				stkComentarios.Children.Add (lbl);
			}

			btnAvaliar.Clicked += async (sender, e) => {
				await Navigation.PushAsync(new EvaluationPage(_empregado));
			};
		}
	}
}

