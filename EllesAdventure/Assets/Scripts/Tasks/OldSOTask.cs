using UnityEngine;
/// <summary>
/// Made By: Jamie Carmichael
/// Details: An abstract class to allow all tasks to be assigned the same type.
/// </summary>

public abstract class OldSOTask : ScriptableObject
{
    /// <summary>
    /// Has this task been completed.
    /// </summary>
    public virtual bool IsComplete { get; }

    /// <summary>
    /// The name of this task. Displayed in the task list.
    /// </summary>
    public virtual string TaskName { get; }

    /// <summary>
    /// Try to complete this task.
    /// </summary>
    /// <returns>If true the task was completed.</returns>
    public abstract bool TryComplete();

    /// <summary>
    /// Start doing this task. Initializes anything that needs to be set up when task is started.
    /// </summary>
    public abstract void StartTask();
}