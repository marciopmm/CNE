using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CNE
{
	public partial class RegisterPage1 : ContentPage
	{
		public RegisterPage1 ()
		{
			InitializeComponent ();

			btnProfissional.Clicked += async (sender, e) => {
				if (await IsValid ()) {
					App.Current.MainPage = new EmployeeRegisterPage2 (txtEmail.Text, txtSenha.Text);
				}
			};

			btnContratante.Clicked += async (sender, e) => {
				if (await IsValid ()) {
					App.Current.MainPage = new EmployerRegisterPage2 (txtEmail.Text, txtSenha.Text);
				}
			};
		}

		private async Task<bool> IsValid()
		{
			bool valid = true;
			string msg = null;

			if (string.IsNullOrWhiteSpace (txtEmail.Text)) {
				valid = false;
				msg += "Digite um email válido." + Environment.NewLine;
			} else if (!Regex.IsMatch (txtEmail.Text, "^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,}$", RegexOptions.IgnoreCase)) {
				valid = false;
				msg += "Digite um email válido." + Environment.NewLine;
			} else {
				// Verificar se o email já está em uso
				RestService service = new RestService();
				bool disponivel = await service.VerificarEmailDisponivel (txtEmail.Text.ToLower());
				if (!disponivel) {
					valid = false;
					msg += "Este email já está sendo utilizado. Por favor escolha outro email.";
				}
			}

			if (string.IsNullOrWhiteSpace (txtSenha.Text)) {
				valid = false;
				msg += "Digite uma senha." + Environment.NewLine;
			} else if (txtSenha.Text.Length < 6) {
				valid = false;
				msg += "Digite uma senha com um mínimo de 6 caracteres.";
			}

			if (!string.IsNullOrWhiteSpace (msg)) {
				msg = msg.Trim ();
				if (msg.Length > 0)
					await DisplayAlert ("Atenção", msg, "OK");
			}

			return valid;
		}
	}
}

