// 19169 - Felipe Pires Araujo
// 19196 - Rafael Romanhole Borrozino

using apCaminhosMarte.Classes.GPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apCaminhosMarte.Classes.GPS.Dijkstra;
using apCaminhosMarte.Classes.GPS.Recursao;
using apCaminhosMarte.Classes.GPS.Pilhas;

namespace apCaminhosMarte
{
    enum CriterioMelhorCaminho
    {
        Custo,
        Distancia,
        Tempo
    }

    enum MetodoDeBusca
    {
        Pilhas,
        Recursao,
        Dijkstra
    }

    class GPS
    {
        CriterioMelhorCaminho criterio;
        MetodoDeBusca metodo;
        Arvore<Cidade> arvore;
        List<Cidade> listaCidades;
        List<Movimento> listaMovimentos;        
        List<Caminho> caminhosEncontrados;
        Movimento[,] matrizAdjacencia;    

        /**
         * Construtor, cada atributo é instanciado ou tem seus dados carregados.
         */
        public GPS()
        {
            arvore = new Arvore<Cidade>();
            listaCidades = new List<Cidade>();
            listaMovimentos = new List<Movimento>();            

            arvore = Leitor.LerCidades();
            listaCidades = arvore.getListaOrdenada();
            listaMovimentos = Leitor.LerMovimentos();
            
            matrizAdjacencia = MontarMatrizAdjacencia();            
        }

        /**
         * Método que retorna a matriz de adjacência, que representa o matrizAdjacencia de cidades.
         * Primeiramente, é criada uma matriz com o tamanho máximo equivalente à quantidade de cidades nos arquivos texto.
         * Após isso, são usados 2 "for" para percorrer os elementos da lista de caminho e verificar se existem caminhos equivalentes ao índices.
         * Se sim, a distância do caminho é armazenada na matriz de adjacência.
         */
        public Movimento[,] MontarMatrizAdjacencia()
        {
            Movimento[,] matrizAdjacencia = new Movimento[QtdCidades, QtdCidades];

            for (int i = 0; i < QtdCidades; i++)
                for (int j = 0; j < QtdCidades; j++)
                    foreach (Movimento mov in ListaMovimentos)
                        if (mov.IdCidadeOrigem == i && mov.IdCidadeDestino == j)
                            matrizAdjacencia[i, j] = mov;

            return matrizAdjacencia;
        } 
        
        public void BuscarCaminhosDijkstra(int idCidadeOrigem, int idCidadeDestino)
        {
            Dijkstra algoritmoDijkstra = new Dijkstra();
            caminhosEncontrados = algoritmoDijkstra.BuscarCaminhos(idCidadeOrigem, idCidadeDestino);
        }


        
        public void BuscarCaminhosRecursivo(int idCidadeOrigem, int idCidadeDestino)
        {
            Recursao algoritmoRecursivo = new Recursao();
            caminhosEncontrados = algoritmoRecursivo.BuscarCaminhos(idCidadeOrigem, idCidadeDestino);
        }

        public void BuscarCaminhosPilhas(int idCidadeOrigem, int idCidadeDestino)
        {
            Pilhas algoritmoPilhas = new Pilhas();
            caminhosEncontrados = algoritmoPilhas.BuscarCaminhos(idCidadeOrigem, idCidadeDestino);            
        }

        /**
         * Método que retorna o melhor caminho entre duas cidades.
         * É passado como parâmetro a lista "caminhos", que representa todos os caminhos possíveis entre determinadas cidades.
         * Dessa forma, são declarados uma Pilha de retorno e uma matriz, que representará a matriz de adjacência.
         * Cada caminho da lista passada como parâmetro é percorrido e tem seu critério (distância, tempo ou custo) total calculado.
         * Por fim, a pilhaDeMovimentos de retorno recebe o caminho com o menor critério total e é retornado.
         */
        public Caminho BuscarMelhorCaminho(List<Caminho> caminhos)
        {
            Caminho ret = new Caminho();
            int menorPeso = 0; // critério -> distância, tempo ou custo (o que for escolhido pelo usuário)

            foreach (Caminho caminho in caminhos)
            {
                Caminho caminhoClone = (Caminho) caminho.Clone(); // caminhoClone para não desempilhar o caminho que deve ser retornado.
                int pesoTotal = 0;
                while (!caminhoClone.Movimentos.EstaVazia)
                {
                    int idCidadeOrigem = caminhoClone.Movimentos.OTopo().IdCidadeOrigem;
                    int idCidadeDestino = caminhoClone.Movimentos.OTopo().IdCidadeDestino;

                    switch (this.Criterio)
                    {
                        case CriterioMelhorCaminho.Distancia:
                            pesoTotal += matrizAdjacencia[idCidadeOrigem, idCidadeDestino].Distancia;
                            break;

                        case CriterioMelhorCaminho.Tempo:
                            pesoTotal += matrizAdjacencia[idCidadeOrigem, idCidadeDestino].Tempo;
                            break;

                        case CriterioMelhorCaminho.Custo:
                            pesoTotal += matrizAdjacencia[idCidadeOrigem, idCidadeDestino].Custo;
                            break;
                    }
                    caminhoClone.RemoverMovimento();

                }
                if (pesoTotal < menorPeso || menorPeso == 0) // se a distância total do caminho em questão for menor que todas as outras até o momento
                {
                    ret = (Caminho) caminho.Clone();
                    menorPeso = pesoTotal;
                    ret.PesoTotal = menorPeso;
                }
            }
            return ret;
        }

        public Arvore<Cidade> Arvore { get => arvore; set => arvore = value; }
        public List<Cidade> ListaCidades { get => listaCidades; set => listaCidades = value; }
        public List<Movimento> ListaMovimentos { get => listaMovimentos; set => listaMovimentos = value; }        
        public int QtdCidades { get => listaCidades.Count; }
        public List<Caminho> CaminhosEncontrados { get => caminhosEncontrados; set => caminhosEncontrados = value; }
        public Movimento[,] MatrizAdjacencia { get => matrizAdjacencia; set => matrizAdjacencia = value; }
        public CriterioMelhorCaminho Criterio { get => criterio; set => criterio = value; }
        public MetodoDeBusca Metodo { get => metodo; set => metodo = value; }
    }
}
