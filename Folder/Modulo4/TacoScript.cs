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

    private Dictionary<GameObject, Vector3> posicoesIniciais = new Dictionary<GameObject, Vector3>();



    private void Start()
    {
       
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Bola"))
        {
            posicoesIniciais[obj] = obj.transform.position;
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
        PosicionarTaco();
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