using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Attached to a collider that sits on the corener of a surface. Allows the player to know this is an edge that can be grabed or stepped over.
/// </summary>
public class Edge : MonoBehaviour 
{
    #region Fields
    [SerializeField] private bool canClimb = true;
    /// <summary>
    /// The collider attached to this edge.
    /// </summary>
    private Collider thisCollider;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = GetComponent<Collider>();
    }
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
    #endregion
}