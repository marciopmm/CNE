using System;

using Xamarin.Forms;

namespace CNE
{
	public class App : Application
	{
		public static App Current { get; private set; }

		public App ()
		{
			SetStyles ();
			
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
					Properties["Session"] = loginResponse;
					ShowMainPage(loginResponse);
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
			ShowMainPage (loginResponse);
		}

		public void ShowMainPage (LoginResponse loginResponse)
		{
			if (loginResponse != null) {
				UserData.Save (loginResponse);

				if (loginResponse.AlterarSenha) {
					MainPage = new ChangePasswordPage ();
				} else if (loginResponse.IsEmpregado) {
					RestService rest = new RestService ();
					Empregado empregado = rest.GetEmployee (loginResponse.Token);
					MainPage = new EmployeeMainPage (empregado, loginResponse.QtdVisualizacoes);
				}
				else {
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

		#region Métodos Privados

		private void SetStyles()
		{
			Resources = new ResourceDictionary ();

			var contentStyle = new Style (typeof(ContentPage)) {
				Setters = {
					new Setter {
						Property = ContentPage.BackgroundColorProperty,
						Value = Color.White
					}
				}
			};
			Resources.Add (contentStyle);

			var entryStyle = new Style (typeof(Entry)) {
				Setters = {
					new Setter {
						Property = Entry.BackgroundColorProperty,
						Value = Color.FromHex("EEEEEE")
					},
					new Setter {
						Property = Entry.TextColorProperty,
						Value = Color.FromHex("333333")
					},
					new Setter {
						Property = Entry.HorizontalOptionsProperty,
						Value = LayoutOptions.FillAndExpand
					},
					new Setter {
						Property = Entry.VerticalOptionsProperty,
						Value = LayoutOptions.CenterAndExpand
					}
				}
			};
			Resources.Add (entryStyle);

			var editorStyle = new Style (typeof(Editor)) {
				Setters = {
					new Setter {
						Property = Editor.BackgroundColorProperty,
						Value = Color.FromHex("EEEEEE")
					},
					new Setter {
						Property = Editor.TextColorProperty,
						Value = Color.FromHex("333333")
					},
					new Setter {
						Property = Editor.HorizontalOptionsProperty,
						Value = LayoutOptions.FillAndExpand
					},
					new Setter {
						Property = Editor.VerticalOptionsProperty,
						Value = LayoutOptions.FillAndExpand
					}
				}
			};
			Resources.Add (editorStyle);

			var pickerStyle = new Style (typeof(Picker)) {
				Setters = {
					new Setter {
						Property = Picker.BackgroundColorProperty,
						Value = Color.FromHex("EEEEEE")
					},
					new Setter {
						Property = Picker.HorizontalOptionsProperty,
						Value = LayoutOptions.FillAndExpand
					},
					new Setter {
						Property = Picker.VerticalOptionsProperty,
						Value = LayoutOptions.CenterAndExpand
					}
				}
			};
			Resources.Add (pickerStyle);

			var datePickerStyle = new Style (typeof(DatePicker)) {
				Setters = {
					new Setter {
						Property = DatePicker.BackgroundColorProperty,
						Value = Color.FromHex("EEEEEE")
					},
					new Setter {
						Property = DatePicker.HorizontalOptionsProperty,
						Value = LayoutOptions.FillAndExpand
					},
					new Setter {
						Property = DatePicker.VerticalOptionsProperty,
						Value = LayoutOptions.CenterAndExpand
					}
				}
			};
			Resources.Add (datePickerStyle);

			var buttonStyle = new Style (typeof(Button)) {
				Setters = {
					new Setter { 
						Property = Button.BackgroundColorProperty, 
						Value = Color.FromHex("DDDDDD")
					},
					new Setter {
						Property = Button.BorderRadiusProperty,
						Value = 5
					},
					new Setter {
						Property = Button.BorderWidthProperty,
						Value = 2
					},
					new Setter {
						Property = Button.BorderColorProperty,
						Value = Color.FromHex("444444")
					},
					new Setter {
						Property = Button.TextColorProperty,
						Value = Color.FromHex("333333")
					},
					new Setter {
						Property = Button.HorizontalOptionsProperty,
						Value = LayoutOptions.FillAndExpand
					},
					new Setter {
						Property = Button.VerticalOptionsProperty,
						Value = LayoutOptions.CenterAndExpand
					}
				}
			};
			Resources.Add (buttonStyle);

			var labelStyle = new Style (typeof(Label)) {
				Setters = {
					new Setter {
						Property = Label.TextColorProperty,
						Value = Color.FromHex ("333333")
					}
				}
			};
			Resources.Add (labelStyle);

		}

		#endregion
	}
}

