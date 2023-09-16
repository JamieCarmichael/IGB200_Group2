using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A task that is a collection task. The player will need to return a number of an item to the NPC.
/// </summary>

[CreateAssetMenu(fileName = "NewSOFindTask", menuName = "ScriptableObjects/SOFindTask")]
public class SOFindTask : SOTask
{
    #region Fields
    [Tooltip("The item that is needed. The name of the item needed.")]
    [SerializeField] private string item;
    [Tooltip("The name of the task being done. This is displayed in the notepad and should be descriptive.")]
    [SerializeField] private string taskName;

    /// <summary>
    /// Has this task been completed.
    /// </summary>
    private bool isComplete = false;
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
    #endregion

    #region Public Methods
    public override bool TryComplete()
    {
        if (isComplete)
        {
            return isComplete;
        }

        isComplete = PlayerManager.Instance.PlayerInteract.HeldItem == item;
        if (isComplete)
        {
            PlayerManager.Instance.PlayerInteract.RemoveHeldObject();
        }

        return isComplete;
    }

    public override void StartTask()
    {
        isComplete = false;
    }
    #endregion

}