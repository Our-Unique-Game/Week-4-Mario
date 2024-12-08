using UnityEngine;
using TMPro; // For TextMeshPro UI
using UnityEngine.SceneManagement; // For SceneManager

public class PlayerLives : MonoBehaviour
{
    [SerializeField] private int maxLives = 3; // Maximum lives
    [SerializeField] private TextMeshProUGUI livesText; // TextMeshPro UI element for displaying lives
    private int currentLives;

    void Start()
    {
        currentLives = maxLives; // Initialize lives
        UpdateLivesText();
    }

    public void LoseLife()
    {
        currentLives--;
        UpdateLivesText();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    public void RestoreLives()
    {
        currentLives = maxLives; // Reset lives to maximum
        UpdateLivesText();
        Debug.Log("Lives restored to maximum!");
    }

    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
        else
        {
            Debug.LogError("LivesText reference is missing! Assign a TextMeshProUGUI element in the Inspector.");
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }
}
