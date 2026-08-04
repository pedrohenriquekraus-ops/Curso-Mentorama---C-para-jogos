using System;
using UnityEngine;

[Serializable]
public abstract class Participante
{
    [SerializeField] protected string Nome;
    [SerializeField] protected int Pontos;

    public string GetNome() => Nome;
    public int GetPontos() => Pontos;

    public void AdicionarPontos(int pontos) => Pontos += pontos;
    public void ResetarPontos() => Pontos = 0;

    public abstract void ProximoTurno();


    public abstract string GetJogadorDaVez();
}