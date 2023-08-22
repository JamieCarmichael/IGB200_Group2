using UnityEngine;
using System;
/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>


[Serializable]
public class DoTask : ITask
{
    #region Fields
    [SerializeField] private ITask[] tasks;

    [SerializeField] private string name;
    #endregion

    #region Properties
    public override bool IsComplete
    {
        get
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                if (!tasks[i].IsComplete)
                {
                    return false;
                }
            }
            return true;
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
            string value = "";

            for (int i = 0; i < tasks.Length; i++)
            {
                value += "\t" + tasks[i].Description + "\n";
            }

            return value;
        }
    }
    #endregion

    #region Public Methods
    public override bool TryComplete()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].TryComplete();
        }
        return IsComplete;
    }
    #endregion
}