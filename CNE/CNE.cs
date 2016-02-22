using System;

using Xamarin.Forms;

namespace CNE
{
	public class App : Application
	{
		public static App Current { get; private set; }

		public App ()
		{
			Current = this;
			var service = new RestService ();
			var loginResponse = UserData.Load ();

			if (loginResponse == null) {
				loginResponse = Properties.ContainsKey("Session") ? (LoginResponse)Properties ["Session"] : null;
			}

			if (loginResponse != null) {
				// Verifica a sessao na API
				if (service.CheckSession (loginResponse.Token)) {
					// Se estiver OK, inicia normalmente
					UserData.Save (loginResponse);
					Properties ["Session"] = loginResponse;
					if (loginResponse.AlterarSenha)
						MainPage = new ChangePasswordPage ();
					else
						MainPage = new NavigationPage (new CNE.MainPage ());
				} else {
					MainPage = new LoginPage ();
				}
			}
			else
				MainPage = new LoginPage ();			
		}

		public void ShowMainPage ()
		{	
			var loginResponse = Properties.ContainsKey("Session") ? (LoginResponse)Properties ["Session"] : null;

			if (loginResponse != null) {
				UserData.Save (loginResponse);
				if (loginResponse.AlterarSenha) {
					MainPage = new ChangePasswordPage ();
				} else {
					MainPage = new NavigationPage (new CNE.MainPage ());
				}
			} else {
				MainPage = new LoginPage ();
			}
		}

		public void Logout ()
		{
			UserData.Save (null);
			Properties ["Session"] = null;

			MainPage = new LoginPage ();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

