using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A scriptable object that contains information on a single item in the inventroy.
/// </summary>


[CreateAssetMenu(fileName = "NewSOInventoryItem", menuName = "ScriptableObjects/SOInventoryItem")]
public class SOInventoryItem : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
}