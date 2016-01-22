using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Text.RegularExpressions;

namespace CNE
{
	public partial class RegisterPage1 : ContentPage
	{
		public RegisterPage1 ()
		{
			InitializeComponent ();

			btnProfissional.Clicked += async (sender, e) => {
				if (IsValid ()) {
					App.Current.MainPage = new EmployeeRegisterPage2 (txtEmail.Text, txtSenha.Text);
				}
			};

			btnContratante.Clicked += async (sender, e) => {
				if (IsValid ()) {
					App.Current.MainPage = new EmployerRegisterPage2 (txtEmail.Text, txtSenha.Text);
				}
			};
		}

		private bool IsValid()
		{
			bool valid = true;
			string msg = null;

			if (string.IsNullOrWhiteSpace (txtEmail.Text)) {
				valid = false;
				msg += "Digite um email válido." + Environment.NewLine;
			} else if (!Regex.IsMatch (txtEmail.Text, "^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,}$", RegexOptions.IgnoreCase)) {
				valid = false;
				msg += "Digite um email válido." + Environment.NewLine;
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
					DisplayAlert ("Atenção", msg, "OK");
			}

			return valid;
		}
	}
}

