using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

[Serializable]
public class FindTask : ITask
{
    [Serializable]
    public struct Item
    {
        public string itemName;
        public int numberOfItems;
    }

    #region Fields
    [SerializeField] private Item item;

    [SerializeField] private string name;

    [SerializeField] private string description;

    private bool isComplete;
    #endregion

    #region Properties
    public override bool IsComplete
    {
        get
        {
            return isComplete;
        }
    }

    public override string Name
    {
        get { return name; }
    }

    public override string Description
    {
        get
        {
            return description;
        }
    }
    #endregion

    #region Public Methods
    public override bool TryComplete()
    {
        if (IsComplete)
        {
            return IsComplete;
        }

        isComplete = PlayerManager.Instance.UseItem(item.itemName, item.numberOfItems, true);

        return IsComplete;
    }
    #endregion

}