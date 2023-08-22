using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

[CreateAssetMenu(fileName = "NewSOFindTask", menuName = "ScriptableObjects/SOFindTask")]
public class SOFindTask : SOTask
{
    [Serializable]
    public struct Item
    {
        public string itemName;
        public int numberOfItems;
    }

    #region Fields
    [SerializeField] private Item item;

    [SerializeField] private string taskName;

    [SerializeField] private string description;

    public bool isComplete = false;
    #endregion

    #region Properties
    public override bool IsComplete
    {
        get
        {
            return isComplete;
        }
    }

    public override string TaskName
    {
        get { return taskName; }
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

    public override void StartTask()
    {
        isComplete = false;
    }
    #endregion

}