using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class Arvore<Dado> where Dado : IComparable<Dado>
    {
        private NoArvore<Dado> raiz, atual, antecessor;

        public NoArvore<Dado> Raiz { get => raiz; set => raiz = value; }
        public NoArvore<Dado> Atual { get => atual; set => atual = value; }
        public NoArvore<Dado> Antecessor { get => antecessor; set => antecessor = value; }
    }
}
