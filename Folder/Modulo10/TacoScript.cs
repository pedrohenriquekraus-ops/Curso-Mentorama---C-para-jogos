using System;
using System.Collections.Generic;
using UnityEngine;

public class ControleTaco : MonoBehaviour
{
    [Serializable]

    public struct ConfiguracoesPartida
    {
        public float tempoLimitePartida;
        public float forcaTaco;
        public float distanciaTaco;
        public float alturaCamera;

        public ConfiguracoesPartida(float tempoLimitePartida, float forcaTaco, float distanciaTaco, float alturaCamera)
        {
            this.tempoLimitePartida = tempoLimitePartida;
            this.forcaTaco = forcaTaco;
            this.distanciaTaco = distanciaTaco;
            this.alturaCamera = alturaCamera;
        }
    }


    [Header("Configurações da Partida")]
    [SerializeField]
    private ConfiguracoesPartida config = new ConfiguracoesPartida(
    tempoLimitePartida: 300f,
    forcaTaco: 10f,
    distanciaTaco: 4f,
    alturaCamera: 3f
);

    public enum ControleCamera { Visualizador, BolaBranca }

    public ControleCamera cameraEstado = ControleCamera.BolaBranca;

    [SerializeField] private String equipeatual; // 1 para equipe 1, 2 para equipe 2


    [Header("Referências")]
    [SerializeField] private Transform bolaBrancaTransform;
    [SerializeField] private GameObject bolaBranca;
    [SerializeField] private Transform taco;
    [SerializeField] private Camera cameraTaco;
    [SerializeField] private Transform bola7;






    [Header("Config da camera")]
    [SerializeField] private float sensibilidadeX = 500f;
    [SerializeField] private float sensibilidadeY = 500f;
    [SerializeField] private float anguloX = 0f;
    [SerializeField] private float anguloY = 0f;
    [SerializeField] private float AnguloMiny = 0f;
    [SerializeField] private float AnguloMaxy = 10f;

    [Header("Camera de cima da mesa")]
    [SerializeField] private Camera cameraMesa;
    [Header("Prefab de fumaça")]
    [SerializeField] private GameObject prefabFumaca;

    [Header("Time")]
    [SerializeField] float time;
    [SerializeField] float timeVerifica;



    public event Action<Participante, Participante, List<string>> Onjogadordavez;

    [SerializeReference] private Participante equipe1 = new Dupla("Equipe 1", new List<string> { "Jogador 1", "Jogador 2" });
    [SerializeReference] private Participante equipe2 = new Solo("Jogador 3");




    public List<string> BolasDerrubadas = new List<string>();
    public string[] Menores = { "bola1", "bola2", "bola3", "bola4", "bola5", "bola6", "bola7" };
    public string[] Maiores = { "bola9", "bola10", "bola11", "bola12", "bola13", "bola14", "bola15" };
    public string[] neutras = { "bola8", "bolaBranca" };

    bool encacapoualgo = false;

    [SerializeField] private DetectorTacada detectorTacada;






    private Dictionary<GameObject, Vector3> posicoesIniciais = new Dictionary<GameObject, Vector3>();




    /*
    private List<GameObject> bolas = new List<GameObject>();
    private List<Vector3> posicoesIniciais = new List<Vector3>();

    private void Start()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bola"))
        {
            bolas.Add(obj);
            posicoesIniciais.Add(obj.transform.position);
        }
    }

    void ResetarBolas()
    {
        for (int i = 0; i < bolas.Count; i++)
        {
            GameObject bola = bolas[i];
            Vector3 posicaoInicial = posicoesIniciais[i];

            bola.transform.position = posicaoInicial;

            Rigidbody rb = bola.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    */

