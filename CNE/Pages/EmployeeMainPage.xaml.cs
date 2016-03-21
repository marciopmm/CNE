using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CNE
{
	public partial class EmployeeMainPage : ContentPage
	{
		public EmployeeMainPage (Empregado empregado, int qtdVisualizacoes)
		{
			InitializeComponent ();

			#if DEBUG
			bool contains = App.Current.Properties.ContainsKey("Session");
			if (contains)
				lblNome.Text = ((LoginResponse)App.Current.Properties ["Session"]).Token;
			#else
			lblNome.Text = "Olá, " + empregado.Nome;
			#endif

			if (qtdVisualizacoes == 0) {
				lblVisualizado.Text = "Seu perfil ainda não foi visualizado.";
			} else if (qtdVisualizacoes == 1) {
				lblVisualizado.Text = "Seu perfil foi visualizado apenas uma vez.";
			} else {
				lblVisualizado.Text = 
					string.Format ("Seu perfil já foi visualizado {0} vezes.", qtdVisualizacoes);
			}

			btnPerfil.Clicked += (object sender, EventArgs e) => {				
				App.Current.MainPage = new EmployeeRegisterPage2(empregado);
			};
		}
	}
}

