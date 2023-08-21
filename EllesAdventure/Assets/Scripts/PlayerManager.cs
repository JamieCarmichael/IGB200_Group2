using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;

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

    private Dictionary<string, int> inventoryDictionary = new Dictionary<string, int>();
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

    public void AddToInventory(string name)
    {
        if (!inventoryDictionary.TryAdd(name, 1))
        {
            inventoryDictionary[name] += 1;
        }
        PrintInventory();
    }
    public bool UseItem(string name, int number, bool removeObject)
    {
        if (inventoryDictionary.ContainsKey(name) && inventoryDictionary[name] >= number)
        {
            if (removeObject)
            {
                inventoryDictionary[name] -= number;
            }
            PrintInventory();
            return true;
        }
        else
        {
            PrintInventory();
            return false;
        }
    }

    public void PrintInventory()
    {
        for (int i = 0; i < inventoryDictionary.Count; i++)
        {
            int a = inventoryDictionary.ElementAt(i).Value;
            string b = inventoryDictionary.ElementAt(i).Key;

            Debug.Log($"{b} : {a}");
        }
    }
    #endregion

    #region Private Methods

    #endregion

}