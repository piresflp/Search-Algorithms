using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class Movimento : IComparable<Movimento>, ICloneable
    {
        int idOrigem, idDestino;


        public Movimento (int idOrigem, int idDestino)
        {
            this.IdDestino = idDestino;
            this.IdOrigem = idOrigem;
        }

        public int IdOrigem { get => idOrigem; set => idOrigem = value; }
        public int IdDestino { get => idDestino; set => idDestino = value; }

        public int CompareTo(Movimento other)
        {
            throw new NotImplementedException();
        }

        public override String ToString()
        {
            return "id origem: "+ idOrigem + "  id destino: "+ idDestino;
        }

        public Movimento(Movimento modelo)
        {
            this.IdOrigem = modelo.IdOrigem;
            this.IdDestino = modelo.IdDestino;
        }

        public Object Clone()
        {
            Movimento ret = null;
            try
            {
                ret = new Movimento(this);
            }
            catch (Exception e) { }

            return ret;
        }
    }
}
