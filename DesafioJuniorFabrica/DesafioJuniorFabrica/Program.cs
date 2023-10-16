using DesafioJuniorFabrica.Interfaces;
using DesafioJuniorFabrica.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace DesafioJuniorFabrica
{
    internal class Program
    {
        //Implementa a interface, para manter o registro das ordens de produção
        static List<IOrdemProduto> ordensProdutos = new List<IOrdemProduto>();

        //Enum Definindo as opções da aplicação
        enum Menu { Adicionar = 1, Atualizar, Listar, Encerrar }
        static void Main(string[] args)
        {
            //Carrega arquivo com dados das ordens ja produzidos
            CarregarArquivo();

            bool encerrarAplicativo = false;
            //While permanece o codigo em loop ate que seja encerrado pelo proprio usuario
            //E mostrando as opções no console
           
            while (!encerrarAplicativo)
            {
                Console.WriteLine("Gerenciamento de Produção\nEscolha a opção\n");
                Console.WriteLine("1-Adicionar Ordem de produção" +
                    "\n2-Atualizar Ordem de produção" +
                    "\n3-Listar Ordens de produção" +
                    "\n4-Encerrar Aplicativo");

                //lança uma Expection de caracteres para impedir opções invalidas
                try
                {
                    //converte a string digitada para int
                    string opStr = Console.ReadLine();
                    int opInt = int.Parse(opStr);
                    Menu escolha = (Menu)opInt;

                    //Dependendo da escolha o usuario ele chama o metodo referentes 
                    //as escolhas que o usuario pode realizar 
                    //Caso a opção não seja valida lança uma Exception solicitando uma opção valida
                    if (opInt > 0 && opInt < 4)
                    {
                        switch (escolha)
                        {
                            case Menu.Adicionar:
                                CadastroOrdemProduto();
                                break;
                            case Menu.Atualizar:
                                Atualizar();
                                break;
                            case Menu.Listar:
                                Listagem();
                                break;
                            case Menu.Encerrar:
                                encerrarAplicativo = true;
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Escolha uma Opção valida\nAperte Enter para tentar Novamente");
                        Console.ReadKey();
                    }
                    Console.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n{ex.Message}\nOperação invalida\nAperte Enter para tentar novamente");
                    Console.ReadKey();
                    Console.Clear();
                }
                
            }
        }

        static void CadastroOrdemProduto()
        {
            Console.WriteLine("Cadastrando uma Ordem de Produção");

            //Obtem o nome passado pelo o usuario
            Console.WriteLine("Digite o nome do Produto: ");
            string NomeProduto = Console.ReadLine();

            // Verificar se o nome do produto está vazio ou contém apenas espaços em branco
            if (string.IsNullOrWhiteSpace(NomeProduto))
            {
                Console.WriteLine("Nome do Produto não pode estar vazio.");
                // lança uma Exception para que seja preenchido o campo corretamente
                throw new ArgumentException("Preencha o campo corretamente");
            }

            //Obtem o numero de materais em estoque passados pelo usuario
            Console.WriteLine("Digite a quantidade de materiais Atualmente");
            int QtMaterial = int.Parse(Console.ReadLine());

            //Obtem a quantidade de produtos que deverão ser criados
            Console.WriteLine("Digite a quantidade de Produtos a serem criados: ");
            int QtProduto = int.Parse(Console.ReadLine());

            //Aqui realiza uma estimativa de quanto material seria necessario para criar o produto supondo que seria 3 materiais para 1 produto
            Console.WriteLine($"Para a quantidade de {QtProduto}, Serão necessarios: {QtProduto * 3} materiais\n" +
                $"Você tem atualmente {QtMaterial}\n ");

            QtProduto = QtProduto * 3;
            string Estado = "Em produção";

            //Verifica se a quantidade de materiais e suficiente para a confecção dos produtos
            if (QtMaterial < QtProduto)
            {
                Console.WriteLine($"\nEstoque insuficiente para criação dos produtos");
                Console.ReadLine();
            }
            else
            {
                //Realizar a criação do produto e salva no arquivo externo
                OrdemProduto ordemProduto = new OrdemProduto(NomeProduto, QtProduto, QtMaterial, Estado);
                ordensProdutos.Add(ordemProduto);
                Console.WriteLine("Ordem Criada");
                Console.ReadLine();
                SalvarArquivo();
            }
        }

        //Este metodo salva as Ordens de Produção e serializa em formato binario, num arquivo chamado OrdemProducao
        static void SalvarArquivo()
        {
            FileStream stream = new FileStream("OrdemProducao.dat", FileMode.OpenOrCreate);
            BinaryFormatter encoder = new BinaryFormatter();

            encoder.Serialize(stream, ordensProdutos);
            stream.Close();
        }
        //Este metodo e para carregar o arquivo com os dados salvos e verifica se o arquivo existe
        //caso não exista ele cria outro arquivo com mesmo nome 
        static void CarregarArquivo()
        {
            FileStream stream = new FileStream("OrdemProducao.dat", FileMode.OpenOrCreate);
            BinaryFormatter encoder = new BinaryFormatter();

            try
            {
                ordensProdutos = (List<IOrdemProduto>)encoder.Deserialize(stream);

                if(ordensProdutos == null)
                {
                    ordensProdutos = new List<IOrdemProduto>();
                }
            }
            catch (Exception ex)
            {
                ordensProdutos = new List<IOrdemProduto>();
            }
            stream.Close();
        }

        //Este metodo exibe a lista com todas as ordens de produção registradas
        //Incluido detalhes dos produtos como e Id da ordem, Nome do produto,
        //Quantidade do produto, o estado do produto se foi Concluido ou Em produção
        //e uma data estimada para a entrega do produto
        static void Listagem()
        {
            Console.WriteLine("Lista de Todas as Ordens:\n");
            int i = 0;
            foreach (IOrdemProduto ordem in ordensProdutos)
            {
                Console.WriteLine("Id: " + i);
                ordem.Exibir();
                i++;
                Console.ReadLine();
            }
        }

        //Este permite o usuario alterar atraves do Id da ordem de produção
        //fornecida pelo usuario, o estado dela de "Em Produção" para "Concluida"
        //Caso coloque um numero invalido ou deixe em branco, lança uma Exception
        //solicitando um id valido
        static void Atualizar()
        {
            Listagem();
            Console.WriteLine("Digite o Id da Ordem para atualizar o estado de |Em Produção| para |Concluido|");

            try
            {
                int id = int.Parse(Console.ReadLine());

                if (id >= 0 && id <= ordensProdutos.Count)
                {
                    IOrdemProduto ordem = ordensProdutos[id];
                    ordem.AtualizarEstado("Concluido");
                    SalvarArquivo();
                    Console.ReadKey();
                }
                else
                {
                    throw new InvalidOperationException("ID de ordem inválido. Certifique-se de fornecer um ID válido.");
                    Console.WriteLine("Aconteceu um erro");
                    Console.ReadKey();
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("ID de ordem inválido. Certifique-se de fornecer um número inteiro válido.");
                Console.ReadKey();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

        }
    }
}
