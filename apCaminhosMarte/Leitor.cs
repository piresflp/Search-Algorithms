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
                arvore.Incluir(novaCidade);
            }
            return arvore;
        }
        public static List<Caminho> lerCaminhos()
        {
            StreamReader leitor = new StreamReader("CaminhosEntreCidadesMarte.txt");
            List<Caminho> listaCaminho = new List<Caminho>();
            while (!leitor.EndOfStream)
            {
                String linha = leitor.ReadLine();
                int idCidadeOrigem = int.Parse(linha.Substring(0, 4).Trim());
                int idCidadeDestino = int.Parse(linha.Substring(4, 3).Trim());
                int distancia = int.Parse(linha.Substring(7, 6).Trim());
                int tempo = int.Parse(linha.Substring(13, 4).Trim());
                int custo = int.Parse(linha.Substring(17, 3).Trim());

                Caminho caminho = new Caminho(idCidadeOrigem, idCidadeDestino, distancia, tempo, custo);
                listaCaminho.Add(caminho);
            }
            return listaCaminho;
        }
    }
}
