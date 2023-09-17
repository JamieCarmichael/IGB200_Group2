using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Allows an object to be pciked up by the player when they interact with it.
/// </summary>
public class Pickup : MonoBehaviour, IIntertactable
{
    #region Fields
    [Serializable]
    public struct ItemDetails
    {
        public string itemName;
        public int numberOfItems;
        public Sprite itemImage;
    }

    private Vector3 startPos;

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
            if (PlayerManager.Instance.PlayerInteract.CanUseItemType(itemIdentifier))
            {
                return intertactable;
            }
            else
            {
                return false;
            }

        }
    }
    private bool intertactable = true;

    [Tooltip("The object that indicates when this object is being looked at.")]
    [SerializeField] private GameObject lookAtObject;

    private Collider thisCollider;

    private bool isHeld = false;

    public bool IsHeld { get { return isHeld; } }

    [SerializeField] private string itemName;

    public string Item { get { return itemName; } }

    [SerializeField] private string itemIdentifier = "default";

    public string ItemIdentifier { get { return itemIdentifier; } }
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = GetComponent<Collider>();
        startPos = transform.position;
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

    public void ReturnToStart()
    {
        transform.position = startPos;
        gameObject.SetActive(true);
    }
    #endregion

    #region Private Methods

    #endregion

    #region IIntertactable
    public void Interact(string item)
    {
        if (PlayerManager.Instance.PlayerInteract.HeldObject != null)
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