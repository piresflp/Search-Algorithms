// 19169 - Felipe Pires Araujo
// 19196 - Rafael Romanhole Borrozino

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Arvore<Cidade> arvore;
        List<Cidade> listaCidades;
        List<Movimento> listaCaminhos;
        PilhaLista<Movimento> pilhaMovimento;
        List<PilhaLista<Movimento>> caminhosEncontrados;
        bool[] passou;
        int[,] grafo;
        bool semSaida = false;
        CriterioMelhorCaminho criterio;
        MetodoDeBusca metodo;

        /**
         * Construtor, cada atributo é instanciado ou tem seus dados carregados.
         */
        public GPS()
        {
            Arvore = new Arvore<Cidade>();
            ListaCidades = new List<Cidade>();
            ListaCaminhos = new List<Movimento>();
            CarregarDados();
            PilhaMovimento = new PilhaLista<Movimento>();
            CaminhosEncontrados = new List<PilhaLista<Movimento>>();
            Passou = new bool[ListaCidades.Count];
            Grafo = montarMatrizAdjacencia();
        }

        /**
         *  Método responsável por chamar os métodos da classe Leitor, que lêem os dados do arquivo texto, e carregar a resposta em seus devidos atributos
         **/
        private void CarregarDados()
        {
            Arvore = Leitor.lerCidades();
            ListaCidades = Arvore.getListaOrdenada();
            ListaCaminhos = Leitor.lerCaminhos();
        }

        /**
         * Método que retorna a matriz de adjacência, que representa o grafo de cidades.
         * Primeiramente, é criada uma matriz com o tamanho máximo equivalente à quantidade de cidades nos arquivos texto.
         * Após isso, são usados 2 "for" para percorrer os elementos da lista de caminho e verificar se existem caminhos equivalentes ao índices.
         * Se sim, a distância do caminho é armazenada na matriz de adjacência.
         */
        public int[,] montarMatrizAdjacencia()
        {
            int qtdCidades = ListaCidades.Count;
            int[,] matriz = new int[qtdCidades, qtdCidades];

            for (int i = 0; i < qtdCidades; i++)
                for (int j = 0; j < qtdCidades; j++)
                    foreach (Movimento c in ListaCaminhos)
                        if (c.IdCidadeOrigem == i && c.IdCidadeDestino == j)
                            switch (this.Criterio)
                            {
                                case CriterioMelhorCaminho.Distancia:
                                    matriz[i, j] = c.Distancia;
                                    break;

                                case CriterioMelhorCaminho.Tempo:
                                    matriz[i, j] = c.Tempo;
                                    break;

                                case CriterioMelhorCaminho.Custo:
                                    matriz[i, j] = c.Custo;
                                    break;
                            }

            return matriz;
        }

        /**
         * Método recursivo que retorna todos os caminhos entre as cidades que possuem o Id equivalentes aos parâmetros.
         * Cada passo do caminho em questão é armazenado na pilha de movimentos "pilhaMovimento".
         * Por fim, se chegou ao destino, a pilha é armazenada na variável "caminhosEncontrados", representando um dos caminhos possíveis
         * O método acaba quando todos os caminhos possíveis são percorridos.
         */
        public void buscarCaminhos(int idOrigem, int idDestino)
        {
            for (int i = 0; i < ListaCidades.Count; i++)
            {
                if (Grafo[idOrigem, i] != 0 && passou[i] == false)
                {
                    pilhaMovimento.Empilhar(new Movimento(idOrigem, i));
                    passou[i] = true;

                    if (i == idDestino) // se chegou ao destino
                    {
                        caminhosEncontrados.Add(pilhaMovimento.Clone());
                        pilhaMovimento.Desempilhar(); // busca novos caminhos
                        passou[i] = false;
                    }
                    else
                        buscarCaminhos(i, idDestino); // backtracking
                }
            }
            if (!pilhaMovimento.EstaVazia)
            {
                pilhaMovimento.Desempilhar();
                passou[idOrigem] = false;
            }
        }

        /**
         * Método que retorna o melhor caminho entre duas cidades.
         * É passado como parâmetro a lista "caminhos", que representa todos os caminhos possíveis entre determinadas cidades.
         * Dessa forma, são declarados uma Pilha de retorno e uma matriz, que representará a matriz de adjacência.
         * Cada caminho da lista passada como parâmetro é percorrido e tem sua distância total calculada.
         * Por fim, a pilha de retorno recebe o caminho com a menor distância total e é retornada.
         */
        public PilhaLista<Movimento> buscarMelhorCaminho(List<PilhaLista<Movimento>> caminhos)
        {
            PilhaLista<Movimento> ret = new PilhaLista<Movimento>();
            int menorDistancia = 0;
            int[,] matriz = this.Grafo;

            foreach (PilhaLista<Movimento> caminho in caminhos)
            {
                PilhaLista<Movimento> clone = caminho.Clone(); // clone para não desempilhar o caminho que deve ser retornado.
                int distanciaTotal = 0;
                while (!clone.EstaVazia)
                {
                    int idOrigem = clone.OTopo().IdCidadeOrigem;
                    int idDestino = clone.OTopo().IdCidadeDestino;
                    distanciaTotal += matriz[idOrigem, idDestino];
                    clone.Desempilhar();
                }
                if (distanciaTotal < menorDistancia || menorDistancia == 0) // se a distância total do caminho em questão for menor que todas as outras até o momento
                {
                    ret = caminho.Clone(); 
                    menorDistancia = distanciaTotal;
                }
            }
            return ret;
        }
        public Arvore<Cidade> Arvore { get => arvore; set => arvore = value; }
        public List<Cidade> ListaCidades { get => listaCidades; set => listaCidades = value; }
        public List<Movimento> ListaCaminhos { get => listaCaminhos; set => listaCaminhos = value; }
        public PilhaLista<Movimento> PilhaMovimento { get => pilhaMovimento; set => pilhaMovimento = value; }
        public List<PilhaLista<Movimento>> CaminhosEncontrados { get => caminhosEncontrados; set => caminhosEncontrados = value; }
        public bool[] Passou { get => passou; set => passou = value; }
        public int[,] Grafo { get => grafo; set => grafo = value; }
        public bool SemSaida { get => semSaida; set => semSaida = value; }
        public CriterioMelhorCaminho Criterio { get => criterio; set => criterio = value; }
        public MetodoDeBusca Metodo { get => metodo; set => metodo = value; }
    }
}
