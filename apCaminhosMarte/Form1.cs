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
        public Form1()
        {
            InitializeComponent();
        }

        private void TxtCaminhos_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Buscar caminhos entre cidades selecionadas");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader lerArvore = new StreamReader("CidadesMarte.txt");
            arvore = new Arvore<Cidade>();
            listaCidade = new List<Cidade>();
            while(!lerArvore.EndOfStream)
            {
                String linha = lerArvore.ReadLine();
                int idCidade = int.Parse(linha.Substring(0, 3).Trim());
                String nome = linha.Substring(3, 16).Trim();
                int cordenadaX = int.Parse(linha.Substring(19, 5).Trim());
                int cordenadaY = int.Parse(linha.Substring(24, 4).Trim());
                Cidade novaCidade = new Cidade(idCidade, nome, cordenadaX, cordenadaY);
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
