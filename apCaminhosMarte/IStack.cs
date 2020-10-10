
using System;

interface IStack<Dado>
{
    int Tamanho { get; }
    bool EstaVazia { get; }

    void Empilhar(Dado elemento);

    Dado Desempilhar();

    Dado OTopo();

}