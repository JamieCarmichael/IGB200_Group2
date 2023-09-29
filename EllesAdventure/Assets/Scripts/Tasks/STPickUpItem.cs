using System;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

[Serializable]
public class STPickUpItem : SubTask
{
    #region Fields

    #endregion


    #region Public Methods

    public override bool DoSubtask()
    {
        if (task == null)
        {
            task = GetComponent<Task>();
        }

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


    public override bool CheckTask()
    {
        throw new NotImplementedException();
    }
    #endregion
}