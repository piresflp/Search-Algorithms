// 19169 - Felipe Pires Araujo
// 19196 - Rafael Romanhole Borrozino

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class NoArvore<Dado> : IComparable<NoArvore<Dado>> where Dado : IComparable<Dado>
    {
        Dado info;
        NoArvore<Dado> esq;
        NoArvore<Dado> dir;
        int altura;
        bool estaMarcadoParaMorrer;

        public NoArvore(Dado Informacao)
        {
            this.Info = Informacao;
            this.Esq = null;
            this.Dir = null;
            this.Altura = 0;
        }

        public NoArvore(Dado dados, NoArvore<Dado> esquerdo, NoArvore<Dado> direito, int altura)
        {
            this.Info = dados;
            this.Esq = esquerdo;
            this.Dir = direito;
            this.Altura = altura;
        }

        public Dado Info { get => info; set => info = value; }
        public NoArvore<Dado> Esq { get => esq; set => esq = value; }
        public NoArvore<Dado> Dir { get => dir; set => dir = value; }
        public bool EstaMarcadoParaMorrer { get => estaMarcadoParaMorrer; set => estaMarcadoParaMorrer = value; }
        public int Altura { get => altura; set => altura = value; }

        public int CompareTo(NoArvore<Dado> o)
        {
            return info.CompareTo(o.info);
        }

        public bool Equals(NoArvore<Dado> o)
        {
            return this.info.Equals(o.info);
        }
    }
}
