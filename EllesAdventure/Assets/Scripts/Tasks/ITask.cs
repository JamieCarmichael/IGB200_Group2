
using System;
using UnityEngine;
/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>



[Serializable]
public class ITask
{
    public int a;

    public virtual bool IsComplete { get; }

    public virtual string Name { get; }

    public virtual string Description { get; }

    public virtual bool TryComplete()
    {
        return false;
    }
}