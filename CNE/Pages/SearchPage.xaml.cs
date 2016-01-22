using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Diagnostics;

namespace CNE
{
	public partial class SearchPage : ContentPage
	{
		public SearchPage ()
		{
			InitializeComponent ();
		}

		public SearchPage (string cep, int tipo, int distancia) : this()
		{
			// Chamar os dados da API CNE passando estes 3 parametros 
			var service = new RestService ();
			var empregadosList = service.List (cep, tipo, distancia);

			#region ***** Dados de TESTE *****		
			/*
			List<Empregado> empregadosList = new List<Empregado> ();
			empregadosList.Add(new Empregado(){ 
				Nome = "Maria José do Socorro",
				Especialidades = "Arrumadeira, Babá, Babá Folguista, Diarista",
				TelCelular = "+55 11 987-654-321",
				TelResidencial = "+55 11 2345-6789",
				Endereco = "Rua Austregésilo de Atayde, 67, Vila Ermenengarda, São Paulo/SP",
				QtdQualificacoes = 34,
				Estrelas = 4,
				Comentarios = new List<string>() {
					"Uma maravílha de moça. Vale a pena! Muito competente e responsável.",
					"Atrasou um poquinho no horário combinado, mas no geral é muito boa profissional",
					"Sensacional! Faz tudo!",
					"Sentimos falta quando ela está longe! Recomendo!"
				}
			});

			empregadosList.Add(new Empregado(){ 
				Nome = "Eugênia Estrogonófica dos Santos",
				Especialidades = "Arrumadeira, Diarista",
				TelCelular = "+55 11 999-888-777",
				TelResidencial = "+55 11 2222-3333",
				Endereco = "Rua Auspeciosamêntila, 847, apto 42, Jardim das Jardinagens, São Paulo/SP",
				QtdQualificacoes = 7,
				Estrelas = 3,
				Comentarios = new List<string>() {
					"Bem meia boca mesmo.",
					"Fez a limpeza mas achei várias teias de aranha pelo teto. Não gostei.",
					"A limpeza é ruim, mas tem coisas que só a Eugênia faz pra você.",
					"Maravilhosa!!"
				}
			});

			empregadosList.Add(new Empregado(){ 
				Nome = "Auspiciosa Marcondina Espósito",
				Especialidades = "Babá, Cuidadora de Idosos, Diarista",
				TelCelular = "+55 11 912-345-678",
				TelResidencial = "+55 11 3333-4444",
				Endereco = "Rua Oblivionça, 345, Parque das Guiozêmicas, São Paulo/SP",
				QtdQualificacoes = 25,
				Estrelas = 1,
				Comentarios = new List<string>() {
					"Deixa quieto.",
					"Não contratem esta mulher!",
					"Vou processá-la! Ela jogou minha comida no vaso sanitário e fez meu pai comer fezes!",
					"Nem de graça. Corram!",
					"Até que pela pontualidade valeu."
				}
			}); */
			#endregion

			lstPesquisa.ItemsSource = empregadosList;
			lstPesquisa.ItemTemplate = new DataTemplate (typeof(TextCell));
			lstPesquisa.ItemTemplate.SetBinding (TextCell.TextProperty, "Nome"); 
			lstPesquisa.ItemTemplate.SetBinding (TextCell.DetailProperty, "TextoEspecialidades"); 
			
			lstPesquisa.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) => {
				await Navigation.PushAsync(new DetailsPage((Empregado)e.SelectedItem));
			};
		}
		 
	}
}

