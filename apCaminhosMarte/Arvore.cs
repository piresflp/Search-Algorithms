using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class Arvore<Dado> where Dado : IComparable<Dado>, ICloneable
    {
        protected NoArvore<Dado> raiz, atual, antecessor;

        public NoArvore<Dado> Raiz { get => raiz; set => raiz = value; }
        public NoArvore<Dado> Atual { get => atual; set => atual = value; }
        public NoArvore<Dado> Antecessor { get => antecessor; set => antecessor = value; }

        public Arvore() { }

        public void Incluir(Dado dadoLido)
        {
            Incluir(ref raiz, dadoLido);
        }
        private void Incluir(ref NoArvore<Dado> atual, Dado dadoLido)
        {
            if (atual == null)
            {
                atual = new NoArvore<Dado>(dadoLido);
            }
            else
            if (dadoLido.CompareTo(atual.Info) == 0)
                throw new Exception("Já existe esse registro!");
            else
            if (dadoLido.CompareTo(atual.Info) > 0)
            {     
                NoArvore<Dado> apDireito = atual.Dir;
                Incluir(ref apDireito, dadoLido);
                atual.Dir = apDireito;
            }
            else
            {
                NoArvore<Dado> apEsquerdo = atual.Esq;
                Incluir(ref apEsquerdo, dadoLido);

                atual.Esq = apEsquerdo;
            }
        }

        public void DesenharArvore(bool primeiraVez, NoArvore<Cidade> raiz, int x, int y, double angulo, double incremento, double comprimento, Graphics g)
        {
            int xf, yf;
            if (raiz != null)
            {
                Pen caneta = new Pen(Color.Red);
                xf = (int)Math.Round(x + Math.Cos(angulo) * comprimento);
                yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);
                if (primeiraVez)
                    yf = 25;
                g.DrawLine(caneta, x, y, xf, yf);
                DesenharArvore(false, raiz.Esq, xf, yf, Math.PI / 2 + incremento,
                incremento * 0.60, comprimento * 0.8, g);
                DesenharArvore(false, raiz.Dir, xf, yf, Math.PI / 2 - incremento,
                incremento * 0.60, comprimento * 0.8, g);
                SolidBrush preenchimento = new SolidBrush(Color.Black);
                g.FillEllipse(preenchimento, xf - 25, yf - 15, 60, 30);
                g.DrawString(Convert.ToString(raiz.Info.ToString()), new Font("Comic Sans", 7),
                new SolidBrush(Color.LightGray), xf - 23, yf - 7);
            }
        }

        public List<Dado> getListaOrdenada()
        {
            return ListaOrdenada(this.Raiz);
        }

        /*private List<Dado> ListaOrdenada(NoArvore<Dado> raiz)
        {
            List<Dado> lista = new List<Dado>();
            if (raiz.Esq != null)
                lista.AddRange(ListaOrdenada(raiz.Esq));
            lista.Add(raiz.Info);
            if (raiz.Dir != null)
                lista.AddRange(ListaOrdenada(raiz.Esq));

            return lista;
        }*/

        private List<Dado> ListaOrdenada(NoArvore<Dado> raiz)
        {
            if (raiz is null)
            {
                return new List<Dado>();
            }

            var result = new List<Dado>();
            result.AddRange(ListaOrdenada(raiz.Esq));
            result.Add(raiz.Info);
            result.AddRange(ListaOrdenada(raiz.Dir));
            return result;
        }

        public Arvore(Arvore<Dado> modelo)
        {
            this.Raiz = modelo.Raiz;
            this.Atual = modelo.Atual;
            this.Antecessor = modelo.Antecessor;
        }

        public Object Clone()
        {
            Arvore<Dado> ret = null;
            try
            {
                ret = new Arvore<Dado>(this);
            }
            catch (Exception e) { }

            return ret;
        }
    }
}
