// 19169 - Felipe Pires Araujo
// 19196 - Rafael Romanhole Borrozino

using System;
public class NoLista<Dado> where Dado : IComparable<Dado>
{
    private Dado info;
    private NoLista<Dado> prox;

    public Dado Info
    {
        get { return info; }

        set
        {
            if (value != null)
                info = value;
        }
    }

    public NoLista<Dado> Prox
    {
        get
        {
            return prox;
        }

        set
        {
            prox = value;
        }
    }

    public NoLista(Dado novaInfo, NoLista<Dado> proximo)
    {
        Info = novaInfo;
        prox = proximo;
    }

}