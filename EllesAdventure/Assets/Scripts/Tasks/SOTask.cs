using UnityEngine;
/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

public abstract class SOTask : ScriptableObject
{
    [HideInInspector]
    public string[] GiveTaskDialogue;

    [HideInInspector]
    public string[] DuringTaskDialogue;

    [HideInInspector]
    public string[] FinishTaskDialogue;

    public virtual bool IsComplete { get; }

    public virtual string TaskName { get; }

    public virtual string Description { get; }

    public abstract bool TryComplete();

    public abstract void StartTask();

}