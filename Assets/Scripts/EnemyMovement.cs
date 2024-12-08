using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f; // Speed of the enemy
    [SerializeField] private float jumpForce = 5f; // Force of the jump
    [SerializeField] private float jumpInterval = 2f; // Interval between jumps
    [SerializeField] private float detectionDistance = 0.5f; // Distance to detect walls or edges
    private Vector3 direction = Vector3.right; // Current movement direction
    private Rigidbody enemyRigidbody; // Rigidbody for physics-based movement
    private float jumpTimer; // Timer to control jumping intervals

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();

        if (enemyRigidbody == null)
        {
            Debug.LogError("Rigidbody component is missing on the enemy.");
        }
    }

    void Update()
    {
        // Horizontal movement
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // Check for walls or edges using a raycast
        if (IsObstacleInFront())
        {
            // Reverse direction
            direction = -direction;
        }

        // Handle jumping
        jumpTimer += Time.deltaTime;
        if (jumpTimer >= jumpInterval)
        {
            Jump();
            jumpTimer = 0f; // Reset timer after jumping
        }
    }

    private bool IsObstacleInFront()
    {
        // Cast a ray in the direction of movement
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, detectionDistance))
        {
            // Check if the ray hit a wall or obstacle
            if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("InvisibleWall"))
            {
                return true;
            }
        }
        return false;
    }

    private void Jump()
    {
        if (enemyRigidbody != null && Mathf.Abs(enemyRigidbody.linearVelocity.y) < 0.01f)
        {
            // Apply vertical force to make the enemy jump
            enemyRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
