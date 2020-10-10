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
            int qtdCidades = ListaCidades.Count;
            int[,] matriz = new int[qtdCidades, qtdCidades];

            for (int i = 0; i < qtdCidades; i++)
                for (int j = 0; j < qtdCidades; j++)
                    foreach (Caminho c in ListaCaminhos)
                        if (c.IdCidadeOrigem == i && c.IdCidadeDestino == j)
                            matriz[i, j] = c.Distancia;

            return matriz;
        }

        public List<PilhaLista<Movimento>> buscarCaminhos(int idOrigem, int idDestino)
        {
            int cidadeAtual, saidaAtual;
            bool achouCaminho = false,
            naoTemSaida = false;
            int qtsCidades = ListaCidades.Count;
            bool[] passou = new bool[qtsCidades];
            int[,] grafo = montarMatrizAdjacencia();
            List<PilhaLista<Movimento>> caminhos = new List<PilhaLista<Movimento>>();

            for (int indice = 0; indice < qtsCidades; indice++)
                passou[indice] = false;

            cidadeAtual = idOrigem;
            saidaAtual = 0;
            var pilha = new PilhaLista<Movimento>();
            while (!naoTemSaida)
            {
                naoTemSaida = (cidadeAtual == idOrigem && saidaAtual == 23 && pilha.EstaVazia);
                if (!naoTemSaida)
                {
                    while ((saidaAtual < qtsCidades) && !achouCaminho)
                    {

                        if (grafo[cidadeAtual, saidaAtual] == 0)
                            saidaAtual++;
                        else

                        if (passou[saidaAtual])
                            saidaAtual++;
                        else

                        if (saidaAtual == idDestino)
                        {
                            Movimento movimento = new Movimento(cidadeAtual, saidaAtual);
                            pilha.Empilhar(movimento);
                            achouCaminho = true;
                        }
                        else
                        {
                            Movimento movimento = new Movimento(cidadeAtual, saidaAtual);
                            pilha.Empilhar(movimento);
                            passou[cidadeAtual] = true;
                            cidadeAtual = saidaAtual;
                            saidaAtual = 0;
                        }
                    }
                }
                if (!achouCaminho)
                    if (!pilha.EstaVazia)
                    {
                        var movimento = pilha.Desempilhar();
                        saidaAtual = movimento.IdDestino;
                        cidadeAtual = movimento.IdOrigem;
                        saidaAtual++;
                    }
                var saida = new PilhaLista<Movimento>();
                if (achouCaminho)
                {
                    PilhaLista<Movimento> clone = new PilhaLista<Movimento>();
                    clone = pilha.Clone();
                    while (!clone.EstaVazia)
                    { 
                        Movimento movimento = clone.Desempilhar();
                        saida.Empilhar(movimento);
                    }
                    caminhos.Add(saida);
                    achouCaminho = false;
                    pilha.Desempilhar();
                    saidaAtual++;
                }
            }
            return caminhos;
        }
    }
}
