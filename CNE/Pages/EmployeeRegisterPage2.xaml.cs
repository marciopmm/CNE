using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Globalization;

namespace CNE
{
	public partial class EmployeeRegisterPage2 : ContentPage
	{
		private IDictionary<string, short> _especialidades;

		public EmployeeRegisterPage2 (string email, string senha)
		{
			InitializeComponent ();

			_especialidades = ListarEspecialidades ();

			pckSexo.Items.Add ("Masculino");
			pckSexo.Items.Add ("Feminino");

			pckDtNasc.MaximumDate = DateTime.Now.AddYears (-16);

			int row = 0;

			foreach (var espec in _especialidades) {
				gridEspecialidades.RowDefinitions.Add (new RowDefinition () {
					Height = GridLength.Auto
				});

				var label = new Label ();

				var swt = new Switch ();

				label.Text = espec.Key;
				label.HorizontalOptions = LayoutOptions.EndAndExpand;
				label.VerticalOptions = LayoutOptions.CenterAndExpand;

				gridEspecialidades.Children.Add (label, 0, row);
				gridEspecialidades.Children.Add (swt, 1, row);
				row++;
			}

			btnRegistrar.Clicked += async (sender, e) => {				
				try
				{
					if (IsValid ()) {
						Empregado empregado = new Empregado();
						empregado.Nome = celNome.Text;
						empregado.Sobrenome = celSobrenome.Text;
						empregado.Sexo = pckSexo.SelectedIndex == 0 ? 'M' : 'F';
						empregado.Cpf = celCpf.Text;
						empregado.IdPerfil = 2; // Fixo para empregado
						empregado.DataNascimento = pckDtNasc.Date;
						empregado.Email = email.ToLower();
						empregado.Senha = senha;

						empregado.Sobre = celMiniCV.Text;
						empregado.TelCelular = celCelular.Text;
						empregado.TelResidencial = celTelefone.Text;
						empregado.AceitaDormir = swtAceitaDormir.IsToggled;

						empregado.Endereco = new Endereco();
						empregado.Endereco.Cep = celCep.Text;
						empregado.Endereco.Logradouro = celRua.Text;
						empregado.Endereco.Complemento = celComplemento.Text;
						empregado.Endereco.Bairro = celBairro.Text;
						empregado.Endereco.Cidade = celCidade.Text;
						empregado.Endereco.Estado = celUF.Text;
						empregado.Endereco.Numero = celNumero.Text;

						string label = "";
						var especs = new List<Especialidade>();
						// Carrega a lista de Especialidades
						for(int i = 0; i < gridEspecialidades.Children.Count; i++)
						{
							var element = gridEspecialidades.Children[i];

							if(element is Label) {
								label = ((Label)element).Text;
							}
							else if (element is Switch) {
								var s = (Switch)element;
								if (s.IsToggled)
								especs.Add(new Especialidade(){
									IdTipoEspecialidade = _especialidades[label],
									TipoEspecialidade = label
								});
							}
						}

						empregado.Especialidades = especs;

						RestService service = new RestService();
						await service.RegisterEmployeeAsync(empregado);

						await DisplayAlert("Pronto", "Seu usuário foi criado com sucesso.", "OK");
						App.Current.Logout();
					}
				}
				catch (Exception ex)
				{
					await DisplayAlert("Ops... :(", "Não foi possível registrar seu usuário devido a um problema (" + ex.Message + ").", "OK");
				}
			};

			celCep.Completed += async (sender, e) => {
				try
				{
					RestService service = new RestService();
					CepApiResponse endereco = await service.GetAddressFromApi(celCep.Text);

					if (!endereco.Ativo) {
						await DisplayAlert("Atenção", "CEP não encontrado.", "OK");
						return;
					}

					celCep.Text = endereco.Cep;
					celRua.Text = endereco.Logradouro;
					celBairro.Text = endereco.Bairro;
					celCidade.Text = endereco.Cidade;
					celUF.Text = endereco.UF;
				}
				catch (Exception ex)
				{
					await DisplayAlert("Ops ... :(", "Não foi possível obter os dados do CEP fornecido devido a um problema (" + ex.Message + ").", "OK");
				}
			};
		}

