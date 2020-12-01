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
        public static Arvore<Cidade> LerCidades()
        {
            StreamReader leitorDeArquivos = new StreamReader("CidadesMarte.txt", Encoding.GetEncoding("iso-8859-1"));
            Arvore<Cidade> arvore = new Arvore<Cidade>();

            while (!leitorDeArquivos.EndOfStream)
            {
                String linhaLinha = leitorDeArquivos.ReadLine();
                int idCidade = int.Parse(linhaLinha.Substring(0, 3).Trim());
                String nomeCidade = linhaLinha.Substring(3, 16).Trim();
                int coordenadaX = int.Parse(linhaLinha.Substring(19, 5).Trim());
                int coordenadaY = int.Parse(linhaLinha.Substring(24, 4).Trim());

                Cidade novaCidade = new Cidade(idCidade, nomeCidade, coordenadaX, coordenadaY);
                arvore.Raiz = arvore.InserirBalanceado(novaCidade, arvore.Raiz);
            }
            return arvore;
        }

        /**
         * Método que le o arquivo "CaminhosEntreCidadesMarte.txt" e retorna os seus dados numa lista genérica de caminhos.
         */ 
        public static List<Movimento> LerMovimentos()
        {
            StreamReader leitorDeArquivos = new StreamReader("CaminhosEntreCidadesMarte.txt", Encoding.GetEncoding("iso-8859-1"));
            List<Movimento> listaMovimentos = new List<Movimento>();
            while (!leitorDeArquivos.EndOfStream)
            {
                String linhaLida = leitorDeArquivos.ReadLine();
                int idCidadeOrigem = int.Parse(linhaLida.Substring(0, 4).Trim());
                int idCidadeDestino = int.Parse(linhaLida.Substring(4, 3).Trim());
                int distancia = int.Parse(linhaLida.Substring(7, 6).Trim());
                int tempo = int.Parse(linhaLida.Substring(13, 4).Trim());
                int custo = int.Parse(linhaLida.Substring(17, 3).Trim());

                Movimento novoMovimento = new Movimento(idCidadeOrigem, idCidadeDestino, distancia, tempo, custo);
                listaMovimentos.Add(novoMovimento);
            }
            return listaMovimentos;
        }
    }
}
