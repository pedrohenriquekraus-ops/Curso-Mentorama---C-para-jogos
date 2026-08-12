using UnityEngine;


public enum GhostType
{
    Blinky,
    Pinky,
    Inky,
    Clyde
}
public class GhostView : MonoBehaviour
{

    public CharacterMotor characterMotor;
    public GhostAI GhostAI;
    public GhostType GhostType;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetInteger("GhostType", (int)GhostType);
        characterMotor.OnDirectionChanged += CharacterMotor_OnDirectionChanged;
        GhostAI.OnGhostStateChanged += GhostAI_OnGhostStateChanged;


    }

    private void GhostAI_OnGhostStateChanged(GhostState ghoststate)
    {
        animator.SetInteger("State", (int)ghoststate);
    }

    private void CharacterMotor_OnDirectionChanged(Direction direction)
    {
        animator.SetInteger("Direction", (int)direction - 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
