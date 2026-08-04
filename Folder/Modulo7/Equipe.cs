using System.Collections.Generic;
using UnityEngine;

public class Equipe
{

    [SerializeField] public string NomeEquipe;
    [SerializeField] public int PontosEquipe;
    [SerializeField] public int JogadoresEquipe;
    [SerializeField] public int JogadorAtual;
    [SerializeField] public List<string> jogadores;

    public Equipe(string nomeEquipe, int pontosEquipe, int jogadoresEquipe, int jogadorAtual, List<string> jogadores)
    {
        NomeEquipe = nomeEquipe;
        PontosEquipe = pontosEquipe;
        JogadoresEquipe = jogadoresEquipe;
        JogadorAtual = jogadorAtual;
        this.jogadores = jogadores;
    }
    public string GetNomeEquipe()
    {
        return NomeEquipe;
    }
    public int GetPontosEquipe()
    {
        return PontosEquipe;
    }
    public int GetJogadoresEquipe()
    {
        return JogadoresEquipe;
    }
    public int GetJogadorAtual()
    {
        return JogadorAtual;
    }
    public List<string> GetJogadores()
    {
        return jogadores;
    }


    public void SetNomeEquipe(string nomeEquipe)
    {
        NomeEquipe = nomeEquipe;
    }

    public void SetPontosEquipe(int pontosEquipe)
    {
        PontosEquipe = pontosEquipe;
    }

    public void SetJogadoresEquipe(int jogadoresEquipe)
    {
        JogadoresEquipe = jogadoresEquipe;
    }

    public void SetJogadorAtual(int jogadorAtual)
    {
        JogadorAtual = jogadorAtual;
    }

    public void SetJogadores(List<string> jogadores)
    {
        this.jogadores = jogadores;
    }

    public void AdicionarJogador(string jogador)
    {
        jogadores.Add(jogador);
        JogadoresEquipe++;
    }

    public void RemoverJogador(string jogador)
    {
        jogadores.Remove(jogador);
        JogadoresEquipe--;
    }

    public void AdicionarPontos(int pontos)
    {
        PontosEquipe += pontos;
    }

    public void ResetarPontos()
    {
        PontosEquipe = 0;
    }

    public
        void ProximoJogador()
    {
        JogadorAtual = (JogadorAtual + 1) % JogadoresEquipe;
    }


    ~Equipe()
    {
        // Cleanup code if needed
    }
}
