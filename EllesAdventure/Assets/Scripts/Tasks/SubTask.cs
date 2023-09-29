using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A smaller task within a task that needs to be done. A step in the procedure.
/// </summary>

//[Serializable]
public abstract class SubTask : MonoBehaviour
{
    #region Fields
    protected Task task;

    [SerializeField] protected string taskName;

    [SerializeField] protected UnityEvent onEndEvent;

    #endregion

    #region Public Methods
    public virtual string GetName()
    {
        return taskName;
    }

    public abstract bool DoSubtask();

    public abstract void StartTask();

    public abstract void StopTask();

    public abstract bool CheckTask();
    #endregion
}