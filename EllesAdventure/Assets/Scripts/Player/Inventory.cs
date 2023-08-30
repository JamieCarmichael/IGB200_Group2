using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: The players inventory
/// </summary>
public class Inventory : MonoBehaviour 
{
    #region Fields
    private List<InventoryObject> inventoryObjects = new List<InventoryObject>();
    #endregion

    #region Public Methods
    /// <summary>
    /// Adds an item to the inventory. Adds 1 of the item.
    /// </summary>
    /// <param name="newItem"></param>
    public void AddToInventory(InventoryObject newItem)
    {
        for (int i = 0; i < inventoryObjects.Count; i++)
        {
            if (newItem.item.itemName == inventoryObjects[i].item.itemName)
            {
                inventoryObjects[i].count += newItem.count;
                return;
            }
        }
        inventoryObjects.Add(newItem);
    }

    /// <summary>
    /// Use an item from the players inventroy.
    /// </summary>
    /// <param name="itemUsed"></param>
    /// <param name="removeObject"></param>
    /// <returns></returns>
    public bool UseItem(InventoryObject itemUsed, bool removeObject)
    {
        if (itemUsed.item == null)
        {
            Debug.LogWarning("Tried to use Null Item!");
            return false;
        }

        // Check list
        for (int i = 0; i < inventoryObjects.Count; i++)
        {
            if (itemUsed.item.itemName == inventoryObjects[i].item.itemName)
            {
                // Item found
                if (inventoryObjects[i].count >= itemUsed.count)
                {
                    // Have enought items
                    if (removeObject)
                    {
                        // Remove items from list
                        inventoryObjects[i].count -= itemUsed.count;
                        if (inventoryObjects[i].count <= 0)
                        {
                            inventoryObjects.RemoveAt(i);
                        }
                    }
                    // Confirm use
                    return true;
                }
                // Not enought items
                return false;
            }
        }
        // Did not find item.
        return false;
    }

    /// <summary>
    /// The list of objects contained within the inventory.
    /// </summary>
    public List<InventoryObject> InventoryList
    {
        get { return inventoryObjects; }
    }

    #endregion
}

