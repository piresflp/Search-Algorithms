﻿// 19169 - Felipe Pires Araujo
// 19196 - Rafael Romanhole Borrozino

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
    public partial class frmBuscaCaminhos : Form
    {
        GPS gps;
        public frmBuscaCaminhos()
        {
            InitializeComponent();
        }

        /**
         * Tratamento do clique do botão de buscar caminhos.
         * Primeiramente, são realizadas verificações para garantir que as Listboxs estão devidamente selecionadas.
         * Se sim, é criado um vetor com o nome de cada cidade presente nos arquivos textos (de forma ordenada).
         * Em seguida, são definidas quais são as cidades de origem e destino selecionadas pelo usuário, de acordo com o índice das Listboxs.
         * Feito isso, o programa percorre a lista de cidades para encontrar o Id de tais cidades e chama o método de buscar caminhoe e melhor caminho.
         * Por fim, os resultados são armazenados em listas que serão passadas como parâmetro dos métodos de exibição.
         */
        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lsbOrigem.SelectedIndex == -1)
                    throw new Exception("Selecione a cidade de origem!");
                if (lsbDestino.SelectedIndex == -1)
                    throw new Exception("Selecione a cidade de destino!");

                gps.CaminhosEncontrados = new List<PilhaLista<Movimento>>();
                lerOpcoesEscolhidas();
                String[] cidades = new String[23] { "Acheron", "Arena", "Arrakeen", "Bakhuysen",
                                                "Bradbury", "Burroughs", "Cairo", "Dumont", "Echus Overlook",
                                                "Esperança", "Gondor", "Lakefront", "Lowell", "Moria", "Nicosia",
                                                "Odessa", "Perseverança", "Rowan", "Senzeni Na", "Sheffield", "Temperança",
                                                "Tharsis", "Underhill"};

                String cidadeOrigem = cidades[lsbOrigem.SelectedIndex];
                String cidadeDestino = cidades[lsbDestino.SelectedIndex];
                int idOrigem = -1, idDestino = -1;

                foreach (Cidade c in gps.ListaCidades) // percorre o vetor de cidades para encontrar o Id das cidades selecionadas
                {
                    if (c.Nome.Equals(cidadeOrigem))
                        idOrigem = c.Id;

                    else if (c.Nome == cidadeDestino)
                        idDestino = c.Id;

                    if (idOrigem != -1 && idDestino != -1) // se o Id de ambas cidades já foram encontrados
                        break; // sai do foreach
                }

                MetodoDeBusca metodoEscolhido = gps.Metodo;
                switch (metodoEscolhido)
                {
                    case MetodoDeBusca.Pilhas:
                        break;

                    case MetodoDeBusca.Recursao:
                        gps.buscarCaminhos(idOrigem, idDestino);
                        break;

                    case MetodoDeBusca.Dijkstra:
                        break;
                }

                PilhaLista<Movimento> melhorCaminho = new PilhaLista<Movimento>();
                List<PilhaLista<Movimento>> caminhosPossiveis = new List<PilhaLista<Movimento>>(gps.CaminhosEncontrados);
                List<PilhaLista<Movimento>> caminhosPossiveisClone = new List<PilhaLista<Movimento>>(caminhosPossiveis.Count);

                // clone da lista dos caminhos encontrados
                caminhosPossiveis.ForEach((item) =>
                {
                    caminhosPossiveisClone.Add((PilhaLista<Movimento>)item.Clone());
                });

                melhorCaminho = buscarMelhorCaminho(caminhosPossiveis);
                ExibirCaminhos(caminhosPossiveisClone, melhorCaminho);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }        
        }
        

        /**
         * Método com a função de exibir os resultados para o usuário.
         * Percorre a lista de caminhos encontrados e de melhor caminho, exibindo cada passo em uma coluna do dataGridView.
         * Caso nenhuma coluna seja exibida, nenhum caminho foi encontrado e o usuário é alertado.
         */
        private void ExibirCaminhos(List<PilhaLista<Movimento>> caminhos, PilhaLista<Movimento> melhorCaminho)
        {                      
            dgvCaminhos.ColumnCount = 0;
            dgvCaminhos.RowCount = caminhos.Count;
            int caminhosExibidos = 0;

            int t;
            foreach (PilhaLista<Movimento> caminho in caminhos)
            {
                t = caminho.Tamanho;
                if (t > dgvCaminhos.ColumnCount)
                {
                    dgvCaminhos.ColumnCount = t; // quantidade de colunas do dataGridView se iguala ao tamanho de passos do maior caminho
                    for (int i = 0; i < t; i++)
                        dgvCaminhos.Columns[i].HeaderText = "Cidade";
                }


                // exibe cada movimento do caminho em questão
                for (int i = t - 1; !caminho.EstaVazia; i--) 
                {
                    Movimento mov = (Movimento)caminho.Desempilhar();
                    dgvCaminhos.Rows[caminhosExibidos].Cells[i].Value = mov.ToString();
                }
                caminhosExibidos++;
            }
            
            if (melhorCaminho.Tamanho > 0) // se a lista de melhorCaminho não estiver vazia
            {
                t = melhorCaminho.Tamanho;
                dgvMelhorCaminho.ColumnCount = t;
                dgvMelhorCaminho.RowCount = 1;
                for (int i = t - 1; !melhorCaminho.EstaVazia; i--) // exibe cada movimento do melhor caminho
                {
                    Movimento mov = melhorCaminho.Desempilhar();
                    dgvMelhorCaminho.Rows[0].Cells[i].Value = mov.ToString();
                }
            }
            else // se nenhum caminho foi encontrado, o usuário é alertado e os dataGridViews limpados
            {
                limparPictureBox();
                dgvMelhorCaminho.RowCount = 0;
                MessageBox.Show("Não foi encontrado nenhum caminho.");
            }
        }

        /**
         * Chama o método que busca o melhor caminho, da classe GPS.
         */
        private PilhaLista<Movimento> buscarMelhorCaminho(List<PilhaLista<Movimento>> caminhos)
        {
            return gps.buscarMelhorCaminho(caminhos);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                gps = new GPS();
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            double xProporcional = 4096.00 / pbMapa.Width;
            double yProporcional = 2048.00 / pbMapa.Height;
            foreach (Cidade c in gps.ListaCidades)
            {
                e.Graphics.FillEllipse(Brushes.Black, c.CoordenadaX / Convert.ToSingle(xProporcional), c.CoordenadaY / Convert.ToSingle(yProporcional), 7f, 7f);
                e.Graphics.DrawString(c.Nome, new Font("Comic Sans", 8, FontStyle.Bold), Brushes.Black, c.CoordenadaX / Convert.ToSingle(xProporcional) - 15, c.CoordenadaY / Convert.ToSingle(yProporcional) - 15);
            }
        }

        private void tpArvore_Paint(object sender, PaintEventArgs e)
        {
            gps.Arvore.DesenharArvore(true, gps.Arvore.Raiz, (int)tpArvore.Width / 2, 0, Math.PI / 2, Math.PI / 2.5, 300, e.Graphics);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            limparPictureBox();
            Application.DoEvents();
            desenharCaminho(this.gps.CaminhosEncontrados[dgvCaminhos.CurrentCell.RowIndex]);
        }

        /**
         * Método responsável por desenhar as retas ligando cada movimento do caminho passado como parâmetro.
         */
        private void desenharCaminho(PilhaLista<Movimento> caminho)
        {
            if (caminho != null)
            {
                Graphics g = pbMapa.CreateGraphics();
                Pen pen = new Pen(Color.Blue, 3);

                int xProporcional = 4096 / pbMapa.Width;
                int yProporcional = 2048 / pbMapa.Height;
                PilhaLista<Movimento> clone = caminho.Clone();
                int cordXOrigem = 0;
                int cordYOrigem = 0;
                int cordXDestino = 0;
                int cordYDestino = 0;
                while (!clone.EstaVazia)
                {
                    foreach (Cidade c in gps.ListaCidades)
                    {
                        if (clone.Primeiro.Info.IdCidadeOrigem == c.Id)
                        {
                            cordXOrigem = c.CoordenadaX;
                            cordYOrigem = c.CoordenadaY;
                        }
                        if (clone.Primeiro.Info.IdCidadeDestino == c.Id)
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

        /**
         * Limpa a pictureBox do mapa de marte para exibição de novos caminhos, evitando sobreposição.
         */
        private void limparPictureBox()
        {
            Graphics g = pbMapa.CreateGraphics();
            g.Clear(Color.White);
            pbMapa.Image = Image.FromFile("mars_political_map_by_axiaterraartunion_d4vfxdf-pre.jpg");
        }

        private void lerOpcoesEscolhidas()
        {
            String criterioMelhorCaminho = null, metodoDeBusca = null;

            foreach(RadioButton rb in groupBox1.Controls.OfType<RadioButton>())            
                if (rb.Checked)
                    criterioMelhorCaminho = rb.Name;
            
            foreach (RadioButton rb in groupBox2.Controls.OfType<RadioButton>())            
                if (rb.Checked)
                    metodoDeBusca = rb.Name;

            if (criterioMelhorCaminho == null) // se nenhum botão foi selecionado pelo usuário
                throw new Exception("Selecione o critério desejado!");
                
            if (metodoDeBusca == null) // se nenhum botão foi selecionado pelo usuário
                throw new Exception("Selecione o método de busca desejado!");               
        
            switch (criterioMelhorCaminho)
            {
                case "rbTempo":
                    gps.Criterio = CriterioMelhorCaminho.Tempo;
                    break;

                case "rbDistancia":
                    gps.Criterio = CriterioMelhorCaminho.Distancia;
                    break;

                case "rbCusto":
                    gps.Criterio = CriterioMelhorCaminho.Custo;
                    break;
            }

            switch(metodoDeBusca)
            {
                case "rbPilhas":
                    gps.Metodo = MetodoDeBusca.Pilhas;
                    break;

                case "rbRecursao":
                    gps.Metodo = MetodoDeBusca.Recursao;
                    break;

                case "rbDijkstra":
                    gps.Metodo = MetodoDeBusca.Dijkstra;
                    break;
            }
        }
    }
}
