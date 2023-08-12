using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Allows an object to be pciked up by the player when they interact with it.
/// </summary>
public class Pickup : MonoBehaviour, IIntertactable
{
    #region Fields
    public Collider ThisCollider { get; private set; }
    [Tooltip("The object that indicates when this object is being looked at.")]
    [SerializeField] private GameObject lookAtObject;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        ThisCollider = gameObject.GetComponent<Collider>();
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods

    #endregion

    #region IIntertactable
    public void Interact()
    {
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
    #endregion
}