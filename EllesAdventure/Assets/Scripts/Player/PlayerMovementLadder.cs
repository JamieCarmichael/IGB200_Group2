using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Player movement while on a ladder.
/// </summary>
public class PlayerMovementLadder : MonoBehaviour 
{
    #region Fields
    [Tooltip("The speed that the player climbs. The player climbs this many metres per second.")]
    [SerializeField] private float climbSpeed = 2.0f;
    [Tooltip("How far from the bottom of ladder the player will stop climbing the ladder.")]
    [SerializeField] private float getOffLadderDiatance = 0.5f;

    /// <summary>
    /// The players character controller.
    /// </summary>
    private CharacterController characterController;
    /// <summary>
    /// Half the hieght of the player.
    /// </summary>
    private float halfwidth;
    /// <summary>
    /// Half the width of the player.
    /// </summary>
    private float halfHeight;
    /// <summary>
    /// The bottom of the ladder being climbed.
    /// </summary>
    private Vector3 ladderBottom;
    /// <summary>
    /// The top of the ladder being climbed.
    /// </summary>
    private Vector3 ladderTop;
    /// <summary>
    /// The Direction that the player is climbing the ladder.
    /// </summary>
    private Vector3 upDirection;
    /// <summary>
    /// If true the player can use inputs to climb the ladder. If false they are climbing onto of off the ladder.
    /// </summary>
    private bool inputAccepted = true;
    /// <summary>
    /// The ladder being climbed.
    /// </summary>
    private Ladder ladder;


    [Header("Animation")]
    [Tooltip("The animator used for the player model.")]
    [SerializeField] private Animator animator;
    [SerializeField] private string animClimb = "";
    #endregion

