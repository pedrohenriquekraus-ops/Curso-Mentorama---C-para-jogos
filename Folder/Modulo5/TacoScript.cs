using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ControleTaco : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform bolaBranca;
    [SerializeField] private Transform taco;
    [SerializeField] private Camera cameraTaco;
    [SerializeField] private Transform bola7;


    [Header("Configurações")]
    [SerializeField] private float distanciaTaco = 10f;
    [SerializeField] private float alturaCamera = 3f;
    [Header("Anglo da camera")]
    [SerializeField] private float x = 40f;
    [SerializeField] private float y = 0f;
    [SerializeField] private float z = 0f;
    [Header("Camera de cima da mesa")]
    [SerializeField] private Camera cameraMesa;
    [Header("Prefab de fumaça")]
    [SerializeField] private GameObject prefabFumaca;

    [Header("Força do taco")]
    [SerializeField] private float forca = 20f;

    [Header("Jogadores")]
    [SerializeField] public string jogador1 = "Jogador 1";
    [SerializeField] public string jogador2 = "Jogador 2";

    public List<string> BolasDerrubadas = new List<string>();
    public string[] Menores = { "bola1", "bola2", "bola3", "bola4", "bola5", "bola6","bola7" };
    public string[] Maiores = { "bola9", "bola10", "bola11", "bola12", "bola13","bola14","bola15" };
    public string[] neutras = { "bola8","bolaBranca" };

    string primeiroajogar = "";

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
       
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bola"))
        {
            posicoesIniciais[obj] = obj.transform.position;
        }

        Sorteia_jogador();
    }


    void Sorteia_jogador()
    {
        int sorteio = Random.Range(0, 1);

        if (sorteio == 0)
        {
            primeiroajogar = jogador1;
        }
        else
        {
            primeiroajogar = jogador2;
        }
    }



            void Encacapada(string Bola)
    {
        for (int i = 0; i < Menores.Length; i++)
        {
            if (Bola == Menores[i])
            {
                Menores[i] = null;
                
            }
            if(Bola == Maiores[i])
            {
                Maiores[i] = null;
            }

        }
    }


      public void TrocaJogador()
        {
            if (primeiroajogar == jogador1)
            {
                primeiroajogar = jogador2;
            }
            else
            {
                primeiroajogar = jogador1;
            }
        }

    void ResetarBolas (){
    
                foreach (var bolas in posicoesIniciais)
                {
                    GameObject bola = bolas.Key;
                    Vector3 posicaoInicial = bolas.Value;
                    bola.transform.position = posicaoInicial;
                    // Reseta a velocidade da bola para zero
                    Rigidbody rb = bola.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }
                }
    
    
    }




 

    void Update()
    {
      //  PosicionarTaco();
        PosicionarCamera();

        if (Input.GetKeyDown(KeyCode.C))
        {

            AplicaForca();
            Debug.Log("Bateu na bola!");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ResetarBolas();
            //cria efeito de fumaça
            GameObject fumaca = Instantiate(prefabFumaca);
            fumaca.transform.position = bola7.position;
            Destroy(fumaca, 2f);



        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            cameraTaco.enabled = !cameraTaco.enabled;
            cameraMesa.enabled = !cameraMesa.enabled;
            Encacapada("bola7");
        }
    }

        void AplicaForca()
        {
            // Aplica uma força na bola branca na direção do taco
            Rigidbody rb = bolaBranca.GetComponent<Rigidbody>();
            Vector3 direcao = taco.up;
            rb.AddForce(direcao * forca, ForceMode.Impulse);
        }

        void PosicionarTaco()
        {
            // Posiciona o taco atrás da bola branca, na direção que o taco está "olhando"
            Vector3 direcao = taco.up;
            taco.position = bolaBranca.position - direcao * distanciaTaco;
        }

        void PosicionarCamera()
        {
            // Câmera fica acima do taco, olhando pra baixo em direção a ele
            Vector3 posicaoAcima = taco.position + Vector3.up * alturaCamera;
            cameraTaco.transform.rotation = Quaternion.Euler(x, y, z); // Olhando para baixo
            cameraTaco.transform.position = posicaoAcima;


        }

        



}