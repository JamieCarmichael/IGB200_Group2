using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A ladder object that the player can climb on.
/// </summary>
public class Ladder : MonoBehaviour
{
    #region Fields
    [Tooltip("A value from -1 to 1; 1 is facing towards the same direction as the ladder. -1 is facing the opposit direction. 0 is parallel.")]
    [SerializeField] private float facingDot = 0.6f;
    [Tooltip("A value from -1 to 1; 1 is facing towards the same direction as the ladder. -1 is facing the opposit direction. 0 is parallel.")]
    [SerializeField] private float headingDot = 0.6f;

    [SerializeField] private Transform bottomOfLadder;
    public Transform BottomOfLadder { get { return bottomOfLadder; } }

    [SerializeField] private Transform topOfLadder;
    public Transform TopOfLadder { get { return topOfLadder; } }

    /// <summary>
    /// The collider for this object.
    /// </summary>
    private Collider thisCollider;
    /// <summary>
    /// The collider for this object.
    /// </summary>
    public Collider ThisCollider { get { return thisCollider; } } 
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        // Not the player
        if (other.tag != "Player")
        {
            return;
        }
        // Find the direction the ladder is facing. 
        Vector3 ladderFacing = transform.forward;
        // If the player is behind the ladder (At the top of the ladder facing towards it) then flip the direction.
        bool behindLadder = Vector3.Dot(transform.position - other.transform.position, transform.forward) < 0;
        if (behindLadder)
        {
            ladderFacing = -transform.forward;
        }

        // Not facing in the direction of the ladder
        float directionFacingDot = Vector3.Dot(ladderFacing, other.transform.forward);
        if (directionFacingDot < facingDot)
        {
            return;
        }
        // Not moving towards the ladder.
        float directionHeadingDot = Vector3.Dot(ladderFacing, PlayerManager.Instance.PlayerMovement.MovementVector);
        if (directionHeadingDot < headingDot)
        {
            return;
        }

        // Interact with the ladder.
        PlayerManager.Instance.ClimbLadder(this);
    }
    #endregion
}