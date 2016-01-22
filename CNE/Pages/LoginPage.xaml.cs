using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CNE
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();

			btnLogin.Clicked += async (sender, e) => {
				if (IsValid()) {
					//TODO: Chamada para a API - LOGIN
					// Obter o ID da sessão do Usuário

					App.Current.Properties["SessionID"] = "SESSIONTESTE";
					App.Current.ShowMainPage();
				}
			};

			btnCriarConta.Clicked += async (sender, e) => {
				await Navigation.PushModalAsync(new RegisterPage1());
			};
		}

		private bool IsValid()
		{
			bool valid = true;

			if (string.IsNullOrWhiteSpace (txtEmail.Text)) {				
				txtEmail.BackgroundColor = Color.FromHex ("FFFFBB");
				valid = false;
			} else {
				txtEmail.BackgroundColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (txtSenha.Text)) {				
				txtSenha.BackgroundColor = Color.FromHex ("FFFFBB");
				valid = false;
			} else {
				txtEmail.BackgroundColor = Color.Default;
			}

			return valid;
		}
	}
}

