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
        int pesoTotal;

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
            return this.pesoTotal.CompareTo(outro.pesoTotal);
        }

        public Caminho Clone() // deep copy
        {
            Caminho clone = new Caminho(this.movimentos);

            return clone;
        }

        public int IdCidadeOrigem { get => this.Movimentos.Ultimo.Info.IdCidadeOrigem; }
        public int IdCidadeDestino { get => this.Movimentos.Primeiro.Info.IdCidadeDestino;}
        public int PesoTotal { get => this.pesoTotal; set => pesoTotal = value; }
        public PilhaLista<Movimento> Movimentos { get => movimentos; set => movimentos = value; }
        public int Tamanho { get => Movimentos.Tamanho;}
    }
}
