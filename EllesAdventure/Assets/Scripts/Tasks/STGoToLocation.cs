using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

[Serializable]
public class STGoToLocation : SubTask
{
    #region Fields
    public bool canSee;
    #endregion

    public STGoToLocation(Task theTask)
    {
        taskName = "2";
        task = theTask;
    }

    #region Public Methods

    public override bool DoSubtask()
    {
        throw new NotImplementedException();
    }

    public override void StartTask()
    {
        throw new NotImplementedException();
    }

    public override void StopTask()
    {
        throw new NotImplementedException();
    }
    #endregion
}