    private void Start()
    {

        foreach (GameObject redeObj in GameObject.FindGameObjectsWithTag("Rde"))
        {
            Rede rede = redeObj.GetComponent<Rede>();
            print("Rede encontrada: " + redeObj.name);
            rede.OnBolaEncacapada += Encacapada;
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bola"))
        {
            posicoesIniciais[obj] = obj.transform.position;
        }


        Sorteia_jogador();

        Participante jogadorAtual = (equipeatual == "Equipe1") ? equipe1 : equipe2;
        Participante adversaria = (equipeatual == "Equipe1") ? equipe2 : equipe1;
        Onjogadordavez.Invoke(jogadorAtual, adversaria, BolasDerrubadas);


    }





    void Sorteia_jogador()
    {
        int sorteio = UnityEngine.Random.Range(0, 2); // Gera um número aleatório entre 0 e 1

        if (sorteio == 0)
        {
            equipeatual = "Equipe1";
        }
        else
        {
            equipeatual = "Equipe2";
        }
    }





    void Encacapada(string Bola, bool encacapou)
    {

        //verificação se algo entrou
        encacapoualgo = encacapou;
        timeVerifica = Time.time;



        int grupoDaBola = Array.Exists(Maiores, e => e == Bola) ? 1
                         : Array.Exists(Menores, e => e == Bola) ? 2
                         : 0; // bola branca ou bola8

        if (grupoDaBola == 0)
        {
            if (bolaBranca.name == Bola)

            {
                ResetarBolas(bolaBranca);
                Debug.Log("Bola Branca Encacapada");
                TrocaJogador();
                return;
            }
            else
            {
                Debug.Log("Bola 8 Encacapada");
            }
        }

        //remove as bolas encacapadas do Array de Maiores ou menos e adiciona na lista de bolas derrubadas
        for (int i = 0; i < Maiores.Length; i++)
        {
            if (Maiores[i] == Bola)
            {
                Array.Clear(Maiores, i, 1);
                BolasDerrubadas.Add(Bola);
                break;
            }
            else if (Menores[i] == Bola)
            {
                Array.Clear(Menores, i, 1);
                BolasDerrubadas.Add(Bola);
                break;
            }
        }

        Participante atual = (equipeatual == "Equipe1") ? equipe1 : equipe2;
        Participante adversaria = (equipeatual == "Equipe1") ? equipe2 : equipe1;

        if (atual.MorN == 0)
        {
            defini_maior_menor(grupoDaBola);
            atual.AdicionarPontos(1);
        }
        else if (atual.MorN == grupoDaBola)
        {
            atual.AdicionarPontos(1);
        }
        else
        {
            adversaria.AdicionarPontos(1);
            adversaria.ProximoTurno();
        }

        Onjogadordavez?.Invoke(atual, adversaria, BolasDerrubadas);

    }

    void defini_maior_menor(int grupo)
    {
        Participante atual = (equipeatual == "Equipe1") ? equipe1 : equipe2;
        Participante adversaria = (equipeatual == "Equipe1") ? equipe2 : equipe1;

        atual.MorN = grupo;
        adversaria.MorN = (grupo == 1) ? 2 : 1;
    }


    public void TrocaJogador()
    {
        equipeatual = (equipeatual == "Equipe1") ? "Equipe2" : "Equipe1";
        Participante atual = (equipeatual == "Equipe1") ? equipe1 : equipe2;
        Participante adversaria = (equipeatual == "Equipe1") ? equipe2 : equipe1;
        Onjogadordavez?.Invoke(atual, adversaria, BolasDerrubadas);
    }

    void ResetarBolas(GameObject BolaEspecifica = null)
    {


        if (BolaEspecifica != null)
        {
            if (BolaEspecifica == bolaBranca)
            {
                Vector3 bola = posicoesIniciais[BolaEspecifica];

                bolaBranca.transform.position = bola;
            }
        }
        else
        {
            foreach (var bolas in posicoesIniciais)
            {
                GameObject bola = bolas.Key;
                Vector3 posicaoInicial = bolas.Value;
                bola.transform.position = posicaoInicial;
                bola.SetActive(true);
                // Reseta a velocidade da bola para zero
                Rigidbody rb = bola.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }

    }






    void Update()
    {
        PosicionarCamera();
        ProcessarInput();
        AtualizarEstadoCamera();
    }

    void ProcessarInput()
    {
        if (Input.GetKeyDown(KeyCode.C) && cameraEstado != ControleCamera.Visualizador)
        {
            AplicaForca();
            Debug.Log("Bateu na bola!");
            time = Time.time;
            cameraEstado = ControleCamera.Visualizador;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ResetarBolas();
            GameObject fumaca = Instantiate(prefabFumaca);
            fumaca.transform.position = bola7.position;
            Destroy(fumaca, 2f);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            cameraEstado = (ControleCamera)(((int)cameraEstado + 1) % 3);
        }
    }

    void AtualizarEstadoCamera()
    {
        if (cameraEstado == ControleCamera.Visualizador)
        {
            AtualizarCameraVisualizador();
        }
        else if (cameraEstado == ControleCamera.BolaBranca)
        {
            taco.gameObject.SetActive(true);
            cameraMesa.enabled = false;
            cameraTaco.enabled = true;
        }
    }

    void AtualizarCameraVisualizador()
    {
        taco.gameObject.SetActive(false);
        cameraMesa.enabled = true;
        cameraTaco.enabled = false;

        if (Time.time <= time + 5f) return;

        List<GameObject> bolas = new List<GameObject>(posicoesIniciais.Keys);

        for (int i = 0; i < bolas.Count; i++)
        {
            Rigidbody rb = bolas[i].GetComponent<Rigidbody>();

            if (rb != null && rb.linearVelocity.magnitude > 0.1f)
                return; // ainda há bolas em movimento, não muda o estado da câmera
        }

        // chegou aqui: TODAS as bolas estão paradas, decide uma única vez
        if (time > timeVerifica + 5f && !encacapoualgo)
        {
            TrocaJogador();
        }
        else if (time > timeVerifica + 2f && encacapoualgo)
        {
            encacapoualgo = false;
        }
        if (!detectorTacada.AcertouAlgumaBola)
        {
            falta();
        }

        cameraEstado = ControleCamera.BolaBranca;
    }





    //função para falta

    private void falta()
    {
        Participante adversaria = (equipeatual == "Equipe1") ? equipe2 : equipe1;

        //remove a primeira bola da lista do adversario
        if (adversaria.MorN == 1 && Maiores.Length > 0)
        {
            string bolaRemovida = Maiores[0];
            GameObject bolaObj = GameObject.Find(bolaRemovida);
            bolaObj.SetActive(false);
            Array.Clear(Maiores, 0, 1);
            BolasDerrubadas.Add(bolaRemovida);

        }
        else if (adversaria.MorN == 2 && Menores.Length > 0)
        {
            string bolaRemovida = Menores[0];
            Array.Clear(Menores, 0, 1);
            BolasDerrubadas.Add(bolaRemovida);
        }
        // Troca o jogador
        TrocaJogador();

    }

    void AplicaForca()
    {
        // Aplica uma força na bola branca na direção do taco
        Rigidbody rb = bolaBrancaTransform.GetComponent<Rigidbody>();
        Vector3 direcao = taco.up;
        rb.AddForce(direcao * config.forcaTaco, ForceMode.Impulse);

        detectorTacada.Reset();
    }








    void PosicionarTaco()
    {
        // Posiciona o taco atrás da bola branca, na direção que o taco está "olhando"
        Vector3 direcao = taco.up;
        taco.position = bolaBrancaTransform.position - direcao * config.distanciaTaco;
    }



    void PosicionarCamera()
    {
        if (Input.GetMouseButton(1) && cameraEstado != ControleCamera.Visualizador)
        {
            anguloX += Input.GetAxis("Mouse X") * sensibilidadeX * Time.deltaTime;
            anguloY -= Input.GetAxis("Mouse Y") * sensibilidadeY * Time.deltaTime;
            anguloY = Mathf.Clamp(anguloY, AnguloMiny, AnguloMaxy);
        }

        Quaternion rotacao = Quaternion.Euler(anguloY, anguloX, 0f);

        // o taco gira com o mouse e se posiciona atrás da bola branca nessa direção
        taco.rotation = rotacao * Quaternion.Euler(90f, 0f, 0f); // ajusta a rotação
        taco.position = bolaBrancaTransform.position - (rotacao * Vector3.forward * config.distanciaTaco);

        // a câmera acompanha o taco, olhando por cima dele em direção à bola
        Vector3 posicaoAcima = taco.position + Vector3.up * config.alturaCamera;
        cameraTaco.transform.position = posicaoAcima;
        cameraTaco.transform.LookAt(bolaBrancaTransform.position);
    }



}