using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{

    public TextMeshProUGUI HighScoreText;
    public TextMeshProUGUI CurrenScoreText;




    void Start()
    {
        var scoreManager = FindObjectOfType<ScoreManager>();
        scoreManager.OnScoreChange += ScoreManager_OnScoreChange;
        scoreManager.OnHighScoreChanged += ScoreManager_OnHighScoreChanged;
        HighScoreText.text = $"{scoreManager.highestScore:00}";
    }

    private void ScoreManager_OnHighScoreChanged(int obj)
    {
        HighScoreText.text = $"{obj:00}";
    }

    private void ScoreManager_OnScoreChange(int Score)
    {
        CurrenScoreText.text = $"{Score:00}";
    }


}
