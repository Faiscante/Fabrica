namespace DesafioJuniorFabrica.Interfaces
{
    //Aki tem uma interface que realiza o contrato com as classes
    //fazendo com que os métodos estarão disponiveis
    internal interface IOrdemProduto
    {
        void Exibir();
        void AtualizarEstado(string estado);

    }
}
