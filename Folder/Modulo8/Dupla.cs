using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dupla : Participante
{
    [SerializeField] private List<string> jogadores;
    [SerializeField] private int jogadorAtual;

    public Dupla(string nome, List<string> jogadores)
    {
        Nome = nome;
        this.jogadores = jogadores;
        jogadorAtual = 0;
    }

    public override void ProximoTurno()
    {
        jogadorAtual = (jogadorAtual + 1) % jogadores.Count;
    }

    public override string GetJogadorDaVez()
    {
        return jogadores[jogadorAtual];
    }
}