using UnityEngine;

public class PacmanView : MonoBehaviour
{

    public CharacterMotor characterMotor;

    public Animator animator;

    public LIfe lIfe;

    public AudioSource audioSource;

    public AudioClip clip;

    private void Start()
    {
        characterMotor.OnDirectionChanged += CharacterMotor_OnDirectionChanged;
        lIfe.OnLifeRemoved += LIfe_OnLifeRemoved;
        characterMotor.OnresetPosition += CharacterMotor_OnresetPosition;
        characterMotor.OnDisabled += CharacterMotor_OnDisabled;


    }

    private void CharacterMotor_OnDisabled()
    {

    }

    private void CharacterMotor_OnresetPosition()
    {
        animator.SetBool("Dead", false);
    }

    private void LIfe_OnLifeRemoved(int obj)
    {
        transform.Rotate(0, 0, -90);
        audioSource.PlayOneShot(clip);
        animator.SetBool("Moving", false);
        animator.SetBool("Dead", true);
        //espera a animação acabar e desativa


    }

    private void CharacterMotor_OnDirectionChanged(Direction direction)
    {

        switch (direction)
        {
            case Direction.up:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                animator.SetBool("Moving", true);

                break;
            case Direction.down:
                animator.SetBool("Moving", true);
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case Direction.left:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                animator.SetBool("Moving", true);
                break;
            case Direction.right:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                animator.SetBool("Moving", true);
                break;
            case Direction.None:
                animator.SetBool("Moving", false);
                break;

        }
    }
}
