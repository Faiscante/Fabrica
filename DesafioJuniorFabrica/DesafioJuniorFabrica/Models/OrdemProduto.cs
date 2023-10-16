using DesafioJuniorFabrica.Interfaces;
using System;

namespace DesafioJuniorFabrica.Models
{
    //Essa marcação permite que seja salvo em um arquivo para armazenamento 
    //em um arquivo ou tramitido pela rede
    [Serializable]
    internal class OrdemProduto : IOrdemProduto
    {
        //Propriedades das ordens de produção
        public string NomeProduto { get; set; }
        private int QtProduto { get; set; }
        private int QtMaterial { get; set; }
        private string Estado { get; set; }
        public DateTime dataEntrega { get; set; }

        //Metodo construtor responsavel pela inicialização usando
        //os parametros abaixo
        public OrdemProduto(string NomeProduto, int QuantidadeProduto, int QuantidadeMaterial, string Estado)
        {
            this.NomeProduto = NomeProduto;
            this.QtProduto = QuantidadeProduto;
            this.QtMaterial = QuantidadeMaterial;
            this.Estado = Estado;
        }

        //Metodo de Exibição das paramentros que foram salvos pelo usuario
        //com uma estimativa fixada de entrega dos itens em 10 dias
        //E o estado do item se esta em produção ou concluido
        public void Exibir()
        {
            dataEntrega = DateTime.Now;
            Console.WriteLine($"\nNome do Produto: {NomeProduto}" +
                $"\nQuantidade para Produção: {QtProduto}" +
                $"\nData da Entrega: {dataEntrega.AddDays(10)}" +
                $"\nEstado: {Estado}" +
                $"\n=============================================================");
        }

        //Este metodo recebe um parametro para posibilitar a alteração dele
        //ele e acessado através de referencia da interface
        void IOrdemProduto.AtualizarEstado(string estado)
        {
            Estado = estado;
        }

    }
}
