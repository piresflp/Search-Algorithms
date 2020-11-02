// 19169 - Felipe Pires Araujo
// 19196 - Rafael Romanhole Borrozino

using apCaminhosMarte.Classes.GPS;
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
        List<Caminho> caminhosEncontrados;
        bool[] jaPassou;
        int[,] matrizAdjacencia;
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
            CaminhosEncontrados = new List<Caminho>();
            JaPassou = new bool[ListaCidades.Count];
            MatrizAdjacencia = montarMatrizAdjacencia();
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
         * Método que retorna a matriz de adjacência, que representa o matrizAdjacencia de cidades.
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
        public void buscarCaminhos(int idCidadeOrigem, int idCidadeDestino)
        {
            for (int i = 0; i < ListaCidades.Count; i++)
            {
                if (MatrizAdjacencia[idCidadeOrigem, i] != 0 && jaPassou[i] == false)
                {
                    pilhaMovimento.Empilhar(new Movimento(idCidadeOrigem, i));
                    jaPassou[i] = true;

                    if (i == idCidadeDestino) // se chegou ao destino
                    {
                        Caminho novoCaminho = new Caminho();
                        novoCaminho.Movimentos = pilhaMovimento.Clone();
                        CaminhosEncontrados.Add(novoCaminho);
                        pilhaMovimento.Desempilhar(); // busca novos caminhos
                        jaPassou[i] = false;
                    }
                    else
                        buscarCaminhos(i, idCidadeDestino); // backtracking
                }
            }
            if (!pilhaMovimento.EstaVazia)
            {
                pilhaMovimento.Desempilhar();
                jaPassou[idCidadeOrigem] = false;
            }
        }

        /**
         * Método que retorna o melhor caminho entre duas cidades.
         * É passado como parâmetro a lista "caminhos", que representa todos os caminhos possíveis entre determinadas cidades.
         * Dessa forma, são declarados uma Pilha de retorno e uma matriz, que representará a matriz de adjacência.
         * Cada caminho da lista passada como parâmetro é percorrido e tem seu critério (distância, tempo ou custo) total calculado.
         * Por fim, a pilha de retorno recebe o caminho com o menor critério total e é retornado.
         */
        public Caminho buscarMelhorCaminho(List<Caminho> caminhos)
        {
            Caminho ret = new Caminho();
            int menorCriterio = 0; // critério -> distância, tempo ou custo (o que for escolhido pelo usuário)
            int[,] matriz = this.MatrizAdjacencia;

            foreach (Caminho caminho in caminhos)
            {
                Caminho caminhoClone = caminho.Clone(); // caminhoClone para não desempilhar o caminho que deve ser retornado.
                int criterioTotal = 0;
                while (!caminhoClone.Movimentos.EstaVazia)
                {
                    int idCidadeOrigem = caminhoClone.Movimentos.OTopo().IdCidadeOrigem;
                    int idCidadeDestino = caminhoClone.Movimentos.OTopo().IdCidadeDestino;
                    criterioTotal += matriz[idCidadeOrigem, idCidadeDestino];
                    caminhoClone.removerMovimento();
                }
                if (criterioTotal < menorCriterio || menorCriterio == 0) // se a distância total do caminho em questão for menor que todas as outras até o momento
                {
                    ret = caminho.Clone(); 
                    menorCriterio = criterioTotal;
                }
            }
            return ret;
        }
        public Arvore<Cidade> Arvore { get => arvore; set => arvore = value; }
        public List<Cidade> ListaCidades { get => listaCidades; set => listaCidades = value; }
        public List<Movimento> ListaCaminhos { get => listaCaminhos; set => listaCaminhos = value; }
        public PilhaLista<Movimento> PilhaMovimento { get => pilhaMovimento; set => pilhaMovimento = value; }
        public List<Caminho> CaminhosEncontrados { get => caminhosEncontrados; set => caminhosEncontrados = value; }
        public bool[] JaPassou { get => jaPassou; set => jaPassou = value; }
        public int[,] MatrizAdjacencia { get => matrizAdjacencia; set => matrizAdjacencia = value; }
        public bool SemSaida { get => semSaida; set => semSaida = value; }
        public CriterioMelhorCaminho Criterio { get => criterio; set => criterio = value; }
        public MetodoDeBusca Metodo { get => metodo; set => metodo = value; }
    }
}
