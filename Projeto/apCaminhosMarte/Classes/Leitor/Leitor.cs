// 19169 - Felipe Pires Araujo
// 19196 - Rafael Romanhole Borrozino

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    static class Leitor
    {

        /**
         * Método responsável por ler o arquivo "CidadesMarte.txt" e armazenar seus dados numa Arvore de cidades, que será retornada.
         */
        public static Arvore<Cidade> lerCidades()
        {
            StreamReader leitor = new StreamReader("CidadesMarte.txt");
            Arvore<Cidade> arvore = new Arvore<Cidade>();

            while (!leitor.EndOfStream)
            {
                String linha = leitor.ReadLine();
                int idCidade = int.Parse(linha.Substring(0, 3).Trim());
                String nome = linha.Substring(3, 16).Trim();
                int coordenadaX = int.Parse(linha.Substring(19, 5).Trim());
                int coordenadaY = int.Parse(linha.Substring(24, 4).Trim());
                Cidade novaCidade = new Cidade(idCidade, nome, coordenadaX, coordenadaY);
                arvore.Raiz = arvore.InserirBalanceado(novaCidade, arvore.Raiz);
            }
            return arvore;
        }

        /**
         * Método que le o arquivo "CaminhosEntreCidadesMarte.txt" e retorna os seus dados numa lista genérica de caminhos.
         */ 
        public static List<Movimento> lerMovimentos()
        {
            StreamReader leitor = new StreamReader("CaminhosEntreCidadesMarte.txt");
            List<Movimento> listaMovimentos = new List<Movimento>();
            while (!leitor.EndOfStream)
            {
                String linha = leitor.ReadLine();
                int idCidadeOrigem = int.Parse(linha.Substring(0, 4).Trim());
                int idCidadeDestino = int.Parse(linha.Substring(4, 3).Trim());
                int distancia = int.Parse(linha.Substring(7, 6).Trim());
                int tempo = int.Parse(linha.Substring(13, 4).Trim());
                int custo = int.Parse(linha.Substring(17, 3).Trim());

                Movimento caminho = new Movimento(idCidadeOrigem, idCidadeDestino, distancia, tempo, custo);
                listaMovimentos.Add(caminho);
            }
            return listaMovimentos;
        }
    }
}
