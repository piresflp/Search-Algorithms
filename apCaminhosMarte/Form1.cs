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
        List<Cidade> listaCidades;
        List<Caminho> listaCaminhos;
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

            foreach(Cidade c in listaCidades) {
                if (c.Nome.Equals(cidadeOrigem))
                    idOrigem = c.Id;

                else if (c.Nome.Equals(cidadeDestino))
                    idDestino = c.Id;

                if (idOrigem != -1 && idDestino != -1)
                    break;
            }            
            buscarCaminhos(idOrigem, idDestino);
        }

        private Caminho[,] montarMatrizAdjacencia()
        {            
            int qtdCidades = listaCidades.Count - 1;
            Caminho[,] matriz = new Caminho[qtdCidades, qtdCidades];

            for(int i = 0; i < qtdCidades; i++) 
                for(int j = 0; j < qtdCidades; j++)                
                    foreach(Caminho c in listaCaminhos)                    
                        if (c.IdCidadeOrigem == i && c.IdCidadeDestino == j)
                            matriz[i, j] = c;                    
        
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
            arvore = new Arvore<Cidade>();
            listaCidades = new List<Cidade>();
            listaCaminhos = new List<Caminho>();
            arvore = Leitor.lerCidades();
            listaCidades = arvore.getListaOrdenada();
            listaCaminhos = Leitor.lerCaminhos();
        }    
        
        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            double xProporcional = 4096.00 / pbMapa.Width;
            double yProporcional = 2048.00 / pbMapa.Height;
            foreach (Cidade c in listaCidades) {
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
