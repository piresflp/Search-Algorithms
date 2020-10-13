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
            gps.CaminhosEncontrados = new List<PilhaLista<Caminho>>();
            if (lsbOrigem.SelectedIndex == -1)
                MessageBox.Show("Selecione a cidade de origem!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (lsbDestino.SelectedIndex == -1)
                MessageBox.Show("Selecione a cidade de destino!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            else
            {
                String[] cidades = new String[7] { "Acheron", "Arena", "Arrakeen", "Bakhuysen", "Bradbury", "Burroughs", "Cairo" };
                String cidadeOrigem = cidades[lsbOrigem.SelectedIndex];
                String cidadeDestino = cidades[lsbDestino.SelectedIndex];
                int idOrigem = -1, idDestino = -1;

                foreach (Cidade c in gps.ListaCidades)
                {
                    if (c.Nome.Equals(cidadeOrigem))
                        idOrigem = c.Id;

                    else if (c.Nome== cidadeDestino)
                        idDestino = c.Id;

                    if (idOrigem != -1 && idDestino != -1)
                        break;
                }
                gps.buscarCaminhos(idOrigem, idDestino);
                PilhaLista<Caminho> melhorCaminho = new PilhaLista<Caminho>();
                List<PilhaLista<Caminho>> caminhosPossiveis = new List<PilhaLista<Caminho>>(gps.CaminhosEncontrados);
                List<PilhaLista<Caminho>> caminhosPossiveisClone = new List<PilhaLista<Caminho>>(caminhosPossiveis.Count);

                caminhosPossiveis.ForEach((item) =>
                {
                    caminhosPossiveisClone.Add((PilhaLista<Caminho>)item.Clone());
                });
                melhorCaminho = buscarMelhorCaminho(caminhosPossiveis);
                ExibirCaminhos(caminhosPossiveisClone, melhorCaminho);
            }

        }

        private void ExibirCaminhos(List<PilhaLista<Caminho>> caminhos, PilhaLista<Caminho> melhorCaminho)
        {
            dataGridView1.ColumnCount = 0;
            dataGridView1.RowCount = caminhos.Count;
            int caminhosExibidos = 0;

            int t;
            foreach (PilhaLista<Caminho> caminho in caminhos)
            {
                t = caminho.Tamanho;
                if(t > dataGridView1.ColumnCount)
                    dataGridView1.ColumnCount = t;

               
                for(int i = t-1; !caminho.EstaVazia; i--)
                {
                    Caminho mov = (Caminho )caminho.Desempilhar();
                    dataGridView1.Rows[caminhosExibidos].Cells[i].Value = mov.ToString();
                }
                caminhosExibidos++;
            }

            t = melhorCaminho.Tamanho;
            dataGridView2.ColumnCount = t;
            dataGridView2.RowCount = 1;
            for(int i = t-1 ; !melhorCaminho.EstaVazia; i--)
            {
                Caminho mov = melhorCaminho.Desempilhar();
                dataGridView2.Rows[0].Cells[i].Value = mov.ToString();
            }
        }

      private PilhaLista<Caminho> buscarMelhorCaminho(List<PilhaLista<Caminho>> caminhos)
        {
            PilhaLista<Caminho> ret = new PilhaLista<Caminho>();
            int menorDistancia = 0;
            int[,] matriz = gps.Grafo;

            foreach(PilhaLista<Caminho> caminho in caminhos)
            {
                PilhaLista<Caminho> clone = caminho.Clone();
                int distanciaTotal = 0;
                while (!clone.EstaVazia)
                {
                    int idOrigem = clone.OTopo().IdCidadeOrigem;
                    int idDestino = clone.OTopo().IdCidadeDestino;
                    distanciaTotal += matriz[idOrigem, idDestino];
                    clone.Desempilhar();
                }
                if (distanciaTotal < menorDistancia || menorDistancia == 0)
                {
                    ret = caminho.Clone();
                    menorDistancia = distanciaTotal;
                }
            }
            return ret;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gps = new GPS();
        }
       
        
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Graphics g = pbMapa.CreateGraphics();
            g.Clear(Color.White);
            pbMapa.Image = Image.FromFile("mars_political_map_by_axiaterraartunion_d4vfxdf-pre.jpg");
            Application.DoEvents();
            desenharCaminho(this.gps.CaminhosEncontrados[dataGridView1.CurrentCell.RowIndex]);
        }

        private void desenharCaminho(PilhaLista<Caminho> caminho)
        {
            if (caminho != null)
            {
                Graphics g = pbMapa.CreateGraphics();
                Pen pen = new Pen(Color.Blue, 3);

                int xProporcional = 4096 / pbMapa.Width;
                int yProporcional = 2048 / pbMapa.Height;
                PilhaLista<Caminho> clone = caminho.Clone();
                int cordXOrigem = 0;
                int cordYOrigem = 0;
                int cordXDestino = 0;
                int cordYDestino = 0;
                while(!clone.EstaVazia)
                {
                    foreach(Cidade c in gps.ListaCidades)
                    {
                        if (clone.Primeiro.Info.IdCidadeOrigem == c.Id)
                        {
                            cordXOrigem = c.CoordenadaX;
                            cordYOrigem = c.CoordenadaY;
                        }
                        if(clone.Primeiro.Info.IdCidadeDestino == c.Id)
                        {
                            cordXDestino = c.CoordenadaX;
                            cordYDestino = c.CoordenadaY;
                        }
                    }
                    g.DrawLine(pen, new Point(cordXOrigem / xProporcional, cordYOrigem / yProporcional), new Point(cordXDestino / xProporcional, cordYDestino / yProporcional));
                    clone.Desempilhar();
                }
            }
        }
    }
}
