using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Holds data for the player that needs to be accessable by other objects.
/// </summary>
public class PlayerManager : MonoBehaviour 
{
    #region Fields
    public static PlayerManager Instance { get; private set; }

    [Tooltip("The players tarnsform. ")]
    [SerializeField] private Transform playerTransform;

    /// <summary>
    /// The inventroy of items the player has.
    /// </summary>
    private List<GameObject> inventory = new List<GameObject>();

    /// <summary>
    /// The players tarnsform.
    /// </summary>
    public Transform PlayerTransform
    {
        get { return playerTransform; }
    }
    #endregion

    #region Unity Call Functions
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    #endregion

    #region Public Methods
    public void AddToInventory(GameObject newObject)
    {
        inventory.Add(newObject);
    }

    /// <summary>
    /// Try to use an item from the inventory. If it is used return true.
    /// </summary>
    /// <param name="itemToUse">The item to use.</param>
    /// <param name="removeObject">If true the item is removed from inventroy.</param>
    /// <returns>The item is used.</returns>
    public bool UseItem(GameObject itemToUse, bool removeObject)
    {
        if (inventory.Contains(itemToUse))
        {
            if (removeObject)
            {
                inventory.Remove(itemToUse);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Private Methods

    #endregion

}