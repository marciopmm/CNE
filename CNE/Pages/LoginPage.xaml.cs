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
						App.Current.Properties["Session"] = result;
						App.Current.ShowMainPage();
					} else {
						await DisplayAlert("Ops... :(", "Email e/ou senha inválidos.", "OK");
					}
				}
			};

			btnEsqueci.Clicked += async (sender, e) => {
				if (EmailIsValid())
				{
					string email = txtEmail.Text.Trim();
					var result = await DisplayAlert("Alteração de Senha", "Tem certeza que deseja alterar a senha para o usuário " + email + " ?", "Sim", "Não");
					if (result) {
						RestService rest = new RestService();
						var restResult = await rest.ChangePassword(email);
						if (restResult.IsSuccess)
						{
							await DisplayAlert("Pronto!", "Foi enviado um email com as instruções para troca de senha no endereço " + email, "OK");
							App.Current.ShowMainPage();
						} else {
							await DisplayAlert("Ops... :(", restResult.Message, "OK");
						}
					}
				}
				else {
					await DisplayAlert("Atenção", "Digite um email válido.", "OK");
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

		private bool EmailIsValid()
		{
			bool valid = true;

			if (string.IsNullOrWhiteSpace (txtEmail.Text)) {		
#if __IOS__
				txtEmail.BackgroundColor = Color.FromHex ("FFFFBB");
#elif __ANDROID
				txtEmail.BackgroundColor = Color.FromHex ("750000");
#else
				txtEmail.BackgroundColor = Color.FromHex ("750000");
#endif
				valid = false;
			} else {
				txtEmail.BackgroundColor = Color.Default;
			}

			return valid;
		}
	}
}

