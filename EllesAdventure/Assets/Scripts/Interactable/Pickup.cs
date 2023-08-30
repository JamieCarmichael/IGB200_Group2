using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Allows an object to be pciked up by the player when they interact with it.
/// </summary>
public class Pickup : MonoBehaviour, IIntertactable
{
    #region Fields
    public string InteractAminationString
    {
        get
        {
            return interactAminationString;
        }
    }

    [SerializeField] private string interactAminationString;

    public bool Intertactable
    {
        get
        {
            return intertactable;
        }
    }
    private bool intertactable = true;

    [Tooltip("The object that indicates when this object is being looked at.")]
    [SerializeField] private GameObject lookAtObject;
    [Tooltip("The items name when added to the players inventroy.")]
    [SerializeField] private InventoryObject item;

    private Collider thisCollider;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = GetComponent<Collider>();
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods

    #endregion

    #region IIntertactable


    public void Interact()
    {
        PlayerManager.Instance.playerInventory.AddToInventory(item);

        gameObject.SetActive(false);
    }

    public void StartLookAt()
    {
        lookAtObject.SetActive(true);
    }

    public void StopLookAt()
    {
        lookAtObject.SetActive(false);
    }

    public Vector3 GetClosestPoint(Vector3 playerPos)
    {
        Vector3 closestPoint = thisCollider.ClosestPoint(playerPos);
        closestPoint.y = transform.position.y;
        return closestPoint;
    }
    #endregion
}