using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Text.RegularExpressions;

namespace CNE
{
	public partial class EmployerRegisterPage2 : ContentPage
	{
		private string _email;
		private string _senha;
		private Usuario _user;

		public EmployerRegisterPage2 (string email, string senha)
		{
			InitializeComponent ();

			_email = email;
			_senha = senha;

			pckSexo.Items.Add ("Masculino");
			pckSexo.Items.Add ("Feminino");

			btnRegistrar.Clicked += async (sender, e) => {
				if (IsValid()) {
					try 
					{
						if (_user == null)
							_user = new Usuario();

						_user.Nome = txtNome.Text.Trim();
						_user.Sobrenome = txtSobrenome.Text.Trim();
						_user.Cpf = txtCpf.Text.Trim();
						_user.DataNascimento = pckDtNasc.Date;
						_user.Email = _email;
						_user.Senha = _senha;
						_user.IdPerfil = 1; // Contratante

						// Chamar o método para gravar na API
						RestService service = new RestService();
						await service.RegisterEmployerAsync(_user);

						DisplayAlert("Parabéns", "Seu registro foi efetuado!", "OK");
						App.Current.ShowMainPage();
					}
					catch (Exception ex)
					{
						DisplayAlert("Ops ... :(", "Occoreu um problema durante o registro: " + ex.Message, "OK");
					}
				}
			};
		}

		private bool IsValid()
		{
			bool valid = true;
			string msg = null;

			if (string.IsNullOrWhiteSpace (txtNome.Text)) {
				valid = false;
				msg += "Digite seu nome." + Environment.NewLine;
			}

			if (string.IsNullOrWhiteSpace (txtSobrenome.Text)) {
				valid = false;
				msg += "Digite seu sobrenome." + Environment.NewLine;
			}

			if (string.IsNullOrWhiteSpace (txtCpf.Text)) {
				valid = false;
				msg += "Digite seu CPF." + Environment.NewLine;
			}

			if (pckSexo.SelectedIndex < 0) {
				valid = false;
				msg += "Informe seu sexo." + Environment.NewLine;
			}

			if (pckDtNasc.Date > DateTime.Now.AddYears(-16)) {
				valid = false;
				msg += "Você deve ter mais de 16 anos para se cadastrar." + Environment.NewLine;
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