		private bool IsValid()
		{
			List<string> erros = new List<string> ();

			if (string.IsNullOrWhiteSpace (celNome.Text)) {
#if __IOS__
				celNome.BackgroundColor = Color.FromHex("FFFFBB");
#else
				celNome.BackgroundColor = Color.FromHex("882222");
#endif
				erros.Add ("Preencha o Nome");
			} else {
				celNome.BackgroundColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celSobrenome.Text)) {
#if __IOS__
				celSobrenome.BackgroundColor = Color.FromHex("FFFFBB");
#else
				celSobrenome.BackgroundColor = Color.FromHex("882222");
#endif
				erros.Add ("Preencha o Sobrenome");
			} else {
				celSobrenome.BackgroundColor = Color.Default;
			}

			if (pckSexo.SelectedIndex < 0) {
#if __IOS__
				pckSexo.BackgroundColor = Color.FromHex("FFFFBB");
#else
				pckSexo.BackgroundColor = Color.FromHex("882222");
#endif
				erros.Add ("Escolha o Sexo");
			} else {
				pckSexo.BackgroundColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celCpf.Text)) {
				#if __IOS__
				celCpf.BackgroundColor = Color.FromHex("FFFFBB");
				#else
				celCpf.BackgroundColor = Color.FromHex("882222");
				#endif
				erros.Add ("Preencha o Cpf");
			} else {
				celCpf.BackgroundColor = Color.Default;
			}

			if (pckDtNasc.Date.AddYears(16) > DateTime.Now) {
				#if __IOS__
				pckDtNasc.BackgroundColor = Color.FromHex("FFFFBB");
				#else
				pckDtNasc.BackgroundColor = Color.FromHex("882222");
				#endif
				erros.Add ("Preencha o Data de Nascimento");
			} else {
				pckDtNasc.BackgroundColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celCep.Text)) {
				#if __IOS__
				celCep.BackgroundColor = Color.FromHex("FFFFBB");
				#else
				celCep.BackgroundColor = Color.FromHex("882222");
				#endif
				erros.Add ("Preencha o CEP");
			} else {
				celCep.BackgroundColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celNumero.Text)) {
				#if __IOS__
				celNumero.BackgroundColor = Color.FromHex("FFFFBB");
				#else
				celNumero.BackgroundColor = Color.FromHex("882222");
				#endif
				erros.Add ("Preencha o Número");
			} else {
				celNumero.BackgroundColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celCelular.Text)) {
				#if __IOS__
				celCelular.BackgroundColor = Color.FromHex("FFFFBB");
				#else
				celCelular.BackgroundColor = Color.FromHex("882222");
				#endif
				erros.Add ("Preencha o Tel. Celular");
			} else {
				celCelular.BackgroundColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celTelefone.Text)) {
				#if __IOS__
				celTelefone.BackgroundColor = Color.FromHex("FFFFBB");
				#else
				celTelefone.BackgroundColor = Color.FromHex("882222");
				#endif
				erros.Add ("Preencha o Tel. Residencial");
			} else {
				celTelefone.BackgroundColor = Color.Default;
			}

			// Itens de Especialidade
			int count = 0;
			foreach (var cell in gridEspecialidades.Children) {
				if (cell is Switch && ((Switch)cell).IsToggled) {
					count++;
				}
			}			

			if (count == 0) {
				erros.Add ("Escolha pelo menos uma especialidade");
			}

			if (erros.Count > 0) {
				string msg = string.Join (System.Environment.NewLine, erros).Trim();
				DisplayAlert ("Atenção", msg, "OK");
				return false;
			}

			return true;
		}

		private IDictionary<string, short> ListarEspecialidades()
		{
			Dictionary<string, short> dic = new Dictionary<string, short> ();

			//TODO: Obter valores da API

			dic.Add ("Arrumadeira", 1);
			dic.Add ("Babá", 2);
			dic.Add ("Babá Folguista", 3);
			dic.Add ("Caseiro", 4);
			dic.Add ("Copeira", 5);
			dic.Add ("Cozinheira", 6);
			dic.Add ("Cuidador de Animais", 7);
			dic.Add ("Cuidador de Idosos", 8);
			dic.Add ("Diarista", 9);
			dic.Add ("Empregada Doméstica", 10);
			dic.Add ("Governanta/Mordomo", 11);
			dic.Add ("Home Organizer", 12);
			dic.Add ("Motorista", 13);

			return dic;
		}
	}
}

