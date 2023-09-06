using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A switch that interacts with another object.
/// </summary>
public class Lever : MonoBehaviour, IIntertactable
{
    #region Fields
    private Collider thisCollider;

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

    /// <summary>
    /// If true this object is interaclable.
    /// </summary>
    private bool intertactable = true;

    [Tooltip("The object that indicates when this object is being looked at.")]
    private GameObject lookAtObject;

    [Tooltip("How long the lever takes to opperate.")]
    [SerializeField] private float openTime = 1.0f;
    [Tooltip("The name of the item requeired to use the lever. If empty no item is required.")]
    [SerializeField] private InventoryObject requiredItem;
    [Tooltip("The number of the item needed to use the level.")]
    [SerializeField] private int numberOfItmesNeeded;
    [Tooltip("If true the required item will be ussd and no longer be in the inventroy.")]
    [SerializeField] private bool useRequiredItem = false;
    [Tooltip("The event called after the lever had been used.")]
    [SerializeField] private UnityEvent afterLeverEvent;

    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = gameObject.GetComponent<Collider>();
    }
    #endregion

    #region IIntertactable
    public void Interact()
    {
        if (numberOfItmesNeeded != 0)
        {
            if (!PlayerManager.Instance.PlayerInventory.UseItem(requiredItem, useRequiredItem))
            {
                return;
            }
        }

        gameObject.GetComponent<Animator>().SetTrigger("Open");
        Invoke("AfterLever", openTime);
    }

    private void AfterLever()
    {
        afterLeverEvent.Invoke();
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
        Vector3 closestPoint = thisCollider.ClosestPoint(playerPos);
        closestPoint.y = transform.position.y;
        return closestPoint;
    }
    #endregion
}