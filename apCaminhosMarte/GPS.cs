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

        public List<PilhaLista<Caminho>> buscarCaminhos(int idOrigem, int idDestino, ref PilhaLista<Caminho> melhorCaminho)
        {
            int cidadeAtual, saidaAtual, menorDistancia = 0;
            bool achouCaminho = false,
            naoTemSaida = false;
            int qtsCidades = ListaCidades.Count;
            bool[] passou = new bool[qtsCidades];
            int[,] grafo = montarMatrizAdjacencia();
            List<PilhaLista<Caminho>> caminhos = new List<PilhaLista<Caminho>>();

            for (int indice = 0; indice < qtsCidades; indice++)
                passou[indice] = false;

            cidadeAtual = idOrigem;
            saidaAtual = 0;
            var pilha = new PilhaLista<Caminho>();
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
                            Caminho movimento = new Caminho(cidadeAtual, saidaAtual);
                            pilha.Empilhar(movimento);
                            achouCaminho = true;
                        }
                        else
                        {
                            Caminho movimento = new Caminho(cidadeAtual, saidaAtual);
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
                        passou[cidadeAtual] = false;
                        var movimento = pilha.Desempilhar();
                        saidaAtual = movimento.IdCidadeDestino;
                        cidadeAtual = movimento.IdCidadeOrigem;
                        saidaAtual++;
                    }
                var saida = new PilhaLista<Caminho>();
                if (achouCaminho)
                {                    
                    PilhaLista<Caminho> clone = new PilhaLista<Caminho>();
                    clone = pilha.Clone();
                    while (!clone.EstaVazia)
                    { 
                        Caminho movimento = clone.Desempilhar();
                        saida.Empilhar(movimento);
                    }
                    caminhos.Add(saida);
                    achouCaminho = false;
                    pilha.Desempilhar();

                    for (int i = 0; i < qtsCidades; i++)
                        passou[i] = false;

                    saidaAtual++;

                    int distanciaTotal = 0;                    
                }
            }            
            return caminhos;
        }
    }
}
