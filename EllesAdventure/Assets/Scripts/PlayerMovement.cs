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
    [Tooltip("How many seconds it takes the player to go from the maximum running speed to 0 if no key is pressed.")]
    [SerializeField] private float deceleration = 10.0f;
    [Tooltip("How many seconds it takes the player to go from 0 to the maximum walking speed.")]
    [SerializeField] private float walkAcceleration = 5.0f;
    [Tooltip("The maximum speed the player can move when walking.")]
    [SerializeField] private float maxWalkSpeed = 5.0f;
    [Tooltip("How many seconds it takes the player to go from 0 to the maximum running speed.")]
    [SerializeField] private float runAcceleration = 10.0f;
    [Tooltip("The maximum speed the player can move when running.")]
    [SerializeField] private float maxRunSpeed = 10.0f;

    /// <summary>
    /// The velocity of the players horizontal movement.
    /// </summary>
    private Vector2 horizontailMovement = Vector2.zero;

    [Header("Rotation")]
    [Tooltip("The players visuals. Rotated to face the players movement direction.")]
    [SerializeField] private Transform playerMesh;
    [Tooltip("How many seconds it takes for the player to turn 180 degrees.")]
    [SerializeField] private float rotateSpeed = 10.0f;

    [Header("Jump")]
    [Tooltip("How many meters the player can jump.")]
    [SerializeField] private float jumpHeight = 2.0f;
    [Tooltip("The force applied to the player if they are not grounded.")]
    [SerializeField] private float gravity = 9.81f;
    [Tooltip("Multiplier to increase the speed that the player falls.")]
    [SerializeField] private float fallMultiplier = 2.0f;

    [Header("Check Grounded")]
    [Tooltip("The distance from the ground that the player will detect the ground and stop falling/be able to jump.")]
    [SerializeField] float groundCheckDistance = 0.1f;
    [Tooltip("The layer that the player will detect as ground.")]
    [SerializeField] LayerMask groundedLayer;
    /// <summary>
    /// True if the player is on the ground.
    /// </summary>
    private bool isGrounded = true;
    /// <summary>
    /// The players verticle velocity. Used for jumping and gravity.
    /// </summary>
    private float verticalVelocity = 0.0f;

    /// <summary>
    /// Players character controller
    /// </summary>
    private CharacterController characterController;
    #endregion

    #region Unity Call Methods
    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        InputManager.Instance.PlayerInput.InGame.Jump.performed += context => Jump();
    }

    private void LateUpdate()
    {
        CollisionCheck();
        Move();
        CheckGround();

        // Move
        characterController.Move(new Vector3(horizontailMovement.x, verticalVelocity, horizontailMovement.y) * Time.deltaTime);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Finds the horizontal velocity for the player.
    /// </summary>
    private void Move()
    {
        // If the player is not grounded maintain movement.
        if (!isGrounded)
        {
            return;
        }

        // Get Input
        Vector2 moveInput = InputManager.Instance.PlayerInput.InGame.Move.ReadValue<Vector2>();
        moveInput = moveInput.normalized;


        // Turning
        Vector3 moveDirection = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;
        Vector3 rotationDirection = Vector3.RotateTowards(playerMesh.forward, moveDirection, (1 / rotateSpeed * 2) * Time.deltaTime, 0.0f);
        playerMesh.rotation = Quaternion.LookRotation(rotationDirection);

        if (!InputManager.Instance.PlayerInput.InGame.Move.IsInProgress())
        {
            // No input
            horizontailMovement = Vector2.MoveTowards(horizontailMovement, Vector2.zero, (maxRunSpeed / deceleration) * Time.deltaTime);
        }
        else if (!InputManager.Instance.PlayerInput.InGame.Sprint.IsInProgress())
        {
            // Walking
            horizontailMovement = Vector2.MoveTowards(horizontailMovement, moveInput * maxWalkSpeed, (maxWalkSpeed / walkAcceleration) * Time.deltaTime);
        }
        else
        {
            // Running
            horizontailMovement = Vector2.MoveTowards(horizontailMovement, moveInput * maxRunSpeed, (maxRunSpeed / runAcceleration) * Time.deltaTime);
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

        if (isGrounded && verticalVelocity < 0.0f)
        {
            // Grounded and falling
            verticalVelocity = 0;
        }
        else if (!isGrounded && verticalVelocity > 0.0f)
        {
            // Jumping
            verticalVelocity -= gravity * Time.deltaTime;
        }
        else if (!isGrounded)
        {
            // Falling
            verticalVelocity -= gravity * fallMultiplier * Time.deltaTime;
        }
    }

    /// <summary>
    /// Apply upwards velcoity to the player. Make them jump.
    /// </summary>
    private void Jump()
    {
        if (isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * 2 * gravity);
        }
    }

    /// <summary>
    /// Checks if the player collided with something and stops velocity on that plane.
    /// </summary>
    private void CollisionCheck()
    {
        // 
        if (characterController.collisionFlags == CollisionFlags.Sides)
        {
            horizontailMovement = Vector2.zero;
        }
        // Hit head
        if (characterController.collisionFlags == CollisionFlags.Above && verticalVelocity > 0)
        {
            verticalVelocity = 0;
        }
    }
    #endregion
}
