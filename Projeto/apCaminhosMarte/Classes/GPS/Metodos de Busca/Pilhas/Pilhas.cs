using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte.Classes.GPS.Pilhas
{
    class Pilhas : apCaminhosMarte.GPS
    {
        List<Caminho> caminhosEncontrados;
        PilhaLista<Movimento> pilhaAuxiliar;

        public Pilhas()
        {
            caminhosEncontrados = new List<Caminho>();
            pilhaAuxiliar = new PilhaLista<Movimento>();
        }

        public List<Caminho> BuscarCaminhos(int idCidadeOrigem, int idCidadeDestino)
        {
            int idCidadeAtual, idSaidaAtual;
            bool achouCaminho = false, naoTemSaida = false;
            bool[] jaPassou = new bool[QtdCidades];

            for (int indice = 0; indice < QtdCidades; indice++)
                jaPassou[indice] = false;

            idCidadeAtual = idCidadeOrigem;
            idSaidaAtual = 0;
            while (!naoTemSaida)
            {
                naoTemSaida = (idCidadeAtual == idCidadeOrigem && idSaidaAtual == QtdCidades && pilhaAuxiliar.EstaVazia);
                if (!naoTemSaida)
                {
                    while ((idSaidaAtual < QtdCidades) && !achouCaminho)
                    {

                        if (MatrizAdjacencia[idCidadeAtual, idSaidaAtual] == null)
                            idSaidaAtual++;

                        else if (jaPassou[idSaidaAtual])
                            idSaidaAtual++;

                        else if (idSaidaAtual == idCidadeDestino)
                        {
                            Movimento movimento = new Movimento(idCidadeAtual, idSaidaAtual);
                            pilhaAuxiliar.Empilhar(movimento);
                            achouCaminho = true;
                        }
                        else
                        {
                            Movimento movimento = new Movimento(idCidadeAtual, idSaidaAtual);
                            pilhaAuxiliar.Empilhar(movimento);
                            jaPassou[idCidadeAtual] = true;
                            idCidadeAtual = idSaidaAtual;
                            idSaidaAtual = 0;
                        }
                    }
                }
                if (!achouCaminho)
                    if (!pilhaAuxiliar.EstaVazia)
                    {
                        jaPassou[idCidadeAtual] = false;
                        Movimento ultimoMovimento = pilhaAuxiliar.Desempilhar();
                        idSaidaAtual = ultimoMovimento.IdCidadeDestino;
                        idCidadeAtual = ultimoMovimento.IdCidadeOrigem;
                        idSaidaAtual++;
                    }
                if(achouCaminho)
                {
                    Caminho novoCaminho = new Caminho();
                    novoCaminho.Movimentos = pilhaAuxiliar.Clone();
                    caminhosEncontrados.Add(novoCaminho);
                    achouCaminho = false;
                    pilhaAuxiliar.Desempilhar();

                    for (int i = 0; i < QtdCidades; i++)
                        jaPassou[i] = false; 

                    idSaidaAtual++;
                }
            }
            return caminhosEncontrados;
        }
    }
}
