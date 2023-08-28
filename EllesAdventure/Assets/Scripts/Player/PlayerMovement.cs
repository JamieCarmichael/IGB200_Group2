using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

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
    [Tooltip("The maximum slope the player can stand on without sliding down.")]
    [SerializeField] private float maxSlope = 45.0f;
    /// <summary>
    /// The velocity of the players horizontal movement.
    /// </summary>
    private Vector3 movementVector = Vector3.zero;
    /// <summary>
    /// The players current speed.
    /// </summary>
    private float speed = 0.0f;

    [Header("Looking")]
    [Tooltip("The transform that the camera is looking at.")]
    [SerializeField] Transform lookAtTransform;
    [Tooltip("The speed that the player turns horizontally")]
    [SerializeField] private float horizontalTurnSpeed = 10.0f;
    [Tooltip("The speed that the player turns vertically")]
    [SerializeField] private float verticalTurnSpeed = 10.0f;
    [Tooltip("The minimum and maximum angles that the player can look on the vertical axis.")]
    [SerializeField] Vector2 verticalClamp = new Vector2(-85.0f, 85.0f);

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


    [Header("Jump")]
    [Tooltip("How many meters the player can jump.")]
    [SerializeField] private float jumpHeight = 2.0f;
    [Tooltip("The force applied to the player if they are not grounded.")]
    [SerializeField] private float gravity = 9.81f;
    [Tooltip("Multiplier to increase the speed that the player falls.")]
    [SerializeField] private float fallMultiplier = 2.0f;

    [Header("Check Grounded")]
    [Tooltip("The distance from the ground that the player will detect the ground and stop falling/be able to jump.")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [Tooltip("The layer that the player will detect as ground.")]
    [SerializeField] private LayerMask groundedLayer;
    /// <summary>
    /// The players vertical velocity. Used for jumping and gravity.
    /// </summary>
    private float verticalVelocity = 0.0f;

    /*
    Is grounded checks if the player is twice the groundCheckDistance from the ground. If they are then they stop the animation and can move.
    Touch ground then checks if the player is one times the groundCheckDistance from the ground. They then stop the falling. 
    This combination allows the player to slightly leave the ground when moving along different slops without becoming ungrounded.
     */
    /// <summary>
    /// True if the player is near the ground. 
    /// </summary>
    private bool isGrounded = true;
    /// <summary>
    /// True if the player is touching the ground
    /// </summary>
    private bool touchingGround = false;
    /// <summary>
    /// The hit info from the ground check.
    /// </summary>
    private RaycastHit groundHitInfo;


    [Header("Animation")]
    [Tooltip("The animator used for the player model.")]
    [SerializeField] private Animator animator;
    [SerializeField] private string animWalk = "";
    [SerializeField] private string animRun = "";
    [SerializeField] private string animFall = "";

    /// <summary>
    /// Players character controller
    /// </summary>
    private CharacterController characterController;

    /// <summary>
    /// If true the player moves normally.
    /// </summary>
    private bool canMove = true;

    //[Header("Edges")]
    /// <summary>
    /// The player is touching an edge.
    /// </summary>
    private bool onEdge = false;
    /// <summary>
    /// The point that the player is touching the edge.
    /// </summary>
    private Vector3 edgeTouchPoint;
    /// <summary>
    /// The edge object being touched.
    /// </summary>
    private GameObject edgeObject;
    /// <summary>
    /// The player is currently hanging from an edge.
    /// </summary>
    private bool isHanging = false;
    /// <summary>
    /// The current edge can be climbed.
    /// </summary>
    private bool canClimbEdge = false;
    #endregion

    #region Unity Call Methods
    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        fromDirection = transform.forward;
        toDirection = transform.forward;
        characterController.slopeLimit = maxSlope;


        verticalCameraAngle = transform.rotation.y;
    }
    private void OnEnable()
    {
        InputManager.Instance.PlayerInput.InGame.Jump.performed += Jump;
    }
    private void OnDisable()
    {
        InputManager.Instance.PlayerInput.InGame.Jump.performed -= Jump;
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
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Finds the horizontal velocity for the player. Maintains movement speed as long as the player is using an input. Changes direction of movement instantly.
    /// </summary>
    private void Move()
    {
        // If the player is not grounded maintain movement.
        if (!isGrounded && !onEdge)
        {
            animator.SetBool(animFall, true);
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
            animator.SetBool(animWalk, false);
            animator.SetBool(animRun, false);
            // No input
            speed = Mathf.Clamp(speed - (maxRunSpeed / deceleration * Time.deltaTime), 0, speed);
            moveInput = new Vector2(movementVector.x, movementVector.z).normalized;
        }
        else if (!InputManager.Instance.PlayerInput.InGame.Sprint.IsInProgress())
        {
            animator.SetBool(animWalk, true);
            animator.SetBool(animRun, false);
            // Walking
            if (speed > maxWalkSpeed)
            {
                speed = Mathf.Clamp(speed - (maxRunSpeed / deceleration * Time.deltaTime), 0, speed);
            }
            else
            {
                speed = Mathf.Clamp(speed + (maxWalkSpeed / walkAcceleration * Time.deltaTime), 0, maxWalkSpeed);
            }
        }
        else
        {
            animator.SetBool(animWalk, false);
            animator.SetBool(animRun, true);
            // Running
            if (speed > maxRunSpeed)
            {
                speed = Mathf.Clamp(speed - (maxRunSpeed / deceleration * Time.deltaTime), 0, speed);
            }
            else
            {
                speed = Mathf.Clamp(speed + (maxRunSpeed / runAcceleration * Time.deltaTime), 0, maxRunSpeed);
            }

        }

        // Movement Accounting for angle of ground.
        Vector3 moveInputVector3 = new Vector3(moveInput.x, 0.0f, moveInput.y);
        float angle = Vector3.Angle(Vector3.up, groundHitInfo.normal);
        if (angle > maxSlope)
        {
            Vector3 left = Vector3.Cross(groundHitInfo.normal, Vector3.up);
            Vector3 slope = Vector3.Cross(groundHitInfo.normal, left);
            movementVector = slope.normalized * maxWalkSpeed;
        }
        else
        {
            movementVector = Vector3.ProjectOnPlane(moveInputVector3, groundHitInfo.normal) * speed;
        }


        if (onEdge)
        {
            OnEdge(moveInputVector3);
        }
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
        checkOrigin.y += characterController.radius;

        isGrounded = Physics.SphereCast(checkOrigin, characterController.radius, Vector3.down, out groundHitInfo, groundCheckDistance * 2, groundedLayer, QueryTriggerInteraction.Ignore);

        touchingGround = isGrounded;
        if (touchingGround)
        {
            touchingGround = Vector3.Distance(checkOrigin, groundHitInfo.point) < characterController.radius + groundCheckDistance;
        }

        if ((touchingGround && verticalVelocity < 0.0f) || isHanging)
        {
            // Grounded and falling
            verticalVelocity = 0;
        }
        else if (!isGrounded && verticalVelocity > 0.0f + groundCheckDistance)
        {
            // Jumping
            verticalVelocity -= gravity * Time.deltaTime;
        }
        else if (!touchingGround)
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

    private void Jump(InputAction.CallbackContext obj)
    {
        Jump();
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
    #endregion

    #region Public Methods
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
        float distance = toVector.magnitude - stoppingDistance;

        float timeToMove = distance / maxWalkSpeed;

        float moveTimer = 0.0f;
        Vector3 movePostion = Vector3.zero;

        animator.SetBool(animWalk, true);


        if (timeToMove < 0.0f)
        {
            timeToMove = -timeToMove;
        }


        while (moveTimer < timeToMove)
        {
            moveTimer += Time.deltaTime;
            RotatePlayer(toVector, true, timeToMove);

            movePostion = direction * maxWalkSpeed * Time.deltaTime;
            characterController.Move(movePostion);
            yield return null;
        }



        movementVector = Vector3.zero;
        animator.SetBool(animWalk, false);
        canMove = true;
    }
    #endregion

    #region Edges
    private void OnEdge(Vector3 moveInputVector3)
    {
        Vector2 moveInput = InputManager.Instance.PlayerInput.InGame.Move.ReadValue<Vector2>();

        Vector3 thisForward = transform.forward;
        Vector3 edgeForawrd = edgeObject.transform.forward;

        if (!canClimbEdge)
        {
            movementVector = moveInputVector3 * speed;
            return;
        }

        // Hit with feet
        if (GetComponent<Collider>().bounds.center.y > edgeTouchPoint.y)
        {
            if (verticalVelocity > 0.0f)
            {
                verticalVelocity = 0.0f;
            }
            movementVector = moveInputVector3 * speed;
        }
        // Hit with upper body.
        else
        {
            verticalVelocity = 0.0f;
            if (Vector3.Dot(thisForward, edgeForawrd) > 0)
            {
                if (moveInput.y > 0)
                {
                    // Hanging climb up button pressed.
                    Vector3 extents = edgeObject.GetComponent<Collider>().bounds.extents;
                    Vector3 climbPos = edgeTouchPoint + (edgeObject.transform.up * extents.y) + (edgeObject.transform.forward * extents.z) ;
                    StartCoroutine(ClimbTo(climbPos));

                    isHanging = false;
                }
                else if (moveInput.y < 0)
                {
                    // Hanging drop down button pressed.
                    movementVector = moveInputVector3 * speed;
                    isHanging = false;
                }
                else
                {
                    // Hanging no input.
                    characterController.enabled = false;
                    transform.position = edgeTouchPoint + Vector3.down;
                    movementVector = Vector3.zero;
                    characterController.enabled = true;

                    isHanging = true;
                }
            }
            else
            {
                // Moving away from edge
                isHanging = false;
            }
        }
    }

    /// <summary>
    /// Called when the player enters an edge collider. 
    /// </summary>
    /// <param name="newCollisionPoint"></param>
    /// <param name="other"></param>
    public void EnterEdge(Vector3 newCollisionPoint, GameObject other, bool canClimb)
    {
        onEdge = true;
        edgeObject = other;
        edgeTouchPoint = newCollisionPoint;
        canClimbEdge = canClimb;
    }
    /// <summary>
    /// Called when the player exits a Edge Collider.
    /// </summary>
    public void ExitEdge()
    {
        onEdge = false;
    }

    /// <summary>
    /// Moves the player to the top of the ledge they are climbing.
    /// </summary>
    /// <param name="newPosition">The position at the top of the ledge.</param>
    /// <returns></returns>
    private IEnumerator ClimbTo(Vector3 newPosition)
    {
        canMove = false;
        Vector3 toVector = newPosition - transform.position;
        float timeToMove = toVector.magnitude / maxWalkSpeed;

        float moveTimer = 0.0f;
        Vector3 movePostion = Vector3.zero;

        animator.SetBool(animWalk, true);
        // Climb up ledge.
        while (moveTimer < timeToMove)
        {
            moveTimer += Time.deltaTime;
            movePostion = toVector.normalized * maxWalkSpeed * Time.deltaTime;
            characterController.Move(movePostion);
            yield return null;
        }

        // Step forward from top of ledge.
        toVector = transform.forward;
        timeToMove = (characterController.radius * 2) / maxWalkSpeed;
        moveTimer = 0.0f;
        while (moveTimer < timeToMove)
        {
            moveTimer += Time.deltaTime;
            movePostion = toVector.normalized * maxWalkSpeed * Time.deltaTime;
            characterController.Move(movePostion);
            yield return null;
        }
        movementVector = Vector3.zero;
        animator.SetBool(animWalk, false);
        canMove = true;
    }
    #endregion
}