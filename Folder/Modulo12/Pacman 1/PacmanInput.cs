using UnityEngine;


[RequireComponent(typeof(CharacterMotor))]

public class PacmanInput : MonoBehaviour
{
    private CharacterMotor _motor;
    // Start is called before the first frame update
    void Start()
    {

        _motor = GetComponent<CharacterMotor>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _motor.SetMoveDirection(Direction.up);
            print("W");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _motor.SetMoveDirection(Direction.down);
            print("S");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _motor.SetMoveDirection(Direction.left);
            print("A");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _motor.SetMoveDirection(Direction.right);
            print("D");
        }

    }
}
