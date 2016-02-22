using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CNE
{
	public partial class ChangePasswordPage : ContentPage
	{
		public ChangePasswordPage ()
		{
			InitializeComponent ();

			btnOK.Clicked += async (sender, e) => {
				try
				{
					RestService rest = new RestService();

					if (IsValid()) {
						await rest.SetNewPassword(txtConfSenha.Text);
						await DisplayAlert("Beleza!", "Sua senha foi alterada.", "OK");

						var session = (LoginResponse)App.Current.Properties["Session"];
						session.AlterarSenha = false;
						App.Current.Properties["Session"] = session;
						App.Current.ShowMainPage();
					}
				}
				catch (Exception ex) {
					await DisplayAlert("Erro", "Houve um erro durante a chamada ao servidor (" + ex.Message + ")", "OK");
				}
			};
		}

		private bool IsValid() 
		{
			if (txtNovaSenha.Text != txtConfSenha.Text) {
				DisplayAlert ("Atenção", "Os valores não conferem. Por favor verifique se escreveu a mesma senha nos dois campos.", "OK");
				return false;
			}

			return true;
		}
	}
}