    #region Unity Call Functions
    private void LateUpdate()
    {
        if (inputAccepted)
        {
            ClimbLadder();
        }
    }
    private void OnEnable()
    {
        InputManager.Instance.PlayerInput.InGame.Interact.performed += DetachFromLadder;
    }
    private void OnDisable()
    {
        InputManager.Instance.PlayerInput.InGame.Interact.performed -= DetachFromLadder;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Attach the player to a ladder.
    /// </summary>
    /// <param name="ladder"></param>
    public void AttachToLadder(Ladder ladder)
    {
        characterController = GetComponent<CharacterController>();
        halfwidth = characterController.radius;
        halfHeight = characterController.height / 2;

        inputAccepted = true;
        this.ladder = ladder;

        Collider collider = ladder.ThisCollider;
        Bounds bounds = collider.bounds;

        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;
        ladderBottom = collider.ClosestPoint(center + new Vector3(0, -extents.y, -extents.z + halfwidth));
        ladderTop = collider.ClosestPoint(center + new Vector3(0, extents.y, -extents.z + halfwidth));
        upDirection = (ladderTop - ladderBottom).normalized;

        // If player is above ladder then climb down onto it.
        if (transform.position.y + halfHeight > ladderTop.y)
        {
            StartCoroutine(GetOnTopOfLadder());
        }
        else
        {
            StartCoroutine(GetOnBottomOfLadder());

            //RotatePlayer(ladder.transform.forward, 0.0f);
        }
    }

    #endregion

    #region Private Methods
    /// <summary>
    /// Climb up and down the ladder.
    /// Movement on ladder.
    /// </summary>
    private void ClimbLadder()
    {
        Vector2 moveInput = InputManager.Instance.PlayerInput.InGame.Move.ReadValue<Vector2>();
        
        if(moveInput.y > 0)
        {
            characterController.Move(upDirection * climbSpeed * Time.deltaTime);
            if (transform.position.y + halfHeight > ladderTop.y)
            {
                ClimbOverLadder();
            }
        }
        else if (moveInput.y < 0)
        {
            characterController.Move(-upDirection * climbSpeed * Time.deltaTime);
            if (transform.position.y - getOffLadderDiatance < ladderBottom.y)
            {
                DetachFromLadder();
            }
        }
    }
    /// <summary>
    /// Get off the top of the ladder.
    /// </summary>
    private void ClimbOverLadder()
    {
        StartCoroutine(ClimbTo(ladderTop));
    }
    /// <summary>
    /// Get off the ladder. Will disconnect and return to normal movement.
    /// </summary>
    /// <param name="obj"></param>
    private void DetachFromLadder(InputAction.CallbackContext obj)
    {
        DetachFromLadder();
    }
    /// <summary>
    /// Get off the ladder. Will disconnect and return to normal movement.
    /// </summary>
    private void DetachFromLadder()
    {
        PlayerManager.Instance.StandardMovement();
    }
    /// <summary>
    /// Climb to a position. Used for getting off the ladder.
    /// </summary>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    private IEnumerator ClimbTo(Vector3 newPosition)
    {
        inputAccepted = false;
        Vector3 toVector = newPosition - transform.position;
        float timeToMove = toVector.magnitude / climbSpeed;

        float moveTimer = 0.0f;
        Vector3 movePostion = Vector3.zero;

        animator.SetBool(animClimb, true);
        // Climb up ledge.
        while (moveTimer < timeToMove)
        {
            moveTimer += Time.deltaTime;
            movePostion = toVector.normalized * climbSpeed * Time.deltaTime;
            characterController.Move(movePostion);
            yield return null;
        }

        // Step forward from top of ledge.
        toVector = transform.forward;
        timeToMove = (halfwidth * 2) / climbSpeed;
        moveTimer = 0.0f;
        while (moveTimer < timeToMove)
        {
            moveTimer += Time.deltaTime;
            movePostion = toVector.normalized * climbSpeed * Time.deltaTime;
            characterController.Move(movePostion);
            yield return null;
        }
        animator.SetBool(animClimb, false);
        inputAccepted = true;

        PlayerManager.Instance.StandardMovement();
    }

    /// <summary>
    /// Move the player onto the top of the ladder. Used to get on the ladder from the top.
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetOnTopOfLadder()
    {
        inputAccepted = false;

        Vector3 movePostion = Vector3.zero;

        Vector3 toVector = (ladderTop - (ladder.transform.forward * (halfwidth * 2))) - transform.position;
        float timeToMove = toVector.magnitude / climbSpeed;
        float moveTimer = 0.0f;

        animator.SetBool(animClimb, true);
        // Climb up ledge.
        while (moveTimer < timeToMove)
        {
            moveTimer += Time.deltaTime;
            movePostion = toVector.normalized * climbSpeed * Time.deltaTime;
            characterController.Move(movePostion);

            RotatePlayer(ladder.transform.forward, timeToMove - moveTimer);
            yield return null;
        }

        // Climb down ladder.
        toVector = -upDirection;
        timeToMove = halfHeight / climbSpeed;
        moveTimer = 0.0f;
        while (moveTimer < timeToMove)
        {
            moveTimer += Time.deltaTime;
            movePostion = toVector.normalized * climbSpeed * Time.deltaTime;
            characterController.Move(movePostion);
            yield return null;
        }
        animator.SetBool(animClimb, false);
        inputAccepted = true;
    }

    /// <summary>
    /// Move the player onto the bottom of the ladder. Used to get on the ladder from the bottom.
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetOnBottomOfLadder()
    {
        inputAccepted = false;

        Vector3 movePostion = Vector3.zero;

        Vector3 toVector = (ladderBottom - (ladder.transform.forward * (halfwidth * 2))) - transform.position;
        float timeToMove = toVector.magnitude / climbSpeed;
        float moveTimer = 0.0f;

        animator.SetBool(animClimb, true);
        // Climb up ledge.
        while (moveTimer < timeToMove)
        {
            moveTimer += Time.deltaTime;
            movePostion = toVector.normalized * climbSpeed * Time.deltaTime;
            characterController.Move(movePostion);

            RotatePlayer(ladder.transform.forward, timeToMove - moveTimer);
            yield return null;
        }
        animator.SetBool(animClimb, false);
        inputAccepted = true;
    }

    /// <summary>
    /// Rotate the player to face the current movement direction.
    /// </summary>
    /// </summary>
    /// <param name="moveInput">The current player input</param>
    /// <param name="hasInput">If the player is currently making an input</param>
    private void RotatePlayer(Vector3 toDirection, float timeToMove)
    {
        float angle = Vector3.Angle(transform.forward, toDirection);
        float stepSize = angle / timeToMove * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(toDirection), stepSize);    
    }
    #endregion

} 