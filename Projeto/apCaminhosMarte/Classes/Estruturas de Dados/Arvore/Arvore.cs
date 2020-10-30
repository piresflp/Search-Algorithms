// 19169 - Felipe Pires Araujo
// 19196 - Rafael Romanhole Borrozino

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
        
        public void Incluir(Dado novoRegistro)
        {
            if (Existe(novoRegistro))
                throw new Exception("Registro com chave repetida!");
            else
            {
                // o novoRegistro tem uma chave inexistente, então criamos um
                // novo nó para armazená-lo e depois ligamos esse nó na árvore
                var novoNo = new NoArvore<Dado>(novoRegistro);
                // se a árvore está vazia, a raiz passará a apontar esse novo nó
                if (raiz == null)
                    raiz = novoNo;
                else
                // nesse caso, antecessor aponta o pai do novo registro e
                // verificamos em qual ramo o novo nó será ligado
                if (novoRegistro.CompareTo(antecessor.Info) < 0) // novo é menor que antecessor
                    antecessor.Esq = novoNo; // vamos para a esquerda
                else
                    antecessor.Dir = novoNo; // ou vamos para a direita
            }
        }

        public bool Existe(Dado procurado)
        {
            antecessor = null;
            atual = raiz;
            while (atual != null)
            {
                if (atual.Info.CompareTo(procurado) == 0)
                    return true;
                else
                {
                    antecessor = atual;
                    if (procurado.CompareTo(atual.Info) < 0)
                        atual = atual.Esq; // Desloca apontador para o ramo à esquerda
                    else
                        atual = atual.Dir; // Desloca apontador para o ramo à direita
                }
            }
            return false; // Se local == null, a chave não existe
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
