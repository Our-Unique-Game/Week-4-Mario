using UnityEngine;

/**
 * This component makes an object collectible by the player.
 * When the player enters its trigger, it increases the score and destroys the object.
 */
public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggered by: {other.name}"); // Log the name of the object that triggered the event

        // Check if the colliding object has a ScoreCounter component
        ScoreCounter player = other.GetComponent<ScoreCounter>();
        if (player != null)
        {
            Debug.Log("Player detected! Updating score...");
            player.CollectCoin();
            Destroy(gameObject); // Destroy the coin after collection
        }
        else
        {
            Debug.LogWarning("No ScoreCounter component found on the colliding object!");
        }
    }
}
