using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Movement sctipt for Elle.
/// Uses a character controller component.
/// Handles walking, running, jumping, falling.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    #region Fields
    [Header("Player Movement")]
    [Tooltip("How much the players speed decreases every second while no move key is pressed.")]
    [SerializeField] private float deceleration = 5.0f;
    [Tooltip("How much the players speed increases each second when walking.")]
    [SerializeField] private float walkAcceleration = 5.0f;
    [Tooltip("The maximum speed the player can move when walking.")]
    [SerializeField] private float maxWalkSpeed = 5.0f;
    [Tooltip("How much the players speed increases each second when running.")]
    [SerializeField] private float runAcceleration = 10.0f;
    [Tooltip("The maximum speed the player can move when running.")]
    [SerializeField] private float maxRunSpeed = 10.0f;
    /// <summary>
    /// The velocity of the players horizontal movement. Carried between frames.
    /// </summary>
    private Vector3 moveVelocity = Vector3.zero;

    [Header("Check Grounded")]
    [Tooltip("The distance from the ground that the player will detect the ground and stop falling/be able to jump.")]
    [SerializeField] float groundCheckDistance = 0.1f;
    [Tooltip("The layer that the player will detect as ground.")]
    [SerializeField] LayerMask groundedLayer;
    [Tooltip("The force applied to the player if they are not grounded.")]
    [SerializeField] private float fallingGravity = 19.62f;
    /// <summary>
    /// True if the player is on the ground.
    /// </summary>
    private bool isGrounded = true;
    /// <summary>
    /// The players velocity to be carried between frames. Used for jumping and gravity.
    /// </summary>
    private Vector3 gravityVelocity = Vector3.zero;

    /// <summary>
    /// Players character controller
    /// </summary>
    private CharacterController characterController;


    [Header("Mesh")]
    [SerializeField] private Transform playerMesh;

    [SerializeField] private float rotateSpeed = 360.0f;
    #endregion

    #region Unity Call Methods
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void LateUpdate()
    {
        Move();
        CheckGround();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Applies force to the player and moves the if they are on the ground.
    /// </summary>
    private void Move()
    {
        // If the player is not grounded maintain movement.
        if (!isGrounded)
        {
            characterController.Move(moveVelocity * Time.deltaTime);
            return;
        }

        // Get inputs.
        Vector2 moveInput = InputManager.Instance.PlayerInput.InGame.Move.ReadValue<Vector2>();
        Vector3 moveVector = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;

        // If the player has not movement input decelerate.
        if (!InputManager.Instance.PlayerInput.InGame.Move.IsInProgress())
        {
            ApplyDeceleration();
        }
        else
        {
            // Player sprinting.
            if (InputManager.Instance.PlayerInput.InGame.Sprint.IsInProgress())
            {
                if (moveVelocity.magnitude > maxRunSpeed)
                {
                    ApplyDeceleration();
                }

                float speed = moveVelocity.magnitude;

                moveVector = moveVector * Time.deltaTime * runAcceleration;

                moveVelocity += moveVector;
                if (moveVelocity.magnitude > maxRunSpeed)
                {
                    moveVelocity = moveVelocity.normalized * Mathf.Max(maxRunSpeed, speed);
                }
            }
            // Player walking.
            else
            {
                if (moveVelocity.magnitude > maxWalkSpeed)
                {
                    ApplyDeceleration();
                }

                float speed = moveVelocity.magnitude;

                moveVector = moveVector * Time.deltaTime * walkAcceleration;

                moveVelocity += moveVector;
                if (moveVelocity.magnitude > maxWalkSpeed)
                {
                    moveVelocity = moveVelocity.normalized * Mathf.Max(maxWalkSpeed, speed);
                }
            }
        }
        // Apply Movement.
        characterController.Move(moveVelocity * Time.deltaTime);

        // Rotate Mesh
        if (moveVelocity != Vector3.zero)
        {
            playerMesh.forward = moveVelocity;
        }
    }

    /// <summary>
    /// Applied decelerating force to the player and brings them to a stop.
    /// </summary>
    private void ApplyDeceleration()
    {
        if (moveVelocity.magnitude < 0.1f)
        {
            moveVelocity = Vector3.zero;
        }
        else
        {
            float speed = moveVelocity.magnitude - (deceleration * Time.deltaTime);
            moveVelocity = moveVelocity.normalized * speed;
        }

    }

    /// <summary>
    /// Checks if the player is grounded. And applies gravity to the players.
    /// </summary>
    private void CheckGround()
    {
        Vector3 checkOrigin = gameObject.transform.position;
        checkOrigin.y += characterController.radius;
        Vector3 checkEnd = checkOrigin;
        checkEnd.y -= groundCheckDistance;

        isGrounded = Physics.CheckCapsule(checkOrigin, checkEnd, characterController.radius, groundedLayer, QueryTriggerInteraction.Ignore);

        if (isGrounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0;
        }
        else if (!isGrounded)
        {
            gravityVelocity.y -= fallingGravity * Time.deltaTime;
        }

        characterController.Move(gravityVelocity * Time.deltaTime);
    }
    #endregion
}
