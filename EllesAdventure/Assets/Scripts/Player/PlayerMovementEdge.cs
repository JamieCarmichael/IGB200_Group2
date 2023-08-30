using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Attached to a collider that sits on the corener of a surface. Allows the player to know this is an edge that can be grabed or stepped over.
/// </summary>
public class PlayerMovementEdge : MonoBehaviour 
{
    #region Fields
    [SerializeField] private float overTop = 0.05f;
    [SerializeField] private float downFromTop = 0.3f;


    [SerializeField] private float checkDistance = 0.5f;

    [SerializeField] private LayerMask climbLayer;

    [SerializeField] private float hangDistance = 0.2f;

    [SerializeField] private float maxHangTime = 5.0f;

    [SerializeField] private float hangCooldown = 3.0f;

    private float hangTimer = 0.0f;

    CharacterController characterController;
    private Bounds playerBounds;

    public bool onEdge = false;

    private Vector3 localPlayerFrontTopCentre 
    { 
        get
        {
            playerBounds = characterController.bounds;
            return new Vector3(playerBounds.center.x, playerBounds.max.y, playerBounds.center.z);
        }
    }

    #endregion


    #region Unity Call Methods
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
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

        if (InputManager.Instance.PlayerInput.InGame.Jump.IsPressed() || hangTimer > maxHangTime)
        {
            onEdge = false;
            hangTimer = 0.0f;
            PlayerManager.Instance.StandardMovement();

        }

    }
    #endregion

    #region Private Method
    private void CheckForEdge()
    {
        Vector3 topOfHeadPos = localPlayerFrontTopCentre;

        Debug.DrawRay(topOfHeadPos + (Vector3.up * overTop), transform.forward * (checkDistance + characterController.radius), Color.red, 1.0f);
        Debug.DrawRay(topOfHeadPos - (Vector3.up * downFromTop), transform.forward * (checkDistance + characterController.radius), Color.green, 1.0f);

        Vector3 lowerPos = topOfHeadPos - (Vector3.up * downFromTop);
        Vector3 upperPos = topOfHeadPos + (Vector3.up * overTop);

        Physics.Raycast(lowerPos, transform.forward, out RaycastHit lowerHit, checkDistance + characterController.radius, climbLayer, QueryTriggerInteraction.Ignore);
        Physics.Raycast(upperPos, transform.forward, out RaycastHit upperHit, checkDistance + characterController.radius, climbLayer, QueryTriggerInteraction.Ignore);

        if (lowerHit.collider != null && upperHit.collider == null)
        {
            Physics.Raycast(lowerHit.point + (Vector3.up * (downFromTop + overTop)), -transform.up, out RaycastHit cornerHit, downFromTop + overTop, climbLayer, QueryTriggerInteraction.Ignore);
            // corner Position
            Debug.DrawRay(cornerHit.point, transform.up, Color.blue, 1.0f);

            onEdge = true;
            hangTimer = 0.0f;

            Vector3 hangPos = cornerHit.point;
            hangPos.y -= characterController.height - hangDistance;
            hangPos -= transform.forward * characterController.radius;

            transform.position = hangPos;
            PlayerManager.Instance.OnEdge();
        }
    }
    #endregion

    #region old
    //[SerializeField] private bool canClimb = true;
    /// <summary>
    /// The collider attached to this edge.
    /// </summary>
    //private Collider thisCollider;
    //private void Start()
    //{
    //    thisCollider = GetComponent<Collider>();
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    // Tell the player they are touching this edge.
    //    if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
    //    {
    //        Vector3 collisionPoint = thisCollider.ClosestPoint(other.bounds.center);

    //        playerMovement.EnterEdge(collisionPoint, gameObject, canClimb);
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    // Let the player know that they are no longer on this edge.
    //    if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
    //    {
    //        playerMovement.ExitEdge();
    //    }
    //}


    #region Edges
    ////[Header("Edges")]
    ///// <summary>
    ///// The player is touching an edge.
    ///// </summary>
    //private bool onEdge = false;
    ///// <summary>
    ///// The point that the player is touching the edge.
    ///// </summary>
    //private Vector3 edgeTouchPoint;
    ///// <summary>
    ///// The edge object being touched.
    ///// </summary>
    //private GameObject edgeObject;
    ///// <summary>
    ///// The player is currently hanging from an edge.
    ///// </summary>
    //private bool isHanging = false;
    ///// <summary>
    ///// The current edge can be climbed.
    ///// </summary>
    //private bool canClimbEdge = false;

    //private void OnEdge(Vector3 moveInputVector3)
    //{
    //    Vector2 moveInput = InputManager.Instance.PlayerInput.InGame.Move.ReadValue<Vector2>();

    //    Vector3 thisForward = transform.forward;
    //    Vector3 edgeForawrd = edgeObject.transform.forward;

    //    if (!canClimbEdge)
    //    {
    //        movementVector = moveInputVector3 * speed;
    //        return;
    //    }

    //    // Hit with feet
    //    if (GetComponent<Collider>().bounds.center.y > edgeTouchPoint.y)
    //    {
    //        if (verticalVelocity > 0.0f)
    //        {
    //            verticalVelocity = 0.0f;
    //        }
    //        movementVector = moveInputVector3 * speed;
    //    }
    //    // Hit with upper body.
    //    else
    //    {
    //        verticalVelocity = 0.0f;
    //        if (Vector3.Dot(thisForward, edgeForawrd) > 0)
    //        {
    //            if (moveInput.y > 0)
    //            {
    //                // Hanging climb up button pressed.
    //                Vector3 extents = edgeObject.GetComponent<Collider>().bounds.extents;
    //                Vector3 climbPos = edgeTouchPoint + (edgeObject.transform.up * extents.y) + (edgeObject.transform.forward * extents.z) ;
    //                StartCoroutine(ClimbTo(climbPos));

    //                isHanging = false;
    //            }
    //            else if (moveInput.y < 0)
    //            {
    //                // Hanging drop down button pressed.
    //                movementVector = moveInputVector3 * speed;
    //                isHanging = false;
    //            }
    //            else
    //            {
    //                // Hanging no input.
    //                characterController.enabled = false;
    //                transform.position = edgeTouchPoint + Vector3.down;
    //                movementVector = Vector3.zero;
    //                characterController.enabled = true;

    //                isHanging = true;
    //            }
    //        }
    //        else
    //        {
    //            // Moving away from edge
    //            isHanging = false;
    //        }
    //    }
    //}

    ///// <summary>
    ///// Called when the player enters an edge collider. 
    ///// </summary>
    ///// <param name="newCollisionPoint"></param>
    ///// <param name="other"></param>
    //public void EnterEdge(Vector3 newCollisionPoint, GameObject other, bool canClimb)
    //{
    //    onEdge = true;
    //    edgeObject = other;
    //    edgeTouchPoint = newCollisionPoint;
    //    canClimbEdge = canClimb;
    //}
    ///// <summary>
    ///// Called when the player exits a Edge Collider.
    ///// </summary>
    //public void ExitEdge()
    //{
    //    onEdge = false;
    //}

    ///// <summary>
    ///// Moves the player to the top of the ledge they are climbing.
    ///// </summary>
    ///// <param name="newPosition">The position at the top of the ledge.</param>
    ///// <returns></returns>
    //private IEnumerator ClimbTo(Vector3 newPosition)
    //{
    //    canMove = false;
    //    Vector3 toVector = newPosition - transform.position;
    //    float timeToMove = toVector.magnitude / maxWalkSpeed;

    //    float moveTimer = 0.0f;
    //    Vector3 movePostion = Vector3.zero;

    //    animator.SetBool(animWalk, true);
    //    // Climb up ledge.
    //    while (moveTimer < timeToMove)
    //    {
    //        moveTimer += Time.deltaTime;
    //        movePostion = toVector.normalized * maxWalkSpeed * Time.deltaTime;
    //        characterController.Move(movePostion);
    //        yield return null;
    //    }

    //    // Step forward from top of ledge.
    //    toVector = transform.forward;
    //    timeToMove = (characterController.radius * 2) / maxWalkSpeed;
    //    moveTimer = 0.0f;
    //    while (moveTimer < timeToMove)
    //    {
    //        moveTimer += Time.deltaTime;
    //        movePostion = toVector.normalized * maxWalkSpeed * Time.deltaTime;
    //        characterController.Move(movePostion);
    //        yield return null;
    //    }
    //    movementVector = Vector3.zero;
    //    animator.SetBool(animWalk, false);
    //    canMove = true;
    //}
    #endregion
    #endregion
}