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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            Vector3 collisionPoint = thisCollider.ClosestPoint(other.bounds.center);

            playerMovement.HitEdge(collisionPoint, gameObject);
        }

    }
    #endregion
}