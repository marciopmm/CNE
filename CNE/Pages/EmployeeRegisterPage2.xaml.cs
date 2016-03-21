using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Globalization;

using System.Linq;

namespace CNE
{
	public partial class EmployeeRegisterPage2 : ContentPage
	{
		private IDictionary<string, short> _especialidades;

		public EmployeeRegisterPage2 (Empregado empregado)
		{
			InitializeComponent ();

			_especialidades = ListarEspecialidades ();

			pckSexo.Items.Add ("Masculino");
			pckSexo.Items.Add ("Feminino");

			pckDtNasc.Date = DateTime.Now.AddYears (-20);
			pckDtNasc.MaximumDate = DateTime.Now.AddYears (-16);

			int row = 0;

			foreach (var espec in _especialidades) {
				gridEspecialidades.RowDefinitions.Add (new RowDefinition () {
					Height = GridLength.Auto
				});

				var label = new Label ();

				var swt = new Switch ();
				if (empregado != null)
					swt.IsToggled = empregado.Especialidades.Any (X => X.TipoEspecialidade == espec.Key);

				label.Text = espec.Key;
				label.HorizontalOptions = LayoutOptions.EndAndExpand;
				label.VerticalOptions = LayoutOptions.CenterAndExpand;

				gridEspecialidades.Children.Add (label, 0, row);
				gridEspecialidades.Children.Add (swt, 1, row);
				row++;
			}

			if (empregado != null)
				Fill (empregado);

			btnCancelar.Clicked += (sender, e) => {
				App.Current.ShowMainPage();
			};

			btnRegistrar.Clicked += async (sender, e) => {				
				try
				{
					if (IsValid ()) {
						empregado.Nome = celNome.Text;
						empregado.Sobrenome = celSobrenome.Text;
						empregado.Sexo = pckSexo.SelectedIndex == 0 ? 'M' : 'F';
						empregado.Cpf = celCpf.Text;
						empregado.IdPerfil = 2; // Fixo para empregado
						empregado.DataNascimento = pckDtNasc.Date;
						empregado.Email = empregado.Email.ToLower();
						empregado.Senha = empregado.Senha;

						empregado.Sobre = celMiniCV.Text;
						empregado.TelCelular = celCelular.Text;
						empregado.TelResidencial = celTelefone.Text;
						empregado.AceitaDormir = swtAceitaDormir.IsToggled;

						if  (empregado.Endereco == null)
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

						await DisplayAlert("Pronto", "Seus dados foram salvos com sucesso.", "OK");
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

		private void Fill(Empregado empregado)
		{
			pckSexo.SelectedIndex = empregado.Sexo == 'M' ? 0 : 1;
			pckDtNasc.Date = empregado.DataNascimento;

			celNome.Text = empregado.Nome;
			celSobrenome.Text = empregado.Sobrenome;
			if (empregado.Endereco != null) {
				celBairro.Text = empregado.Endereco.Bairro;

				celCep.Text = empregado.Endereco.Cep;
				celCidade.Text = empregado.Endereco.Cidade;
				celComplemento.Text = empregado.Endereco.Complemento;
				celNumero.Text = empregado.Endereco.Numero;
				celRua.Text = empregado.Endereco.Logradouro;
				celUF.Text = empregado.Endereco.Estado;
			}
			celCpf.Text = empregado.Cpf;
			celMiniCV.Text = empregado.Sobre;
			celTelefone.Text = empregado.TelResidencial;
			celCelular.Text = empregado.TelCelular;

			swtAceitaDormir.IsToggled = empregado.AceitaDormir;
		}

		private bool IsValid()
		{
			List<string> erros = new List<string> ();

			if (string.IsNullOrWhiteSpace (celNome.Text)) {
				celNome.BackgroundColor = Color.FromHex("FFFFBB");
				erros.Add ("Preencha o Nome");
			} else {
				celNome.BackgroundColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celSobrenome.Text)) {
				celSobrenome.BackgroundColor = Color.FromHex("FFFFBB");
				erros.Add ("Preencha o Sobrenome");
			} else {
				celSobrenome.BackgroundColor = Color.Default;
			}

			if (pckSexo.SelectedIndex < 0) {
				pckSexo.BackgroundColor = Color.FromHex("FFFFBB");
				erros.Add ("Escolha o Sexo");
			} else {
				pckSexo.BackgroundColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celCpf.Text)) {
				celCpf.BackgroundColor = Color.FromHex("FFFFBB");
				erros.Add ("Preencha o Cpf");
			} else {
				celCpf.BackgroundColor = Color.Default;
			}

			if (pckDtNasc.Date.AddYears(16) > DateTime.Now) {
				pckDtNasc.BackgroundColor = Color.FromHex("FFFFBB");
				erros.Add ("Preencha o Data de Nascimento");
			} else {
				pckDtNasc.BackgroundColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celCep.Text)) {
				celCep.BackgroundColor = Color.FromHex("FFFFBB");
				erros.Add ("Preencha o CEP");
			} else {
				celCep.BackgroundColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celNumero.Text)) {
				celNumero.BackgroundColor = Color.FromHex("FFFFBB");
				erros.Add ("Preencha o Número");
			} else {
				celNumero.BackgroundColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celCelular.Text)) {
				celCelular.BackgroundColor = Color.FromHex("FFFFBB");
				erros.Add ("Preencha o Tel. Celular");
			} else {
				celCelular.BackgroundColor = Color.Default;
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

