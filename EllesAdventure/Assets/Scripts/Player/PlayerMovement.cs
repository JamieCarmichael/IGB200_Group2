using System.Collections;
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
    [Tooltip("How many seconds it takes the player to go from the maximum moving speed to 0 if no key is pressed.")]
    [SerializeField] private float deceleration = 10.0f;
    [Tooltip("How many seconds it takes the player to go from 0 to the maximum move speed.")]
    [SerializeField] private float moveAcceleration = 5.0f;
    [Tooltip("The maximum speed the player can move when moving.")]
    [SerializeField] private float maxMoveSpeed = 5.0f;
    /// <summary>
    /// The velocity of the players horizontal movement.
    /// </summary>
    private Vector3 movementVector = Vector3.zero;
    /// <summary>
    /// The velocity of the players horizontal movement.
    /// </summary>
    public Vector3 MovementVector { get { return movementVector; } }

    /// <summary>
    /// The players current speed.
    /// </summary>
    private float speed = 0.0f;


    [Tooltip("The force applied to the player if they are not grounded.")]
    [SerializeField] private float gravity = 9.81f;
    [Tooltip("Multiplier to increase the speed that the player falls.")]
    [SerializeField] private float fallMultiplier = 2.0f;

    [Header("Looking")]
    [Tooltip("The transform that the camera is looking at.")]
    [SerializeField] Transform lookAtTransform;
    [Tooltip("The minimum and maximum angles that the player can look on the vertical axis.")]
    [SerializeField] Vector2 verticalClamp = new Vector2(-85.0f, 85.0f);

    /// <summary>
    /// The speed that the player turns horizontally
    /// </summary>
    private float horizontalTurnSpeed;
    /// <summary>
    /// The speed that the player turns vertically.
    /// </summary>
    private float verticalTurnSpeed;
    /// <summary>
    /// The cameras rotation on the X axis.
    /// </summary>
    private float verticalCameraAngle;
    /// <summary>
    /// The movement direction before the latest direction.
    /// </summary>
    private Vector3 fromDirection = Vector3.zero;
    /// <summary>
    /// The current direction that the player is moving in.
    /// </summary>
    private Vector3 toDirection = Vector3.zero;
    /// <summary>
    /// True if the mesh is in the pregress of rotating.
    /// </summary>
    private bool isRotating = false;
    /// <summary>
    /// A timer for how much long the rotation has been going for.
    /// </summary>
    private float rotateTimer = 0.0f;

    private const string MOUSE_SENSITIVITY_X = "MouseSensitivityX";
    private const string MOUSE_SENSITIVITY_Y = "MouseSensitivityY";
    private float defaultSensitivityValue = 10.0f;


    [Header("Check Grounded")]
    [Tooltip("The distance from the ground that the player will detect the ground and stop falling/be able to jump.")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [Tooltip("The layer that the player will detect as ground.")]
    [SerializeField] private LayerMask groundedLayer;
    [Tooltip("If the player is below this height the are sent back to the last position they were grounded.")]
    [SerializeField] private float minYHieght = -5.0f;
    /// <summary>
    /// The players vertical velocity. Used for jumping and gravity.
    /// </summary>
    private float verticalVelocity = 0.0f;

    /// <summary>
    /// True if the player is touching the ground. 
    /// </summary>
    private bool isGrounded = true;
    /// <summary>
    /// Checks if the player is clise to the ground. Checks twice the groundCheckDistance.
    /// </summary>
    private bool closeToGround = true;
    /// <summary>
    /// True if the player is touching the ground. 
    /// </summary>
    public bool IsGrounded {  get { return isGrounded; } }

    /// <summary>
    /// The last position that the player was on the ground
    /// </summary>
    private Vector3 lastGroundedPosition = Vector3.zero;
    /// <summary>
    /// Has the ground last ground position be adjusted to be within play area.
    /// </summary>
    private bool groundSet = false;

    [Header("Animation")]
    [Tooltip("The animator used for the player model.")]
    [SerializeField] private Animator animator;
    [SerializeField] private string animMove = "";
    [SerializeField] private string animFall = "";

    /// <summary>
    /// Players character controller
    /// </summary>
    private CharacterController characterController;

    /// <summary>
    /// If true the player moves normally.
    /// </summary>
    private bool canMove = true;

    [Header("Audio")]
    private AudioSource audioSource;

    [SerializeField] private AudioClip walkingSound;

    [SerializeField] private AnimationCurve pitchChange;
    [SerializeField] private float pitchChangeTime;
    private float pitchTimer = 0.0f;
    #endregion

    #region Unity Call Methods
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        fromDirection = transform.forward;
        toDirection = transform.forward;

        verticalCameraAngle = transform.rotation.y;

        RetrieveMouseSensitivity();
    }
    private void OnEnable()
    {
        verticalVelocity = 0.0f;
        speed = 0.0f;
        movementVector = Vector3.zero;      
    }

    private void LateUpdate()
    {
        if (canMove)
        {
            CollisionCheck();
            CheckGround();


            Look(InputManager.Instance.PlayerInput.InGame.Aim.ReadValue<Vector2>());

            Move();

            // Move
            characterController.Move(new Vector3(movementVector.x, movementVector.y + verticalVelocity, movementVector.z) * Time.deltaTime);

            CheckWithinPlayArea();
        }

        audioSource.pitch = pitchChange.Evaluate(pitchTimer / pitchChangeTime);
        pitchTimer += Time.deltaTime;
        while (pitchTimer > pitchChangeTime)
        {
            pitchTimer -= pitchChangeTime;
        }
        if (animator.GetBool(animMove))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            audioSource.UnPause();
            audioSource.clip = walkingSound;
        }
        else
        {
            audioSource.Pause();
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Finds the horizontal velocity for the player. Maintains movement speed as long as the player is using an input. Changes direction of movement instantly.
    /// </summary>
    private void Move()
    {    
        if (!isGrounded)
        { 
            //animator.SetBool(animFall, true);
            animator.SetBool(animFall, !closeToGround);
            movementVector.y = 0.0f;
            return;
        }

        animator.SetBool(animFall, false);

        // Get Input
        Vector2 moveInput = InputManager.Instance.PlayerInput.InGame.Move.ReadValue<Vector2>();
        moveInput = moveInput.normalized;
        bool hasInput = InputManager.Instance.PlayerInput.InGame.Move.IsInProgress();

        float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        moveInput = new Vector2(moveDirection.x, moveDirection.z);

        if (!hasInput)
        {
            animator.SetBool(animMove, false);
            // No input
            speed = Mathf.Clamp(speed - (maxMoveSpeed / deceleration * Time.deltaTime), 0, speed);
            moveInput = new Vector2(movementVector.x, movementVector.z).normalized;
        }
        else
        {
            animator.SetBool(animMove, true);
            // Running
            if (speed > maxMoveSpeed)
            {
                speed = Mathf.Clamp(speed - (maxMoveSpeed / deceleration * Time.deltaTime), 0, speed);
            }
            else
            {
                speed = Mathf.Clamp(speed + (maxMoveSpeed / moveAcceleration * Time.deltaTime), 0, maxMoveSpeed);
            }

        }

        Vector3 moveInputVector3 = new Vector3(moveInput.x, 0.0f, moveInput.y);
        movementVector = moveInputVector3 * speed;
    }

    /// <summary>
    /// Rotate the player to face the current movement direction.
    /// </summary>
    /// </summary>
    /// <param name="directionVector">The direction to turn towards.</param>
    /// <param name="hasInput">If the player is currently making an input</param>
    private void RotatePlayer(Vector3 directionVector, bool hasInput, float timeToRotate)
    {
        // Turning
        // Check if input has changed.
        if (toDirection != directionVector && hasInput)
        {
            directionVector.y = 0.0f;
            fromDirection = transform.forward;
            toDirection = directionVector;
            rotateTimer = 0.0f;
            isRotating = true;
        }
        // Rotate player if they are not facing the movement direction.
        if (isRotating)
        {
            rotateTimer += Time.deltaTime;

            float angle = Vector3.Angle(fromDirection, toDirection) / timeToRotate;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(toDirection), angle * Time.deltaTime);
            if (rotateTimer >= timeToRotate)
            {
                isRotating = false;
            }
        }
    }

    /// <summary>
    /// Checks if the player is grounded. And applies gravity to the players.
    /// </summary>
    private void CheckGround()
    {
        Vector3 checkOrigin = gameObject.transform.position;
        checkOrigin.y += characterController.radius - groundCheckDistance;

        isGrounded = Physics.CheckSphere(checkOrigin, characterController.radius, groundedLayer, QueryTriggerInteraction.Ignore);

        closeToGround = Physics.Raycast(checkOrigin, Vector3.down, characterController.radius + (groundCheckDistance * 2), groundedLayer, QueryTriggerInteraction.Ignore);

        if (isGrounded && verticalVelocity <= 0.0f)
        {
            // Grounded and falling
            verticalVelocity = -speed;
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
    /// Check if the player is in the play area.
    /// Move player back to the last grounded position if Elle is below min Y.
    /// </summary>
    private void CheckWithinPlayArea()
    {
        if (isGrounded)
        {
            // set postion when touching ground.
            lastGroundedPosition = transform.position;
            groundSet = false;
        }
        else if (!groundSet)
        {
            // Move position back to away from edge.
            groundSet = true;

            Vector3 dir = (lastGroundedPosition - transform.position);
            dir.y = 0.0f;
            dir = dir.normalized;
            lastGroundedPosition = lastGroundedPosition + (dir * characterController.radius * 2);
            lastGroundedPosition.y += characterController.radius * 2;
        }
        // Reset position
        if (transform.position.y < minYHieght)
        {
            transform.position = lastGroundedPosition;
            speed = 0.0f;
            verticalVelocity = 0.0f;
            movementVector = Vector3.zero;
        }
    }



    /// <summary>
    /// Checks if the player collided with something and stops velocity on that plane.
    /// </summary>
    private void CollisionCheck()
    {
        if (characterController.collisionFlags == CollisionFlags.Above && verticalVelocity > 0)
        {
            verticalVelocity = 0;
        }
    }

    /// <summary>
    /// Handles inputs for looking around. Rotates the player and looksm up and down.
    /// </summary>
    /// <param name="rotation"></param>
    private void Look(Vector2 rotation)
    {
        transform.rotation *= Quaternion.Euler(Vector3.up * rotation.x * horizontalTurnSpeed * Time.deltaTime);

        verticalCameraAngle -= rotation.y * verticalTurnSpeed * Time.deltaTime;
        verticalCameraAngle = Mathf.Clamp(verticalCameraAngle, verticalClamp.x, verticalClamp.y);
        lookAtTransform.localRotation = Quaternion.Euler(verticalCameraAngle, 0.0f, 0.0f);

    }

    /// <summary>
    /// Sets the mouse sensitivity values.
    /// </summary>
    private void RetrieveMouseSensitivity()
    {
        horizontalTurnSpeed = PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_X, defaultSensitivityValue);
        verticalTurnSpeed = PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_Y, defaultSensitivityValue);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Sets the mouse sensitivity.
    /// </summary>
    /// <param name="xValue"></param>
    /// <param name="yValue"></param>
    public void SetMouseSentitivity(float xValue, float yValue)
    {
        horizontalTurnSpeed = xValue;
        verticalTurnSpeed = yValue;
    }

    /// <summary>
    /// Stops regular movement and moves the player to a position.
    /// </summary>
    /// <param name="direction">The direction to the destination.</param>
    /// <param name="distance">The distance to the destination.</param>
    /// <returns></returns>
    public IEnumerator MoveTo(Vector3 closestPoint, float stoppingDistance)
    {
        canMove = false;

        Vector3 toVector = closestPoint - transform.position;
        Vector3 direction = toVector.normalized;
        direction.y = -1.0f;
        float distance = toVector.magnitude - stoppingDistance;

        float timeToMove = distance / maxMoveSpeed;

        float moveTimer = 0.0f;
        Vector3 movePostion = Vector3.zero;

        animator.SetBool(animMove, true);


        if (timeToMove < 0.0f)
        {
            timeToMove = -timeToMove;
        }


        while (moveTimer < timeToMove)
        {
            moveTimer += Time.deltaTime;
            RotatePlayer(toVector, true, timeToMove);

            movePostion = direction * maxMoveSpeed * Time.deltaTime;

            characterController.Move(movePostion);
            yield return null;
        }



        movementVector = Vector3.zero;
        animator.SetBool(animMove, false);
        canMove = true;
    }
    #endregion

    #region Jump
    /*
        

    [Header("Jump")]
    [Tooltip("How many meters the player can jump.")]
    [SerializeField] private float jumpHeight = 2.0f;
    
    private void OnEnable()
    {
        verticalVelocity = 0.0f;
        speed = 0.0f;
        movementVector = Vector3.zero;      

        InputManager.Instance.PlayerInput.InGame.Jump.performed += Jump;
    }
    private void OnDisable()
    {
        InputManager.Instance.PlayerInput.InGame.Jump.performed -= Jump;
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

    private void Jump(InputAction.CallbackContext obj)
    {
        Jump();
    }
    */
    #endregion
}