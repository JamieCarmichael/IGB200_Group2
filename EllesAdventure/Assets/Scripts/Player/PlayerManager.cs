using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Holds data for the player that needs to be accessable by other objects.
///             Players inventroy is held on this script.
/// </summary>
public class PlayerManager : MonoBehaviour 
{
    #region Fields
    public static PlayerManager Instance { get; private set; }

    [Tooltip("The players tarnsform. ")]
    [SerializeField] private Transform playerTransform;

    /// <summary>
    /// The players tarnsform.
    /// </summary>
    public Transform PlayerTransform
    {
        get { return playerTransform; }
    }

    /// <summary>
    /// The Dictionary containing the players inventory.
    /// </summary>
    private Dictionary<string, int> inventoryDictionary = new Dictionary<string, int>();

    private PlayerMovement playerMovement;
    private PlayerInteract playerInteract;
    private PlayerMovementLadder playerMovementLadder;
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
        playerMovement = GetComponentInChildren<PlayerMovement>();
        playerInteract = GetComponentInChildren<PlayerInteract>();
        playerMovementLadder = GetComponentInChildren<PlayerMovementLadder>();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Adds an item to the inventory. Adds 1 of the item.
    /// </summary>
    /// <param name="name">The item being added.</param>
    public void AddToInventory(string name)
    {
        if (!inventoryDictionary.TryAdd(name, 1))
        {
            inventoryDictionary[name] += 1;
        }
    }
    /// <summary>
    /// Use an item from the players inventroy.
    /// </summary>
    /// <param name="name">The name of the item</param>
    /// <param name="number">How many of the item are being used.</param>
    /// <param name="removeObject">If true the item is removed from the inventory when used.</param>
    /// <returns>True if the item was successfully used.</returns>
    public bool UseItem(string name, int number, bool removeObject)
    {
        if (inventoryDictionary.ContainsKey(name) && inventoryDictionary[name] >= number)
        {
            if (removeObject)
            {
                inventoryDictionary[name] -= number;
                if (inventoryDictionary[name] <= 0)
                {
                    inventoryDictionary.Remove(name);
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// The dictionary of objects contained within the inventory.
    /// </summary>
    public Dictionary<string, int> InventoryDictionary
    {
        get { return inventoryDictionary; }
    }

    /// <summary>
    /// Sets the current player movement to use a ladder.
    /// </summary>
    /// <param name="ladder">The ladder to be used.</param>
    public void ClimbLadder(Ladder ladder)
    {
        playerMovementLadder.enabled = true;
        playerMovement.enabled = false;
        playerInteract.enabled = false;
        playerMovementLadder.AttachToLadder(ladder);
    }

    /// <summary>
    /// Sets up the current player movement to be normal movement.
    /// </summary>
    public void StandardMovement()
    {
        playerMovementLadder.enabled = false;
        playerMovement.enabled = true;
        playerInteract.enabled = true;
    }
    #endregion
}