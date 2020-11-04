using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte.Classes.GPS
{
    class Caminho : IComparable<Caminho>, ICloneable
    {
        PilhaLista<Movimento> movimentos;
        int pesoTotal;

        public Caminho() 
        {
            this.movimentos = new PilhaLista<Movimento>();
        }

        public Caminho(PilhaLista<Movimento> movimentos)
        {
            this.movimentos = movimentos.Clone();
        }

        public void AdicionarMovimento(Movimento mov)
        {
            if (mov == null)
                throw new Exception("Movimento nulo!");

            this.movimentos.Empilhar(mov);            
        }

        public Movimento RemoverMovimento()
        {
            if (this.movimentos.EstaVazia)
                throw new Exception("Pilha vazia!");

            return this.movimentos.Desempilhar();
        }

        public int CompareTo(Caminho outro)
        {
            return this.pesoTotal.CompareTo(outro.pesoTotal);
        }

        public Object Clone() // deep copy
        {
            Caminho clone = new Caminho(this.movimentos);

            return clone;
        }

        public int IdCidadeOrigem { get => Movimentos.Ultimo.Info.IdCidadeOrigem; }
        public int IdCidadeDestino { get => Movimentos.Primeiro.Info.IdCidadeDestino;}
        public int PesoTotal { get => pesoTotal; set => pesoTotal = value; }
        public PilhaLista<Movimento> Movimentos { get => movimentos; set => movimentos = value; }
        public int Tamanho { get => Movimentos.Tamanho;}
    }
}
