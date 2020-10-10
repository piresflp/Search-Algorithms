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
        GPS gps;
        public Form1()
        {
            InitializeComponent();
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

            foreach(Cidade c in gps.ListaCidades) {
                if (c.Nome.Equals(cidadeOrigem))
                    idOrigem = c.Id;

                else if (c.Nome.Equals(cidadeDestino))
                    idDestino = c.Id;

                if (idOrigem != -1 && idDestino != -1)
                    break;
            }           
            gps.buscarCaminhos(idOrigem, idDestino);
        }

        private void imprimeMatriz(int[,] m, Label lblMetodo, int colunas, int linhas)
        {
            lblMetodo.Text = "";
            for (int i = 0; i <= m.GetUpperBound(0); i++)//percoro as linhas todas
            {
                for (int j = 0; j <= m.GetUpperBound(1); j++) // percoro as clunas todas
                {
                    lblMetodo.Text += (j == 0 ? "" : " ") + m[i, j]; //adiciona um espaço antes de todas as colunas excepto a primeira, mas ao usares o espaço como separador, vai ficar esquisito caso uns números tenham mais dígitos do que outros
                }
                lblMetodo.Text += "\n"; //muda de linha no fim de cada linha
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            gps = new GPS();
            imprimeMatriz(gps.montarMatrizAdjacencia(), lblMatriz, gps.ListaCidades.Count - 1, gps.ListaCidades.Count - 1);
        }

      /* public String[,] testarMatriz()
        {
            int qtdCidades = listaCidades.Count - 1;
            String[,] matriz = new String[qtdCidades, qtdCidades];

            for (int i = 0; i < qtdCidades; i++)
                for (int j = 0; j < qtdCidades; j++)
                    foreach (Caminho c in listaCaminhos)
                        if (c.IdCidadeOrigem == i && c.IdCidadeDestino == j)
                            matriz[i, j] = "1";

            return matriz;
        }*/         
        
        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            double xProporcional = 4096.00 / pbMapa.Width;
            double yProporcional = 2048.00 / pbMapa.Height;
            foreach (Cidade c in gps.ListaCidades) {
                e.Graphics.FillEllipse(Brushes.Black, c.CoordenadaX / Convert.ToSingle(xProporcional), c.CoordenadaY / Convert.ToSingle(yProporcional), 7f, 7f);
                e.Graphics.DrawString(c.Nome, new Font("Comic Sans", 8, FontStyle.Bold), Brushes.Black, c.CoordenadaX / Convert.ToSingle(xProporcional)-15, c.CoordenadaY / Convert.ToSingle(yProporcional) -15);
            }
        }

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            gps.Arvore.DesenharArvore(true, gps.Arvore.Raiz, (int)tpArvore.Width / 2, 0, Math.PI / 2, Math.PI / 2.5, 300, e.Graphics);
        }
    }
}
