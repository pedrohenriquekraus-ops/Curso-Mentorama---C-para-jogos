using UnityEngine;

public class ControleTaco : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform bolaBranca;
    [SerializeField] private Transform taco;
    [SerializeField] private Camera cameraTaco;

    [Header("Configurações")]
    [SerializeField] private float distanciaTaco = 10f;
    [SerializeField] private float alturaCamera = 3f;
    [Header("Anglo da camera")]
    [SerializeField] private float x = 40f;
    [SerializeField] private float y = 0f;
    [SerializeField] private float z = 0f;

    [Header("Força do taco")]
    [SerializeField] private float forca = 10f;

    void FixedUpdate()
    {
        PosicionarTaco();
        PosicionarCamera();

        if(Input.GetKeyDown(KeyCode.C))
        {

            AplicaForca();
            Debug.Log("Bateu na bola!");
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