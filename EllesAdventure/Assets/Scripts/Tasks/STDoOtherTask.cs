using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

[Serializable]
public class STDoOtherTask : SubTask
{
    #region Fields

    #endregion

    public STDoOtherTask()
    {
        taskName = "5";
    }

    #region Public Methods

    public override bool TryComplete()
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