using UnityEngine;
/// <summary>
/// Made By: Jamie Carmichael
/// Details: Interface for interactable objects.
/// </summary>
public interface IIntertactable
{
    /// <summary>
    /// The string for the animation trigger for this interation.
    /// </summary>
    public string InteractAminationString { get; }

    /// <summary>
    /// The identification string for the item required to interact with this.
    /// </summary>
    public string RequiredItem {  get; }

    /// <summary>
    /// If true this object can be interacted with.
    /// </summary>
    public bool Intertactable { get; }

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

    /// <summary>
    /// The position that the player will move to when interacting with this.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetClosestPoint(Vector3 playerPos);

}
