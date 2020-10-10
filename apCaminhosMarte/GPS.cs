using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class GPS
    {
        Arvore<Cidade> arvore;
        List<Cidade> listaCidades;
        List<Caminho> listaCaminhos;

        public Arvore<Cidade> Arvore { get => arvore; set => arvore = value; }
        public List<Cidade> ListaCidades { get => listaCidades; set => listaCidades = value; }
        public List<Caminho> ListaCaminhos { get => listaCaminhos; set => listaCaminhos = value; }

        public GPS()
        {
            Arvore = new Arvore<Cidade>();
            ListaCidades = new List<Cidade>();
            ListaCaminhos = new List<Caminho>();
            CarregarDados();
        }

        private void CarregarDados()
        {
            Arvore = Leitor.lerCidades();
            ListaCidades = Arvore.getListaOrdenada();
            ListaCaminhos = Leitor.lerCaminhos();
        }

        public int[,] montarMatrizAdjacencia()
        {
            int qtdCidades = ListaCidades.Count - 1;
            int[,] matriz = new int[qtdCidades, qtdCidades];

            for (int i = 0; i < qtdCidades; i++)
                for (int j = 0; j < qtdCidades; j++)
                    foreach (Caminho c in ListaCaminhos)
                        if (c.IdCidadeOrigem == i && c.IdCidadeDestino == j)
                            matriz[i, j] = c.Distancia;

            return matriz;
        }

        public void buscarCaminhos(int idOrigem, int idDestino)
        {
            
        }
    }
}
