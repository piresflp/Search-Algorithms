using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class Cidade : IComparable<Cidade>
    {
        private int id, coordenadaX, coordenadaY;
        private string nome;

        public Cidade(int id, string nome, int coordenadaX, int coordenadaY)
        {
            this.id = id;
            this.nome = nome;
            this.coordenadaX = coordenadaX;
            this.coordenadaY = coordenadaY;
        }

        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public int CoordenadaX { get => coordenadaX; set => coordenadaX = value; }
        public int CoordenadaY { get => coordenadaY; set => coordenadaY = value; }

        public int CompareTo(Cidade other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Id " + this.id + "; Nome: " + this.nome + "; CoordenadaX: " + this.coordenadaX + "; CoordenadaY: "+this.coordenadaY;
        }
    }
}
