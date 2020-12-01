using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte.Classes.GPS.Recursao
{
    class Recursao : apCaminhosMarte.GPS
    {
        bool[] jaPassou;
        PilhaLista<Movimento> pilhaMovimento;
        List<Caminho> caminhosEncontrados;

        public Recursao()
        {
            jaPassou = new bool[QtdCidades];
            pilhaMovimento = new PilhaLista<Movimento>();
            caminhosEncontrados = new List<Caminho>();
        }

        /**
         * Método recursivo que retorna todos os caminhos entre as cidades que possuem o Id equivalentes aos parâmetros.
         * Cada passo do caminho em questão é armazenado na pilhaDeMovimentos de movimentos "pilhaMovimento".
         * Por fim, se chegou ao destino, a pilhaDeMovimentos é armazenada na variável "caminhosEncontrados", representando um dos caminhos possíveis
         * O método acaba quando todos os caminhos possíveis são percorridos.
         */
        public List<Caminho> BuscarCaminhos(int idCidadeOrigem, int idCidadeDestino)
        {
            for (int i = 0; i < QtdCidades; i++)
                if ((MatrizAdjacencia[idCidadeOrigem, i] != null) && (!jaPassou[i]))
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
                        BuscarCaminhos(i, idCidadeDestino); // backtracking
                }

            if (!pilhaMovimento.EstaVazia)
            {
                pilhaMovimento.Desempilhar();
                jaPassou[idCidadeOrigem] = false;
            }
            return caminhosEncontrados;
        }

        public PilhaLista<Movimento> PilhaMovimento { get => pilhaMovimento; set => pilhaMovimento = value; }
    }
}
