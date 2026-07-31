using NUnit.Framework;
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
    forcaTaco: 20f,
    distanciaTaco: 10f,
    alturaCamera: 3f
);

    public enum ControleCamera { Visualizador, BolaBranca, Jogando }

    public ControleCamera cameraEstado = ControleCamera.Visualizador;

    [SerializeField] private int equipeatual = 1; // 1 para equipe 1, 2 para equipe 2


    [Header("Referências")]
    [SerializeField] private Transform bolaBranca;
    [SerializeField] private Transform taco;
    [SerializeField] private Camera cameraTaco;
    [SerializeField] private Transform bola7;




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

 




    public List<string> BolasDerrubadas = new List<string>();
    public string[] Menores = { "bola1", "bola2", "bola3", "bola4", "bola5", "bola6","bola7" };
    public string[] Maiores = { "bola9", "bola10", "bola11", "bola12", "bola13","bola14","bola15" };
    public string[] neutras = { "bola8","bolaBranca" };

   

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
        int sorteio = UnityEngine.Random.Range(0, 2); // Gera um número aleatório entre 0 e 1

        if (sorteio == 0)
        {
          equipeatual = 1;
        }
        else
        {
         equipeatual = 2;
        }
    }



            void Encacapada(string Bola)
    {
      
    }


      public void TrocaJogador()
        {
           equipeatual = (equipeatual == 1) ? 2 : 1;
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
             // int total = System.Enum.GetValues(typeof(ControleCamera)).Length;
            cameraEstado = (ControleCamera)(((int)cameraEstado + 1)%3);
            int debug = (int)cameraEstado +1 %3;
            int debug2 = (int)cameraEstado;
          
          


            if (cameraEstado == ControleCamera.Visualizador)
            {
                cameraMesa.enabled = false;
                cameraTaco.enabled = true;
            }else if(cameraEstado == ControleCamera.BolaBranca)
            {
                cameraMesa.enabled = true;
                cameraTaco.enabled = false;
            }
          //  else if (cameraEstado == ControleCamera.Jogando)
         //   {
          //      cameraMesa.enabled = false;
//cameraTaco.enabled = true;
          //  }

       
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
            taco.position = bolaBranca.position - direcao * config.distanciaTaco;
        }

        void PosicionarCamera()
        {
            // Câmera fica acima do taco, olhando pra baixo em direção a ele
            Vector3 posicaoAcima = taco.position + Vector3.up * config.alturaCamera;
            cameraTaco.transform.rotation = Quaternion.Euler(x, y, z); // Olhando para baixo
            cameraTaco.transform.position = posicaoAcima;


        }

        



}