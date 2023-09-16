using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A ladder object that the player can climb on.
/// </summary>
public class Ladder : MonoBehaviour, IIntertactable
{
    #region Fields
    [Tooltip("The position that the player goes to to start climbing the laddder.")]
    [SerializeField] private Transform climbOnPosition;

    /// <summary>
    /// The collider for this object.
    /// </summary>
    private Collider thisCollider;
    /// <summary>
    /// The collider for this object.
    /// </summary>
    public Collider ThisCollider { get { return thisCollider; } } 
    public string InteractAminationString
    {
        get
        {
            return interactAminationString;
        }
    }

    [Tooltip("The string for the trigger to run the animation for this interaction.")]
    [SerializeField] private string interactAminationString;

    public bool Intertactable
    {
        get
        {
            return intertactable;
        }
    }
    [Tooltip("If true this object can be interacted with.")]
    [SerializeField] private bool intertactable = false;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = GetComponent<Collider>();
    }
    #endregion

    #region IIntertactable
    public void Interact(string item)
    {
        PlayerManager.Instance.ClimbLadder(this);
    }

    public void StartLookAt()
    {
        //lookAtObject.SetActive(true);
    }

    public void StopLookAt()
    {
        //lookAtObject.SetActive(false);
    }

    public Vector3 GetClosestPoint(Vector3 playerPos)
    {
        //Bounds bounds = thisCollider.bounds;

        //Vector3 center = bounds.center;
        //Vector3 extents = bounds.extents;

        //extents = transform.InverseTransformPoint(extents);


        //Vector3 bottom = thisCollider.ClosestPoint(center + new Vector3(0, -extents.y, -extents.z));

        //return bottom;

        //Vector3 top = thisCollider.ClosestPoint(center + new Vector3(0, extents.y, -extents.z));
        //Vector3 upDirection = (top - bottom).normalized;
        //float height = bounds.size.y;

        //float percentUp = (playerPos.y -bottom.y) / height;

        //Vector3 vectorUp = upDirection * percentUp;

        //Vector3 closestPoint = vectorUp + bottom;

        //return closestPoint;

        return climbOnPosition.position;
    }
    #endregion
}