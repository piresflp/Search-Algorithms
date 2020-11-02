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
        int idCidadeOrigem, idCidadeDestino;
        int distanciaTotal, tempoTotal, custoTotal;
        int tamanho;

        public Caminho() { }

        public Caminho(int idCidadeOrigem, int idCidadeDestino)
        {
            this.IdCidadeOrigem = idCidadeOrigem;
            this.idCidadeDestino = idCidadeDestino;
            this.Movimentos = new PilhaLista<Movimento>();
        }

        public Caminho(int idCidadeOrigem, int idCidadeDestino, PilhaLista<Movimento> movimentos)
        {
            this.IdCidadeOrigem = idCidadeOrigem;
            this.idCidadeDestino = idCidadeDestino;
            this.Movimentos = movimentos.Clone();
        }

        public void adicionarMovimento(Movimento mov)
        {
            if (mov == null)
                throw new Exception("Movimento nulo!");

            this.Movimentos.Empilhar(mov);            
        }

        public Movimento removerMovimento()
        {
            if (this.Movimentos.EstaVazia)
                throw new Exception("Pilha vazia!");

            return this.Movimentos.Desempilhar();
        }

        public int CompareTo(Caminho outro)
        {
            return this.distanciaTotal.CompareTo(outro.distanciaTotal);
        }

        public Caminho Clone() // deep copy
        {
            Caminho deepCopy = new Caminho(this.IdCidadeOrigem, this.idCidadeDestino, this.movimentos);

            return deepCopy;
        }

        public int IdCidadeOrigem { get => idCidadeOrigem; set => idCidadeOrigem = value; }
        public int IdCidadeDestino { get => idCidadeDestino; set => idCidadeDestino = value; }
        public int DistanciaTotal { get => distanciaTotal; set => distanciaTotal = value; }
        public int TempoTotal { get => tempoTotal; set => tempoTotal = value; }
        public int CustoTotal { get => custoTotal; set => custoTotal = value; }
        public PilhaLista<Movimento> Movimentos { get => movimentos; set => movimentos = value; }
        public int Tamanho { get => Movimentos.Tamanho; set => tamanho = value; }
    }
}
