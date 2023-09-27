using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

[Serializable]
public class STDeliverItemToNPC : SubTask
{
    #region Fields

    #endregion

    public STDeliverItemToNPC(Task theTask)
    {
        taskName = "4";
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