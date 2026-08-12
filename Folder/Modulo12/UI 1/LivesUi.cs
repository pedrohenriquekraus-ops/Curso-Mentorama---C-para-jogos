using UnityEngine;

public class LivesUi : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] Lives;



    private void Start()
    {
        print("iniciei");
        var player = GameObject.FindWithTag("Player");
        print("peguei o player");
        var life = player.GetComponent<LIfe>();
        print("peguei a vida");
        life.OnLifeRemoved += Life_OnLifeRemoved;
        print("conectei no evento");

        UpdateLivesSprite(life.Lives);

    }

    private void UpdateLivesSprite(int lives)
    {
        for (int i = 0; i < Lives.Length; i++)
        {
            print("arrumei");
            Lives[i].SetActive(i < lives);
        }



    }

    private void Life_OnLifeRemoved(int remainingLives)
    {
        UpdateLivesSprite(remainingLives);
    }



    // Update is called once per frame
    void Update()
    {

    }
}
