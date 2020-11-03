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
        List<Movimento> listaMovimentos;
        PilhaLista<Movimento> pilhaMovimento;
        List<Caminho> caminhosEncontrados;
        bool[] jaPassou;
        Movimento[,] matrizAdjacencia;
        bool semSaida;
        CriterioMelhorCaminho criterio;
        MetodoDeBusca metodo;

        /**
         * Construtor, cada atributo é instanciado ou tem seus dados carregados.
         */
        public GPS()
        {
            arvore = new Arvore<Cidade>();
            listaCidades = new List<Cidade>();
            listaMovimentos = new List<Movimento>();
            pilhaMovimento = new PilhaLista<Movimento>();
            caminhosEncontrados = new List<Caminho>();

            arvore = Leitor.lerCidades();
            listaCidades = arvore.getListaOrdenada();
            listaMovimentos = Leitor.lerMovimentos();
            
            jaPassou = new bool[listaCidades.Count];
            matrizAdjacencia = montarMatrizAdjacencia();
        }

        /**
         * Método que retorna a matriz de adjacência, que representa o matrizAdjacencia de cidades.
         * Primeiramente, é criada uma matriz com o tamanho máximo equivalente à quantidade de cidades nos arquivos texto.
         * Após isso, são usados 2 "for" para percorrer os elementos da lista de caminho e verificar se existem caminhos equivalentes ao índices.
         * Se sim, a distância do caminho é armazenada na matriz de adjacência.
         */
        public Movimento[,] montarMatrizAdjacencia()
        {
            int qtdCidades = listaCidades.Count;
            Movimento[,] matriz = new Movimento[qtdCidades, qtdCidades];

            for (int i = 0; i < qtdCidades; i++)
                for (int j = 0; j < qtdCidades; j++)
                    foreach (Movimento mov in ListaMovimentos)
                        if (mov.IdCidadeOrigem == i && mov.IdCidadeDestino == j)
                            matriz[i, j] = mov;

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
            for (int i = 0; i < listaCidades.Count; i++)
            {
                if (matrizAdjacencia[idCidadeOrigem, i] != null && jaPassou[i] == false)
                {
                    pilhaMovimento.Empilhar(new Movimento(idCidadeOrigem, i));
                    jaPassou[i] = true;

                    if (i == idCidadeDestino) // se chegou ao destino
                    {
                        Caminho novoCaminho = new Caminho();
                        novoCaminho.Movimentos = pilhaMovimento.Clone();
                        caminhosEncontrados.Add(novoCaminho);
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
            int menorPeso = 0; // critério -> distância, tempo ou custo (o que for escolhido pelo usuário)

            foreach (Caminho caminho in caminhos)
            {
                Caminho caminhoClone = caminho.Clone(); // caminhoClone para não desempilhar o caminho que deve ser retornado.
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
                    caminhoClone.removerMovimento();

                }
                if (pesoTotal < menorPeso || menorPeso == 0) // se a distância total do caminho em questão for menor que todas as outras até o momento
                {
                    ret = caminho.Clone();
                    menorPeso = pesoTotal;
                    ret.PesoTotal = menorPeso;
                }
            }
            return ret;
        }
        public Arvore<Cidade> Arvore { get => arvore; set => arvore = value; }
        public List<Cidade> ListaCidades { get => listaCidades; set => listaCidades = value; }
        public List<Movimento> ListaMovimentos { get => listaMovimentos; set => listaMovimentos = value; }
        public PilhaLista<Movimento> PilhaMovimento { get => pilhaMovimento; set => pilhaMovimento = value; }
        public List<Caminho> CaminhosEncontrados { get => caminhosEncontrados; set => caminhosEncontrados = value; }
        public bool[] JaPassou { get => jaPassou; set => jaPassou = value; }
        public Movimento[,] MatrizAdjacencia { get => matrizAdjacencia; set => matrizAdjacencia = value; }
        public bool SemSaida { get => semSaida; set => semSaida = value; }
        public CriterioMelhorCaminho Criterio { get => criterio; set => criterio = value; }
        public MetodoDeBusca Metodo { get => metodo; set => metodo = value; }
    }
}
