using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // TextMeshProUGUI for displaying the score
    private int score = 0;

    private void Start()
    {
        UpdateScoreText(); // Initialize score display
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
