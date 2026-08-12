using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _currentScore;
    private int HighestScore;

    public int CurrentScore { get => _currentScore; }
    public int highestScore { get => HighestScore; }


    public event Action<int> OnScoreChange;


    public event Action<int> OnHighScoreChanged;

    private void Awake()
    {
        HighestScore = PlayerPrefs.GetInt("high-score", 0);
    }
    private void Start()
    {
        var allCollectibles = FindObjectsOfType<Collectable>();
        foreach (var collectable in allCollectibles)
        {
            collectable.OnCollected += Collectable_OnCollected;
        }



    }

    private void Collectable_OnCollected(int score, Collectable collectible)
    {
        _currentScore += score;
        OnScoreChange?.Invoke(_currentScore);
        if (_currentScore >= HighestScore)
        {
            HighestScore = +_currentScore;
            OnHighScoreChanged?.Invoke(highestScore);
        }
        collectible.OnCollected -= Collectable_OnCollected;

    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("high-score", HighestScore);

    }
}
