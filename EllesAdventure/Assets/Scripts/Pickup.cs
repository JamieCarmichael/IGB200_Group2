using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Allows an object to be pciked up by the player when they interact with it.
/// </summary>
public class Pickup : MonoBehaviour, IIntertactable
{
    #region Fields
    public Collider ThisCollider { get; private set; }
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
        Debug.Log(gameObject.name + " has been interacted with.");    
    }

    public void StartLookAt()
    {
        Debug.Log(gameObject.name + " start look at.");
    }

    public void StopLookAt()
    {
        Debug.Log(gameObject.name + " stop look at.");
    }
    #endregion
}