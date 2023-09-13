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

    private Collider thisCollider;

    private bool isHeld = false;

    public bool IsHeld { get { return isHeld; } }

    [SerializeField] private UsableItems.Item item;

    public UsableItems.Item Item { get { return item; } }

    private UsableItems.Item requiredItem = UsableItems.Item.None;
    public UsableItems.Item RequiredItem { get { return requiredItem; } }
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = GetComponent<Collider>();
    }
    #endregion

    #region Public Methods
    public void PutDown(Vector3 newPos)
    {
        isHeld = false;
        transform.parent = null;
        transform.position = newPos;
    }

    public void Use()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region Private Methods

    #endregion

    #region IIntertactable
    public void Interact(UsableItems.Item item)
    {
        if (PlayerManager.Instance.PlayerInteract.HeldItem != UsableItems.Item.None)
        {
            return;
        }

        isHeld = true;
        transform.parent = PlayerManager.Instance.ItemCarryTransform;
        transform.position = transform.parent.position;
        lookAtObject.SetActive(false);
    }

    public void StartLookAt()
    {
        if (!isHeld)
        {
            lookAtObject.SetActive(true);
        }
        else
        {
            lookAtObject.SetActive(false);
        }
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