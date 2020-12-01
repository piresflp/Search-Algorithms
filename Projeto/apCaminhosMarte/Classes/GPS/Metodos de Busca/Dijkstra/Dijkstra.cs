using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte.Classes.GPS.Dijkstra
{
    class Dijkstra : apCaminhosMarte.GPS
    {
        DistOriginal[] percursos;
        List<Caminho> caminhoEncontrado;
        int infinity = 1000000;
        Vertice[] vertices;
        int[,] matrizAdjacenciaComPeso;
        int verticeAtual;
        int doInicioAteAtual;

        public Dijkstra()
        {
            percursos = new DistOriginal[QtdCidades];
            caminhoEncontrado = new List<Caminho>();
            this.vertices = new Vertice[QtdCidades];

            this.matrizAdjacenciaComPeso = new int[QtdCidades, QtdCidades];

            for (int i = 0; i < matrizAdjacenciaComPeso.GetLength(0); i++)
                for (int j = 0; j < matrizAdjacenciaComPeso.GetLength(0); j++)
                {
                    if(MatrizAdjacencia[i,j] == null)
                        matrizAdjacenciaComPeso[i, j] = infinity; // define todos os valores da matriz de adjacencia pra infinity

                    else
                        switch (Criterio)
                        {
                            case CriterioMelhorCaminho.Custo:
                                        matrizAdjacenciaComPeso[i, j] = MatrizAdjacencia[i, j].Custo;
                                break;

                            case CriterioMelhorCaminho.Distancia:
                                        matrizAdjacenciaComPeso[i, j] = MatrizAdjacencia[i, j].Distancia;
                                break;

                            case CriterioMelhorCaminho.Tempo:
                                        matrizAdjacenciaComPeso[i, j] = MatrizAdjacencia[i, j].Tempo;
                                break;
                        }
                    }

            for (int i = 0; i < vertices.Length; i++)
                this.vertices[i] = new Vertice(Arvore.BuscarDado(new Cidade(i, "", 0, 0)));
        }        

        public List<Caminho> BuscarCaminhos(int idCidadeOrigem, int idCidadeDestino)
        {
            for (int j = 0; j < QtdCidades; j++)
                vertices[j].FoiVisitado = false;
            vertices[idCidadeOrigem].FoiVisitado = true;                      

            for (int i = 0; i < QtdCidades; i++)
            {
                int distanciaTemporaria = matrizAdjacenciaComPeso[idCidadeOrigem, i];
                percursos[i] = new DistOriginal(idCidadeOrigem, distanciaTemporaria);
            }

            for (int i = 0; i < QtdCidades; i++)
            {
                int indiceDoMenor = ObterIndiceDoMenor();
                vertices[indiceDoMenor].FoiVisitado = true;

                verticeAtual = indiceDoMenor;
                doInicioAteAtual = percursos[indiceDoMenor].distancia;

                AjustarMenorCaminho();
            }
            return SalvarResultados(idCidadeOrigem, idCidadeDestino);
        }
        private int ObterIndiceDoMenor()
        {
            int distanciaMinima = infinity;
            int indiceDaMinima = 0;

            for (int i = 0; i < QtdCidades; i++)
            {
                if (!(vertices[i].FoiVisitado) && (percursos[i].distancia < distanciaMinima))
                {
                    distanciaMinima = percursos[i].distancia;
                    indiceDaMinima = i;
                }
            }
            return indiceDaMinima;
        }

        private void AjustarMenorCaminho()
        {
            for (int coluna = 0; coluna < vertices.Length; coluna++)
                if (!vertices[coluna].FoiVisitado)       // para cada vértice ainda não visitado
                {
                    // acessamos a distância desde o vértice atual (pode ser infinity)
                    int atualAteMargem = matrizAdjacenciaComPeso[verticeAtual, coluna];

                    // calculamos a distância desde inicioDoPercurso passando por vertice atual até
                    // esta saída
                    int doInicioAteMargem = doInicioAteAtual + atualAteMargem;

                    // quando encontra uma distância menor, marca o vértice a partir do
                    // qual chegamos no vértice de índice coluna, e a soma da distância
                    // percorrida para nele chegar
                    int distanciaDoCaminho = percursos[coluna].distancia;
                    if (doInicioAteMargem < distanciaDoCaminho)
                    {
                        percursos[coluna].verticePai = verticeAtual;
                        percursos[coluna].distancia = doInicioAteMargem;
                    }
                }
        }

        private List<Caminho> SalvarResultados(int idCidadeOrigem, int idCidadeDestino)
        {
            int onde = idCidadeDestino;
            PilhaLista<Movimento> pilhaMovimentos = new PilhaLista<Movimento>();

            while (onde != idCidadeOrigem)
            {
                pilhaMovimentos.Empilhar(MatrizAdjacencia[vertices[percursos[onde].verticePai].Cidade.Id, vertices[onde].Cidade.Id]);
                onde = percursos[onde].verticePai;                
            }

            if (pilhaMovimentos.Tamanho > 1)
            {
                PilhaLista<Movimento> pilhaMovimentosInvertido = new PilhaLista<Movimento>();
                for (int i = pilhaMovimentos.Tamanho - 1; !pilhaMovimentos.EstaVazia; i--) // exibe cada movimento do melhor caminho
                {
                    Movimento mov = pilhaMovimentos.Desempilhar();
                    pilhaMovimentosInvertido.Empilhar(mov);
                }
                caminhoEncontrado.Add(new Caminho(pilhaMovimentosInvertido));
            }            

            return caminhoEncontrado;
        }

        private int ObterIndiceDoMenor(DistOriginal[] percursos)
        {
            int distanciaMinima = infinity;
            int indiceDaMinima = 0;

            for (int i = 0; i < vertices.Length; i++)
                if (!(vertices[i].FoiVisitado) && (percursos[i].distancia < distanciaMinima))
                {
                    distanciaMinima = percursos[i].distancia;
                    indiceDaMinima = i;
                }
            return indiceDaMinima;
        }
    }

    class Vertice
    {
        private Cidade cidade;
        private bool foiVisitado;
        private bool estaAtivo;

        public bool FoiVisitado { get => foiVisitado; set => foiVisitado = value; }
        public bool EstaAtivo { get => estaAtivo; set => estaAtivo = value; }
        public Cidade Cidade { get => cidade; set => cidade = value; }

        public Vertice(Cidade cidade)
        {
            this.cidade = cidade;
            FoiVisitado = false;
            estaAtivo = true;
        }
    }
    class DistOriginal
    {
        public int distancia;
        public int verticePai;
        public DistOriginal(int vp, int d)
        {
            distancia = d;
            verticePai = vp;
        }
    }
}

    



