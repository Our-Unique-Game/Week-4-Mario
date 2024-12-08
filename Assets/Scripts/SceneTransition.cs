using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // The name of the scene to load

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collides with the star
        if (other.CompareTag("Star"))
        {
            // Load the specified scene
            Debug.Log("Collision with Star detected!");
            LoadScene();
        }
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Scene name is not set in the Inspector!");
        }
    }
}
