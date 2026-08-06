using System.Collections.Generic;
using TMPro;
using UnityEngine;




public class TelaView : MonoBehaviour
{

    public TextMeshProUGUI Text;
    public TextMeshProUGUI Pontos;
    public TextMeshProUGUI Bolas;
    public ControleTaco controleTaco;


    private void Awake()
    {


    }
    void Start()
    {
        Text.text = "inicio de jogo";
        controleTaco.Onjogadordavez += Atualizatela;

        print("TelaView inscrita no evento Onjogadordavez");
    }

    private void Atualizatela(Participante obj, Participante obj2, List<string> bolas)

    {

        print($"Atualizando tela: jogador da vez: {obj.GetNome()}, pontos: {obj.GetPontos()}, bolas: {string.Join(", ", bolas)}");
        print($"Atualizando tela: jogador da vez: {obj2.GetNome()}, pontos: {obj2.GetPontos()}, bolas: {string.Join(", ", bolas)}");
        Text.text = $"vez de : {obj.GetJogadorDaVez()}";
        Pontos.text = $"pontos {obj.GetNome()} :{obj.GetPontos()}  pontos {obj2.GetNome()} :{obj2.GetPontos()}";
        Bolas.text = $"bolas : {string.Join(", ", bolas)}";
    }
}
