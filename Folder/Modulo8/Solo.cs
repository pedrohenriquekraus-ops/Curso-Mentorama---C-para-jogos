using System;

[Serializable]
public class Solo : Participante
{
    public Solo(string nome)
    {
        Nome = nome;
    }

    public override void ProximoTurno()
    {
        // jogador solo não passa a vez pra ninguém
    }

    public override string GetJogadorDaVez()
    {
        return Nome;
    }
}