using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    public partial class Form1 : Form
    {
        Arvore<Cidade> arvore;
        List<Cidade> listaCidade;
        List<Caminho> listaCaminho;
        public Form1()
        {
            InitializeComponent();
        }

        private void TxtCaminhos_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (lsbOrigem.SelectedIndex == -1)
                MessageBox.Show("Selecione a cidade de origem!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if(lsbDestino.SelectedIndex == -1)
                MessageBox.Show("Selecione a cidade de destino!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            String[] cidades = new String[7]{"Acheron", "Arena", "Arrakeen", "Bakhuysen", "Bradburry", "Burroughs", "Cairo"};
            String cidadeOrigem = cidades[lsbOrigem.SelectedIndex];
            String cidadeDestino = cidades[lsbDestino.SelectedIndex];
            int idOrigem = -1, idDestino = -1;

            foreach(Cidade c in listaCidade) {
                if (c.Nome.Equals(cidadeOrigem))
                    idOrigem = c.Id;

                else if (c.Nome.Equals(cidadeDestino))
                    idDestino = c.Id;

                if (idOrigem != -1 && idDestino != -1)
                    break;
            }            
            buscarCaminhos(idOrigem, idDestino);
        }

        private int[,] montarMatrizAdjacencia()
        {            
            int qtdCidades = listaCidade.Count;
            int[,] matriz = new int[qtdCidades, qtdCidades];

            for(int i = 0; i<matriz.Length;i++)

            return matriz;

        }

        private void buscarCaminhos(int idOrigem, int idDestino)
        {
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LerArquivos();
        }

        private void LerArquivos()
        {
            lerCidades();
            lerCaminhos();
        }

        private void lerCaminhos()
        {
            StreamReader leitor = new StreamReader("CaminhosEntreCidadesMarte.txt");
            listaCaminho = new List<Caminho>();
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
        }

        private void lerCidades()
        {
            StreamReader leitor = new StreamReader("CidadesMarte.txt");
            arvore = new Arvore<Cidade>();
            listaCidade = new List<Cidade>();
            while (!leitor.EndOfStream)
            {
                String linha = leitor.ReadLine();
                int idCidade = int.Parse(linha.Substring(0, 3).Trim());
                String nome = linha.Substring(3, 16).Trim();
                int coordenadaX = int.Parse(linha.Substring(19, 5).Trim());
                int coordenadaY = int.Parse(linha.Substring(24, 4).Trim());
                Cidade novaCidade = new Cidade(idCidade, nome, coordenadaX, coordenadaY);
                listaCidade.Add(novaCidade);
                Console.WriteLine(novaCidade);
                arvore.Incluir(novaCidade);
            }
        }
        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            double xProporcional = 4096.00 / pbMapa.Width;
            double yProporcional = 2048.00 / pbMapa.Height;
            foreach (Cidade c in listaCidade) {
                e.Graphics.FillEllipse(Brushes.Black, c.CoordenadaX / Convert.ToSingle(xProporcional), c.CoordenadaY / Convert.ToSingle(yProporcional), 7f, 7f);
                e.Graphics.DrawString(c.Nome, new Font("Comic Sans", 8, FontStyle.Bold), Brushes.Black, c.CoordenadaX / Convert.ToSingle(xProporcional)-15, c.CoordenadaY / Convert.ToSingle(yProporcional) -15);
            }
        }

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            arvore.DesenharArvore(true, arvore.Raiz, (int)tpArvore.Width / 2, 0, Math.PI / 2, Math.PI / 2.5, 300, e.Graphics);
        }
    }
}
