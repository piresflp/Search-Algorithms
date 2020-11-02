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

        public Caminho(int idCidadeOrigem, int idCidadeDestino)
        {
            this.IdCidadeOrigem = idCidadeOrigem;
            this.idCidadeDestino = idCidadeDestino;
            this.Movimentos = new PilhaLista<Movimento>();
        }

        public void adicionarMovimento(Movimento mov)
        {
            if (mov == null)
                throw new Exception("Movimento nulo!");

            this.Movimentos.Empilhar(mov);            
        }

        public void removerMovimento()
        {
            if (this.Movimentos.EstaVazia)
                throw new Exception("Pilha vazia!");

            this.Movimentos.Desempilhar();
        }

        public int CompareTo(Caminho outro)
        {
            return this.distanciaTotal.CompareTo(outro.distanciaTotal);
        }

        public int IdCidadeOrigem { get => idCidadeOrigem; set => idCidadeOrigem = value; }
        public int IdCidadeDestino { get => idCidadeDestino; set => idCidadeDestino = value; }
        public int DistanciaTotal { get => distanciaTotal; set => distanciaTotal = value; }
        public int TempoTotal { get => tempoTotal; set => tempoTotal = value; }
        public int CustoTotal { get => custoTotal; set => custoTotal = value; }
        internal PilhaLista<Movimento> Movimentos { get => movimentos; set => movimentos = value; }
    }
}
