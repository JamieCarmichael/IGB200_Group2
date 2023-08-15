using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>
public class Edge : MonoBehaviour 
{
    #region Fields

    private Collider thisCollider;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            Vector3 collisionPoint = thisCollider.ClosestPoint(other.bounds.center);

            playerMovement.EnterEdge(collisionPoint, gameObject);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
    //    {
    //        Vector3 collisionPoint = thisCollider.ClosestPoint(other.bounds.center);

    //        playerMovement.EnterEdge(collisionPoint, gameObject);
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            Vector3 collisionPoint = thisCollider.ClosestPoint(other.bounds.center);

            playerMovement.ExitEdge();
        }
    }
    #endregion
}