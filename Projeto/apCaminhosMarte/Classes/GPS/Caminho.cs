using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte.Classes.GPS
{
    class Caminho : IComparable<Caminho>
    {
        PilhaLista<Movimento> movimentos;
        int criterioTotal;

        public Caminho() { }

        public Caminho(PilhaLista<Movimento> movimentos)
        {
            this.movimentos = movimentos.Clone();
        }

        public void adicionarMovimento(Movimento mov)
        {
            if (mov == null)
                throw new Exception("Movimento nulo!");

            this.movimentos.Empilhar(mov);            
        }

        public Movimento removerMovimento()
        {
            if (this.movimentos.EstaVazia)
                throw new Exception("Pilha vazia!");

            return this.movimentos.Desempilhar();
        }

        public int CompareTo(Caminho outro)
        {
            return this.criterioTotal.CompareTo(outro.criterioTotal);
        }

        public Caminho Clone() // deep copy
        {
            Caminho deepCopy = new Caminho(this.movimentos);

            return deepCopy;
        }

        public int IdCidadeOrigem { get => this.Movimentos.Ultimo.Info.IdCidadeOrigem; }
        public int IdCidadeDestino { get => this.Movimentos.Primeiro.Info.IdCidadeDestino;}
        public int CriterioTotal { get => this.criterioTotal; set => criterioTotal = value; }
        public PilhaLista<Movimento> Movimentos { get => movimentos; set => movimentos = value; }
        public int Tamanho { get => Movimentos.Tamanho;}
    }
}
