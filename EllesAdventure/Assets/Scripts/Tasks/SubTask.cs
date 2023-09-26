using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

[Serializable]
public class SubTask
{
    public enum SubtaskType
    {
        TalkToNPC,
        GoToLocation,
        PickUpItem,
        DeliverItem,
        DoOtherTask
    }

    #region Fields
    [SerializeField] protected string taskName;

    [SerializeField] protected UnityEvent onEndEvent;

    #endregion

    #region Public Methods

    public virtual string GetName()
    {
        return taskName;
    }

    public virtual bool TryComplete()
    {
        return false;
    }

    public virtual void StartTask()
    {

    }

    public virtual void StopTask()
    {

    }
    #endregion
}