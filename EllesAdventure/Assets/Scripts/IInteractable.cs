using UnityEngine;
/// <summary>
/// Made By: Jamie Carmichael
/// Details: Interface for interactable objects.
/// </summary>
public interface IIntertactable
{
    /// <summary>
    /// The collider for the interactable object.
    /// </summary>
    public Collider ThisCollider { get;}

    /// <summary>
    /// To be called when interacting with an object.
    /// </summary>
    public void Interact();
    /// <summary>
    /// Called when the player starts looking at this object.
    /// </summary>
    public void StartLookAt();
    /// <summary>
    /// Called when the player stops looking at this object.
    /// </summary>
    public void StopLookAt();
}
