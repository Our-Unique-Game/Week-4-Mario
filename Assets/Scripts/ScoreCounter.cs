using System;
using TMPro; // For TextMeshPro
using UnityEngine;

/**
 * This component tracks the score of the player.
 * The score increases when the player picks up a coin.
 */
public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private int scorePerCoin = 1; // Score to add per coin
    private TextMeshPro scoreText; // Reference to TextMeshPro
    private int score = 0; // Current score

    void Start()
    {
        // Find the "Score" child object and get its TextMeshPro component
        var scoreChild = transform.Find("Score");
        if (!scoreChild)
        {
            throw new Exception("No child with name 'Score' found under the Player!");
        }
        scoreText = scoreChild.GetComponent<TextMeshPro>();
        if (!scoreText)
        {
            throw new Exception("No TextMeshPro component found on the 'Score' GameObject!");
        }

        // Initialize score
        UpdateScore(0);
    }

    private void UpdateScore(int newScore)
    {
        score = newScore;
        scoreText.text = score.ToString();
    }

    public void CollectCoin()
    {
        Debug.Log("Coin collected! Adding score...");
        UpdateScore(score + scorePerCoin);
    }
}
