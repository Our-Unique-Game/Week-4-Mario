using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class KeyboardMover : MonoBehaviour
{
    CharacterController controller;

    [Header("Horizontal movement")]
    [SerializeField] InputAction moveHorizontal = new InputAction(type: InputActionType.Button);

    [Tooltip("Horizontal acceleration when clicking the arrows, in meters per second^2")]
    [SerializeField] float feetAcceleration = 100.0f;

    [Tooltip("Largest horizontal speed that the character can attain, in meters per second.")]
    [SerializeField] float maxSpeed = 10.0f;

    [Tooltip("If the speed is at least this value, the character will be slowed down by friction.")]
    [SerializeField] float minSpeedForFriction = 0.01f;

    [Tooltip("Horizontal acceleration of friction, against the direction of movement, in meters per second^2")]
    [SerializeField] float frictionAcceleration = 20.0f;

    [Header("Vertical movement")]
    [Tooltip("Vertical acceleration when free-falling, in meters per second^2")]
    [SerializeField] float gravityAcceleration = -20.0f;

    [Tooltip("Gravity multiplier for falling to create a snappier fall.")]
    [SerializeField] float fallMultiplier = 2.5f;

    [Tooltip("Gravity multiplier when holding the jump button for a higher jump.")]
    [SerializeField] float lowJumpMultiplier = 1.5f;

    [SerializeField] InputAction jump;

    [Tooltip("Vertical speed immediately after jumping, in meters per second")]
    [SerializeField] float jumpSpeed = 12.0f;

    [Tooltip("Maximum time the jump button can be held to increase jump height.")]
    [SerializeField] float maxJumpHoldTime = 0.2f;

    [Header("These fields are for display only")]
    [SerializeField] Vector3 velocity = Vector3.zero;
    [SerializeField] bool controllerIsGrounded;

    private float jumpHoldTime = 0f;

    void OnValidate()
    {
        if (jump == null)
            jump = new InputAction(type: InputActionType.Button);
        if (jump.bindings.Count == 0)
            jump.AddBinding("<Keyboard>/space");

        if (moveHorizontal == null)
            moveHorizontal = new InputAction(type: InputActionType.Button);
        if (moveHorizontal.bindings.Count == 0)
            moveHorizontal.AddCompositeBinding("1DAxis")
                .With("Positive", "<Keyboard>/rightArrow")
                .With("Negative", "<Keyboard>/leftArrow");
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveHorizontal.Enable();
        jump.Enable();
    }

    private void OnDisable()
    {
        moveHorizontal.Disable();
        jump.Disable();
    }

    private float DeltaVelocityWalking()
    {
        float accelerationX = moveHorizontal.ReadValue<float>() * feetAcceleration;
        if (velocity.x > minSpeedForFriction)
            accelerationX -= frictionAcceleration;
        else if (velocity.x < -minSpeedForFriction)
            accelerationX += frictionAcceleration;

        float deltaVelocityX = accelerationX * Time.deltaTime;
        if (Mathf.Abs(velocity.x + deltaVelocityX) > maxSpeed)
            deltaVelocityX = 0;

        return deltaVelocityX;
    }

    void Update()
    {
        if (!controller.enabled) return;

        controllerIsGrounded = controller.isGrounded;

        // Calculate horizontal movement
        velocity.x += DeltaVelocityWalking();
        if (Mathf.Abs(velocity.x) < minSpeedForFriction && controllerIsGrounded)
            velocity.x = 0;

        if (controllerIsGrounded)
        {
            jumpHoldTime = 0f; // Reset jump hold time when grounded
            if (jump.WasPressedThisFrame())
            {
                velocity.y = jumpSpeed;
            }
        }
        else
        {
            // Apply gravity
            if (velocity.y > 0 && jump.IsPressed() && jumpHoldTime < maxJumpHoldTime)
            {
                // Allow higher jumps while holding the jump button
                velocity.y += gravityAcceleration * lowJumpMultiplier * Time.deltaTime;
                jumpHoldTime += Time.deltaTime;
            }
            else
            {
                // Apply stronger gravity for falling
                velocity.y += gravityAcceleration * fallMultiplier * Time.deltaTime;
            }
        }

        var deltaPosition = velocity * Time.deltaTime;
        controller.Move(deltaPosition);
    }
}
