// 19169 - Felipe Pires Araujo
// 19196 - Rafael Romanhole Borrozino

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class Cidade : IComparable<Cidade>, ICloneable
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
            return this.id - other.id;
        }

        public override string ToString()
        {
            //return "Id " + this.id + "; Nome: " + this.nome + "; CoordenadaX: " + this.coordenadaX + "; CoordenadaY: "+this.coordenadaY;
            return "" + this.id + " " + this.nome;
        }

        public Cidade(Cidade modelo) 
        {
            this.Id = modelo.Id;
            this.Nome = modelo.Nome;
            this.CoordenadaX = modelo.coordenadaX;
            this.coordenadaY = modelo.coordenadaY;
        }

        public Object Clone()
        {
            Cidade ret = null;
            try
            {
                ret = new Cidade(this);
            }
            catch (Exception e) { }

            return ret;
        }
    }
}
