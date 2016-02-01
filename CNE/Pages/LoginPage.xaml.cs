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
					var service = new RestService();

					// Obter o ID da nova sessão do Usuário
					var result = service.Login (txtEmail.Text.ToLower(), txtSenha.Text);

					// Se Login OK
					if (result != null) {
						App.Current.Properties["SessionID"] = result.Token;
						App.Current.ShowMainPage();
					} else {
						DisplayAlert("Ops... :(", "Email e/ou senha inválidos.", "OK");
					}
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

