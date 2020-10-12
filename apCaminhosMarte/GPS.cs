﻿using System;
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
        PilhaLista<Caminho> pilhaMovimento;
        List<PilhaLista<Caminho>> caminhosEncontrados;
        bool[] passou;
        int[,] grafo;
        bool semSaida = false;

        public Arvore<Cidade> Arvore { get => arvore; set => arvore = value; }
        public List<Cidade> ListaCidades { get => listaCidades; set => listaCidades = value; }
        public List<Caminho> ListaCaminhos { get => listaCaminhos; set => listaCaminhos = value; }
        public PilhaLista<Caminho> PilhaMovimento { get => pilhaMovimento; set => pilhaMovimento = value; }
        public List<PilhaLista<Caminho>> CaminhosEncontrados { get => caminhosEncontrados; set => caminhosEncontrados = value; }
        public bool[] Passou { get => passou; set => passou = value; }
        public int[,] Grafo { get => grafo; set => grafo = value; }
        public bool SemSaida { get => semSaida; set => semSaida = value; }

        public GPS()
        {
            Arvore = new Arvore<Cidade>();
            ListaCidades = new List<Cidade>();
            ListaCaminhos = new List<Caminho>();
            CarregarDados();
            PilhaMovimento = new PilhaLista<Caminho>();
            CaminhosEncontrados = new List<PilhaLista<Caminho>>();
            Passou = new bool[ListaCidades.Count];
            for (int i = 0; i < ListaCidades.Count; i++)
                passou[i] = false;
            Grafo = montarMatrizAdjacencia();
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

        public void buscarCaminhos(int idOrigem, int idDestino)
        {
            for (int i = 0; i < ListaCidades.Count; i++)
            {
                if (Grafo[idOrigem, i] != 0 && passou[i] == false)
                {
                    pilhaMovimento.Empilhar(new Caminho(idOrigem, i));
                    passou[i] = true;

                    if (i == idDestino)
                    {
                        caminhosEncontrados.Add(pilhaMovimento.Clone());
                        pilhaMovimento.Desempilhar();
                        passou[i] = false;
                    }
                    else
                        buscarCaminhos(i, idDestino);
                }
            }
            if (!pilhaMovimento.EstaVazia)
            {
                pilhaMovimento.Desempilhar();
                passou[idOrigem] = false;
            }
        }
    }
}
