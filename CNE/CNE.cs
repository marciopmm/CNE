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
			var sessionID = UserData.Load ();

			if (string.IsNullOrWhiteSpace(sessionID)) {
				sessionID = Properties.ContainsKey("SessionID") ? (string)Properties ["SessionID"] : null;
			}

			if (!string.IsNullOrWhiteSpace(sessionID)) {
				// Verifica a sessao na API
				if (service.CheckSession (sessionID)) {
					// Se estiver OK, inicia normalmente
					UserData.Save (sessionID);
					Properties ["SessionID"] = sessionID;
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
			var sessionID = Properties.ContainsKey("SessionID") ? (string)Properties ["SessionID"] : null;
			if (!string.IsNullOrWhiteSpace (sessionID)) {
				UserData.Save (sessionID);
				MainPage = new NavigationPage (new CNE.MainPage ());
			} else {
				MainPage = new LoginPage ();
			}
		}

		public void Logout ()
		{
			UserData.Save ("");
			Properties ["SessionID"] = null;
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

