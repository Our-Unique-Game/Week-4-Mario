using UnityEngine;
using UnityEngine.SceneManagement; // For scene transitions
using TMPro; // For TextMeshPro

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f; // Bounce force when jumping on the enemy
    [SerializeField] private int lives = 3;        // Initial lives
    [SerializeField] private TextMeshProUGUI livesText; // Reference to the UI text element

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        UpdateLivesText();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle collisions with various objects
        if (other.CompareTag("Enemy"))
        {
            HandleEnemyCollision(other);
        }
        else if (other.CompareTag("Star"))
        {
            // Load the next scene
            LoadNextScene();
        }
        else if (other.CompareTag("Mushroom"))
        {
            RestoreLives();
            Destroy(other.gameObject); // Destroy the mushroom after collection
        }
    }

    void HandleEnemyCollision(Collider enemy)
    {
        // Check if the player is above the enemy to "kill" it
        if (transform.position.y > enemy.transform.position.y + 0.5f)
        {
            Destroy(enemy.gameObject);
            Jump();
        }
        else
        {
            LoseLife();
        }
    }

    void Jump()
    {
        // Simulate a jump by applying upward motion manually
        Vector3 jumpVelocity = Vector3.up * jumpForce;
        controller.Move(jumpVelocity * Time.deltaTime);
    }

    void LoseLife()
    {
        lives--;
        UpdateLivesText();

        if (lives <= 0)
        {
            RestartScene();
        }
    }

    void RestoreLives()
    {
        lives++; // Reset lives to the maximum value
        UpdateLivesText();
        Debug.Log("Live added!");
    }

    void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
        else
        {
            Debug.LogError("LivesText reference is missing!");
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
