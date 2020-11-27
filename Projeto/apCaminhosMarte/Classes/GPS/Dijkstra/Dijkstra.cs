using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte.Classes.GPS.Dijkstra
{
    class Dijkstra
    {
        Movimento[,] matrizAdjacencia;
        CriterioMelhorCaminho criterio;
        MetodoDeBusca metodo;

        public Dijkstra(Movimento[,] matrizAdjacencia, CriterioMelhorCaminho criterio, MetodoDeBusca metodo)
        {
            this.matrizAdjacencia = matrizAdjacencia;
            this.criterio = criterio;
            this.metodo = metodo;
        }

        private static int ObterIndiceDoMenor(int[] distancias, bool[] foiVisitado)
        {
            int min = int.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < foiVisitado.Length; ++v)
            {
                if (!foiVisitado[v] && distancias[v] <= min)
                {
                    min = distancias[v];
                    minIndex = v;
                }
            }
            return minIndex;
        }

        public void BuscarCaminhosDijkstra(int idCidadeOrigem, int idCidadeDestino)
        {
            int qtdVertices = matrizAdjacencia.Length;
            int[,] matrizAdjacenciaComPeso = new int[qtdVertices, qtdVertices];

            for (int i = 0; i < matrizAdjacenciaComPeso.Length; i++)
                for (int j = 0; j < matrizAdjacenciaComPeso.Length; j++)
                    matrizAdjacenciaComPeso[i, j] = int.MaxValue;

            switch (this.criterio)
            {
                case CriterioMelhorCaminho.Custo:
                    for (int i = 0; i < qtdVertices; i++)
                        for (int j = 0; j < qtdVertices; j++)
                            matrizAdjacenciaComPeso[i, j] = matrizAdjacencia[i, j].Custo;
                    break;

                case CriterioMelhorCaminho.Distancia:
                    for (int i = 0; i < qtdVertices; i++)
                        for (int j = 0; j < qtdVertices; j++)
                            matrizAdjacenciaComPeso[i, j] = matrizAdjacencia[i, j].Distancia;
                    break;

                case CriterioMelhorCaminho.Tempo:
                    for (int i = 0; i < qtdVertices; i++)
                        for (int j = 0; j < qtdVertices; j++)
                            matrizAdjacenciaComPeso[i, j] = matrizAdjacencia[i, j].Tempo;
                    break;
            }

            bool[] foiVisitado = new bool[qtdVertices];
            DistOriginal[] percursos = new DistOriginal[qtdVertices];
            for (int i = 0; i < qtdVertices; i++)
            {
                int distanciaTemporaria = matrizAdjacenciaComPeso[idCidadeOrigem, i];
                percursos[i] = new DistOriginal(idCidadeOrigem, distanciaTemporaria);
                foiVisitado[i] = false;
            }
            foiVisitado[idCidadeOrigem] = true;

            for (int i = 0; i < qtdVertices; i++)
            {
                int indiceDoMenor = ObterIndiceDoMenor(percursos, foiVisitado);
                foiVisitado[i] = true;

                int distanciaMinima = percursos[indiceDoMenor].distancia;
                int indiceAtual = indiceDoMenor;
                AjustarMenorCaminho(matrizAdjacenciaComPeso, percursos, foiVisitado, indiceAtual, distanciaMinima);
            }
        }

        public static void AjustarMenorCaminho(int[,] matrizAdjacencia, DistOriginal[] percursos, bool[] foiVisitado, int indiceAtual, int doInicioAteAtual)
        {
            for (int coluna = 0; coluna < foiVisitado.Length; coluna++)
                if (!foiVisitado[coluna])       // para cada vértice ainda não visitado
                {
                    // acessamos a distância desde o vértice atual (pode ser infinity)
                    int atualAteMargem = matrizAdjacencia[indiceAtual, coluna];

                    // calculamos a distância desde inicioDoPercurso passando por vertice atual até
                    // esta saída
                    int doInicioAteMargem = doInicioAteAtual + atualAteMargem;

                    // quando encontra uma distância menor, marca o vértice a partir do
                    // qual chegamos no vértice de índice coluna, e a soma da distância
                    // percorrida para nele chegar
                    int distanciaDoCaminho = percursos[coluna].distancia;
                    if (doInicioAteMargem < distanciaDoCaminho)
                    {
                        percursos[coluna].verticePai = indiceAtual;
                        percursos[coluna].distancia = doInicioAteMargem;
                    }
                }
        }

        private static int ObterIndiceDoMenor(DistOriginal[] percursos, bool[] foiVisitado)
        {
            int distanciaMinima = int.MaxValue;
            int indiceDaMinima = 0;

            for (int i = 0; i < foiVisitado.Length; i++)
                if (!(foiVisitado[i]) && (percursos[i].distancia < distanciaMinima))
                {
                    distanciaMinima = percursos[i].distancia;
                    indiceDaMinima = i;
                }
            return indiceDaMinima;
        }
    }
}
