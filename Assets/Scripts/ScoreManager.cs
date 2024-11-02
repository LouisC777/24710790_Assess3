using UnityEngine;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 
    private int score = 0;

    private void Start()
    {
        UpdateScoreText(); 
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
