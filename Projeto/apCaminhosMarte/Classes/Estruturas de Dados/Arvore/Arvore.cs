// 19169 - Felipe Pires Araujo
// 19196 - Rafael Romanhole Borrozino

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{
    class Arvore<Dado> where Dado : IComparable<Dado>, ICloneable
    {
        NoArvore<Dado> raiz, atual, antecessor;

        public Arvore() { }

        public NoArvore<Dado> InserirBalanceado(Dado item, NoArvore<Dado> noAtual)
        {
            if (noAtual == null)
            {
                noAtual = new NoArvore<Dado>(item);
            }
            else
            {
                if (item.CompareTo(noAtual.Info) < 0)
                {
                    noAtual.Esq = InserirBalanceado(item, noAtual.Esq);
                    if (getAltura(noAtual.Esq) - getAltura(noAtual.Dir) == 2)  // a propriedade Altura testa nulo!
                        if (item.CompareTo(noAtual.Esq.Info) < 0)
                            noAtual = RotacaoSimplesComFilhoEsquerdo(noAtual);
                        else
                            noAtual = RotacaoDuplaComFilhoEsquerdo(noAtual);
                }
                else
                  if (item.CompareTo(noAtual.Info) > 0)
                {
                    noAtual.Dir = InserirBalanceado(item, noAtual.Dir);
                    if (getAltura(noAtual.Dir) - getAltura(noAtual.Esq) == 2)  // a propriedade Altura testa nulo!
                        if (item.CompareTo(noAtual.Dir.Info) > 0)
                            noAtual = RotacaoSimplesComFilhoDireito(noAtual);
                        else
                            noAtual = RotacaoDuplaComFilhoDireito(noAtual);
                }
                //else ; - não faz nada, valor duplicado

                noAtual.Altura = Math.Max(getAltura(noAtual.Esq), getAltura(noAtual.Dir)) + 1;
            }
            return noAtual;
        }

        private NoArvore<Dado> RotacaoSimplesComFilhoEsquerdo(NoArvore<Dado> no)
        {
            NoArvore<Dado> temp = no.Esq;
            no.Esq = temp.Dir;
            temp.Dir = no;
            no.Altura = Math.Max(getAltura(no.Esq), getAltura(no.Dir)) + 1;
            temp.Altura = Math.Max(getAltura(temp.Esq), getAltura(no)) + 1;
            return temp;
        }

        private NoArvore<Dado> RotacaoSimplesComFilhoDireito(NoArvore<Dado> no)
        {
            NoArvore<Dado> temp = no.Dir;
            no.Dir = temp.Esq;
            temp.Esq = no;
            no.Altura = Math.Max(getAltura(no.Esq), getAltura(no.Dir)) + 1;
            temp.Altura = Math.Max(getAltura(temp.Dir), getAltura(no)) + 1;
            return temp;
        }

        private NoArvore<Dado> RotacaoDuplaComFilhoEsquerdo(NoArvore<Dado> no)
        {
            no.Esq = RotacaoSimplesComFilhoDireito(no.Esq);
            return RotacaoSimplesComFilhoEsquerdo(no);
        }

        private NoArvore<Dado> RotacaoDuplaComFilhoDireito(NoArvore<Dado> no)
        {
            no.Dir = RotacaoSimplesComFilhoEsquerdo(no.Dir);
            return RotacaoSimplesComFilhoDireito(no);
        }

        public int getAltura(NoArvore<Dado> no)
        {
            if (no != null)
                return no.Altura;
            else
                return -1;
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

        public Dado BuscarDado(Dado dadoBuscado)
        {
            return acharDado(dadoBuscado, this.raiz);
        }

        private Dado acharDado(Dado dadoBuscado, NoArvore<Dado> atual)
        {
            if (atual == null)
                throw new Exception("Dado inexistente!");

            int comp = dadoBuscado.CompareTo(atual.Info);
            if (comp == 0)
                return atual.Info;
            if (comp < 0)
                return acharDado(dadoBuscado, atual.Esq);
            else
                return acharDado(dadoBuscado, atual.Dir);
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

        public NoArvore<Dado> Raiz { get => raiz; set => raiz = value; }
        public NoArvore<Dado> Atual { get => atual; set => atual = value; }
        public NoArvore<Dado> Antecessor { get => antecessor; set => antecessor = value; }
    }
}
