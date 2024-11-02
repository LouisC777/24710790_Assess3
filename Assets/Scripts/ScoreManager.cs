using UnityEngine;
using UnityEngine.UI; // Import this if you're using standard UI Text
using TMPro; // Uncomment this if you're using TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public int score = 0; // Initial score
    public Text scoreText; // Reference to the UI text element (for standard UI)
    // public TMP_Text scoreText; // Uncomment this if using TextMeshPro

    void Start()
    {
        // Find the UI Text GameObject by name and get the Text component
        scoreText = GameObject.Find("Score").GetComponent<Text>(); // For standard UI
        // scoreText = GameObject.Find("YourTextObjectName").GetComponent<TMP_Text>(); // For TextMeshPro
        
        UpdateScoreUI();
    }

    public void AddScore(int points)
    {
        score += points; // Increase score by the specified points
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // Update the UI text
        }
    }
}
