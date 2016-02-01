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
						empregado.DataNascimento = DateTime.ParseExact(celDtNasc.Text, 
							"dd/MM/yyyy", new CultureInfo("pt-BR"));
						empregado.Email = email.ToLower();
						empregado.Senha = senha;

						empregado.Sobre = celMiniCV.Text;
						empregado.TelCelular = celCelular.Text;
						empregado.TelResidencial = celTelefone.Text;
						empregado.AceitaDormir = swtAceitaDormir.On;

						empregado.Endereco = new Endereco();
						empregado.Endereco.Cep = celCep.Text;
						empregado.Endereco.Logradouro = celRua.Text;
						empregado.Endereco.Complemento = celComplemento.Text;
						empregado.Endereco.Bairro = celBairro.Text;
						empregado.Endereco.Cidade = celCidade.Text;
						empregado.Endereco.Estado = celUF.Text;
						empregado.Endereco.Numero = celNumero.Text;

						var especs = new List<Especialidade>();
						//TODO: Será alterado para comportar lista dinâmica de Especialidades
						foreach(var swt in secEspecialidades)
						{
							if (swt is SwitchCell) {
								var s = (SwitchCell)swt;
								if (s.On)
								especs.Add(new Especialidade(){
									IdTipoEspecialidade = _especialidades[s.Text],
									TipoEspecialidade = s.Text
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
				celNome.TextColor = Color.Red;
				erros.Add ("Preencha o Nome");
			} else {
				celNome.TextColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celSobrenome.Text)) {
				celSobrenome.TextColor = Color.Red;
				erros.Add ("Preencha o Sobrenome");
			} else {
				celSobrenome.TextColor = Color.Default;
			}

			if (pckSexo.SelectedIndex < 0) {
				pckSexo.BackgroundColor = Color.FromHex("FFFFBB");
				erros.Add ("Escolha o Sexo");
			} else {
				pckSexo.BackgroundColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celCpf.Text)) {
				celCpf.TextColor = Color.Red;
				erros.Add ("Preencha o Cpf");
			} else {
				celCpf.TextColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celDtNasc.Text)) {
				celDtNasc.TextColor = Color.Red;
				erros.Add ("Preencha o Data de Nascimento");
			} else {
				celDtNasc.TextColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celCep.Text)) {
				celCep.LabelColor = Color.Red;
				erros.Add ("Preencha o CEP");
			} else {
				celCep.LabelColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celNumero.Text)) {
				celNumero.LabelColor = Color.Red;
				erros.Add ("Preencha o Número");
			} else {
				celNumero.LabelColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celCelular.Text)) {
				celCelular.LabelColor = Color.Red;
				erros.Add ("Preencha o Tel. Celular");
			} else {
				celCelular.LabelColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (celTelefone.Text)) {
				celTelefone.LabelColor = Color.Red;
				erros.Add ("Preencha o Tel. Residencial");
			} else {
				celTelefone.LabelColor = Color.Default;
			}

			if (string.IsNullOrWhiteSpace (swtAceitaDormir.Text)) {
				erros.Add ("Indique se aceita dormir");
			}

			// Itens de Especialidade
			int count = 0;
			foreach (Cell cell in secEspecialidades) {
				if (cell is SwitchCell && ((SwitchCell)cell).On) {
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

