using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private enum GameStates
    {
        Starting,
        playing,
        GameOver,
        Victory,
        LifeLost
    }



    public float StartupTime;

    public float LiveLostTime;

    private GhostAI[] _allGhost;
    private CharacterMotor _pacmanMotor;
    private GhostHouse _house;
    private GameStates _GameStates;
    private int _victoryCount;

    private float _lifeLostTimer;
    private bool _isGameOver;

    public event Action OnGameStarted;
    public event Action OnVictory;
    public event Action OnGameOver;
    void Start()
    {
        var allColletibles = FindObjectsOfType<Collectable>();

        _victoryCount = 0;
        foreach (var colletable in allColletibles)
        {
            _victoryCount++;
            colletable.OnCollected += Colletable_OnCollected;


        }

        var pacman = GameObject.FindWithTag("Player");
        _pacmanMotor = pacman.GetComponent<CharacterMotor>();
        _allGhost = FindObjectsOfType<GhostAI>();
        StopAllCharacters();
        _house = FindObjectOfType<GhostHouse>();
        _house.enabled = false;


        pacman.GetComponent<LIfe>().OnLifeRemoved += Pacman_OnlifeRemoved;
        _GameStates = GameStates.Starting;
    }


    private void Pacman_OnlifeRemoved(int remainnigLives)
    {
        StopAllCharacters();

        _lifeLostTimer = LiveLostTime;
        _GameStates = GameStates.LifeLost;

        _isGameOver = remainnigLives <= 0;
    }

    private void Colletable_OnCollected(int _, Collectable collectable)
    {
        _victoryCount--;

        if (_victoryCount <= 0)
        {
            _GameStates = GameStates.Victory;
            StopAllCharacters();
            OnVictory?.Invoke();
        }
        collectable.OnCollected -= Colletable_OnCollected;
    }

    private void Update()
    {
        switch (_GameStates)
        {
            case GameStates.Starting:
                StartupTime -= Time.deltaTime;


                if (StartupTime <= 0)
                {
                    _GameStates = GameStates.playing;
                    StartAllCharacters();
                    _house.enabled = true;
                    OnGameStarted?.Invoke();
                }
                break;

            case GameStates.Victory:

                if (Input.anyKey)
                {
                    print("Victory");
                    SceneManager.LoadScene(0);
                }

                break;

            case GameStates.LifeLost:
                _lifeLostTimer -= Time.deltaTime;

                if (_lifeLostTimer <= 0)
                {
                    if (_isGameOver)
                    {
                        _GameStates = GameStates.GameOver;
                        OnGameOver?.Invoke();
                    }
                    else
                    {
                        ResetAllCharacters();
                        _GameStates = GameStates.playing;

                    }
                }
                break;
            case GameStates.GameOver:
                if (Input.anyKey)
                {
                    SceneManager.LoadScene(0);
                }
                break;

        }

    }

    private void ResetAllCharacters()
    {
        _pacmanMotor.ResetPosition();

        foreach (var ghost in _allGhost)
        {
            ghost.Reset();
        }
        StartAllCharacters();
    }

    private void StartAllCharacters()
    {
        _pacmanMotor.enabled = true;

        foreach (var ghost in _allGhost)
        {
            ghost.StartMoving();
        }
    }

    private void StopAllCharacters()
    {
        _pacmanMotor.enabled = false;

        foreach (var ghost in _allGhost)
        {
            ghost.StopMoving();
        }
    }
}
