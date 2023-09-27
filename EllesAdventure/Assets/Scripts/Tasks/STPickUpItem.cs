using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

[Serializable]
public class STPickUpItem : SubTask
{
    #region Fields

    #endregion

    public STPickUpItem(Task theTask)
    {
        taskName = "3";
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