using UnityEngine;

public class DetectorTacada : MonoBehaviour
{
    public bool AcertouAlgumaBola { get; private set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bola"))
        {
            AcertouAlgumaBola = true;
        }
    }

    public void Reset()
    {
        AcertouAlgumaBola = false;
    }
}





