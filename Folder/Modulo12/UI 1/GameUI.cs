using UnityEngine;

public class GameUI : MonoBehaviour
{


    public GameObject ReadyMessage;
    public GameObject GameOverMessage;
    public AudioSource AudioSource;
    public AudioClip BeginningMusic;
    public GameManager _gameManager;
    public BlinkTilemapColor BlinkTilemap;


    //public BlinkTilemapColor BlinkTilemap;
    void Start()
    {

        _gameManager = FindObjectOfType<GameManager>();
        _gameManager.OnGameStarted += GameManager_OnGameStarted;
        _gameManager.OnGameOver += GameManager_OnGameOver;
        _gameManager.OnVictory += _gameManager_OnVictory;
        AudioSource.PlayOneShot(BeginningMusic);


    }

    private void _gameManager_OnVictory()
    {
        BlinkTilemap.enabled = true;
    }

    private void GameManager_OnGameStarted()
    {
        ReadyMessage.SetActive(false);
    }

    private void GameManager_OnGameOver()
    {
        GameOverMessage.SetActive(true);
    }



    void Update()
    {

    }
}
