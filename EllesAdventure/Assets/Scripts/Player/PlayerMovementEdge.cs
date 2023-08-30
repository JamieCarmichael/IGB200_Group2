using System.Collections;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Detects edges and handle movement while hanging from an edge.
/// </summary>
public class PlayerMovementEdge : MonoBehaviour 
{
    #region Fields
    [Tooltip("The distance from the player that they check for an edge to grab.")]
    [SerializeField] private float checkDistance = 0.5f;
    [Tooltip("How far above the players head the check for no wall will happen.")]
    [SerializeField] private float overTop = 0.05f;
    [Tooltip("How far below the top of the players head the check for a wall will happen.")]
    [SerializeField] private float downFromTop = 0.3f;
    [Tooltip("The layer mask checked for climbable objects.")]
    [SerializeField] private LayerMask climbLayer;
    [Tooltip("When hanging from an edge the top of the character controler will be this far above the corner.")]
    [SerializeField] private float hangDistance = 0.2f;
    [Tooltip("How long the player can hang for before falling.")]
    [SerializeField] private float maxHangTime = 5.0f;
    [Tooltip("How long after finishing hanging before the player can grab another edge.")]
    [SerializeField] private float hangCooldown = 3.0f;
    [Tooltip("How many meters per second the player will climb up the edge.")]
    [SerializeField] private float climbSpeed = 2.0f;

    /// <summary>
    /// The players chartacter controller.
    /// </summary>
    private CharacterController characterController;
    /// <summary>
    /// The bounds of the players character controller.
    /// </summary>
    private Bounds playerBounds;
    /// <summary>
    /// If true the player is currently attached to an edge.
    /// </summary>
    private bool onEdge = false;
    /// <summary>
    /// If true the player is currenly climbing the edge.
    /// </summary>
    private bool isClimbing = false;
    /// <summary>
    /// A timer for how ling the player has been hanging for.
    /// </summary>
    private float hangTimer = 0.0f;
    /// <summary>
    /// Thhe position that the player will move to when hanging.
    /// </summary>
    private Vector3 hangPos;
    /// <summary>
    /// The position the player will climb to when the climb up the edge.
    /// </summary>
    private Vector3 climbPos;

    [Header("Animation")]
    [Tooltip("The animator used for the player model.")]
    [SerializeField] private string animHang = "";
    [SerializeField] private string animClimb = "";
    /// <summary>
    /// The players animator.
    /// </summary>
    private Animator animator;
    #endregion


    #region Unity Call Methods
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (isClimbing)
        {
            return;
        }

        if (!onEdge && hangTimer < hangCooldown)
        {
            hangTimer += Time.deltaTime;
            return;
        }

        if (!onEdge)
        {
            CheckForEdge();

            return;
        }

        hangTimer += Time.deltaTime;

        // Fall of wall.
        if (InputManager.Instance.PlayerInput.InGame.Jump.WasPressedThisFrame() || hangTimer > maxHangTime)
        {
            onEdge = false;
            hangTimer = 0.0f;
            PlayerManager.Instance.StandardMovement();

            animator.SetBool(animHang, false);
        }
        // Climb wall.
        else if (InputManager.Instance.PlayerInput.InGame.Move.WasPressedThisFrame())
        {
            StartCoroutine(ClimbOverEdge());
        }

    }
    #endregion

    #region Private Method
    /// <summary>
    /// Check to see if there is an edge to climb and then hang from it.
    /// </summary>
    private void CheckForEdge()
    {
        // Find the front centre top of the player.
        playerBounds = characterController.bounds;       
        Vector3 topOfHeadPos = new Vector3(playerBounds.center.x, playerBounds.max.y, playerBounds.center.z);

        Vector3 lowerPos = topOfHeadPos - (Vector3.up * downFromTop);
        Vector3 upperPos = topOfHeadPos + (Vector3.up * overTop);

        // Check if there is no wall above the player but their is one in front.
        Physics.Raycast(lowerPos, transform.forward, out RaycastHit lowerHit, checkDistance + characterController.radius, climbLayer, QueryTriggerInteraction.Ignore);
        Physics.Raycast(upperPos, transform.forward, out RaycastHit upperHit, checkDistance + characterController.radius, climbLayer, QueryTriggerInteraction.Ignore);

        if (lowerHit.collider != null && upperHit.collider == null && !PlayerManager.Instance.PlayerMovement.IsGrounded)
        {
            // Find the corner of the wall.
            Physics.Raycast(lowerHit.point + (Vector3.up * (downFromTop + overTop)), -transform.up, out RaycastHit cornerHit, downFromTop + overTop, climbLayer, QueryTriggerInteraction.Ignore);

            onEdge = true;
            hangTimer = 0.0f;

            climbPos = cornerHit.point;
            climbPos += transform.forward * characterController.radius;

            hangPos = cornerHit.point;
            hangPos.y -= characterController.height - hangDistance;
            hangPos -= transform.forward * (characterController.radius + 0.1f);

            StartCoroutine(MoveToEdge());

            PlayerManager.Instance.OnEdge();

            animator.SetBool(animHang, true);
        }
    }

    /// <summary>
    /// Move the player up the edge and then forward.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ClimbOverEdge()
    {
        isClimbing = true;

        Vector3 movePostion = Vector3.zero;
        animator.SetBool(animClimb, true);

        float moveTimer = 0.0f;
        Vector3 toVector = climbPos - transform.position;
        float climbDistance = toVector.magnitude;
        float timeToClimb = climbDistance/climbSpeed;

        // Climb up ledge.
        while (moveTimer < timeToClimb)
        {
            moveTimer += Time.deltaTime;
            movePostion = toVector.normalized * (climbDistance / timeToClimb) * Time.deltaTime;
            characterController.Move(movePostion);
            yield return null;
        }

        // Move forwad.
        moveTimer = 0.0f;
        toVector = climbPos - transform.position;
        climbDistance = toVector.magnitude;
        timeToClimb = climbDistance / climbSpeed;
        while (moveTimer < timeToClimb)
        {
            moveTimer += Time.deltaTime;
            movePostion = toVector.normalized * climbDistance / timeToClimb * Time.deltaTime;
            characterController.Move(movePostion);
            yield return null;
        }
        animator.SetBool(animClimb, false);
        isClimbing = false;

        onEdge = false;
        hangTimer = 0.0f;
        PlayerManager.Instance.StandardMovement();

    }

    /// <summary>
    /// Move the player to the edge to hang.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveToEdge()
    {
        isClimbing = true;
        Vector3 movePostion = Vector3.zero;

        float moveTimer = 0.0f;
        float climbDistance = (transform.position - hangPos).magnitude;
        float timeToClimb = climbDistance / climbSpeed;
        Vector3 toVector = hangPos - transform.position;

        // Move to edge.
        while (moveTimer < timeToClimb)
        {
            moveTimer += Time.deltaTime;
            movePostion = toVector.normalized * (climbDistance / timeToClimb) * Time.deltaTime;
            characterController.Move(movePostion);
            yield return null;
        }
        isClimbing = false;
    }
    #endregion
}