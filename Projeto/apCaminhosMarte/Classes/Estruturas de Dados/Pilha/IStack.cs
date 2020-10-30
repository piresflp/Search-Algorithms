// 19169 - Felipe Pires Araujo
// 19196 - Rafael Romanhole Borrozino

using System;

interface IStack<Dado>
{
    int Tamanho { get; }
    bool EstaVazia { get; }

    void Empilhar(Dado elemento);

    Dado Desempilhar();

    Dado OTopo();

}