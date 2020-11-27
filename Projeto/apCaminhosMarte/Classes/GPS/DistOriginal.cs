using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte.Classes.GPS
{
    class DistOriginal
    {
        public int distancia;
        public int verticePai;
        public DistOriginal(int vp, int d)
        {
            distancia = d;
            verticePai = vp;
        }
    }
}
