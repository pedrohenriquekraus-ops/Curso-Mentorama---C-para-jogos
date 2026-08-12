using UnityEngine;

public class Teleporter : MonoBehaviour
{

    public Transform ExitPosition;

    public Direction Direction;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = ExitPosition.position;
    }
}
