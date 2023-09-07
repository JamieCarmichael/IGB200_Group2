using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A tasks that consists of other tasks. The other tasks can be more Do tasks or can be find tasks.
///             All NPC's have these assigned to them.
/// </summary>

[CreateAssetMenu(fileName = "NewSODoTask", menuName = "ScriptableObjects/SODoTask")]
public class SODoTask : SOTask
{
    #region Fields
    [TextArea]
    [Tooltip("The dialoge when the task is given to the player.")]
    [SerializeField] private string[] startTaskDialogue;

    [TextArea]
    [Tooltip("The dialoge when the player is doing the task.")]
    [SerializeField] private string[] duringTaskDialogue;

    [TextArea]
    [Tooltip("The dialoge when the player completes the task.")]
    [SerializeField] private string[] completeTaskDialogue;

    [Tooltip("An array of tasks to be completed for this task to be done.")]
    [SerializeField] private SOTask[] tasks;

    [Tooltip("The name of this task. Is displayed in the notepad.")]
    [SerializeField] private string taskName;

    [Tooltip("If true the building will be added to when this task is completed.")]
    [SerializeField] private bool buildBuildingOnComplete = false;
    #endregion

    #region Properties
    /// <summary>
    /// The description of the tasks that need to be completed for this task.
    /// A string of the tasks left to be completed. Each task is indented on a new line.
    /// If all tasls are completed returens a string "Finished".
    /// </summary>
    public string Description
    {
        get
        {
            string value = "";

            for (int i = 0; i < tasks.Length; i++)
            {
                if (!tasks[i].IsComplete)
                {
                    value += "\t" + tasks[i].TaskName + "\n";
                }
            }

            if (value == "")
            {
                value = "\tFinished\n";
            }

            return value;
        }
    }

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

    public override string TaskName
    {
        get { return taskName; }
    }

    public string[] StartTaskDialogue { get { return startTaskDialogue; } }
    public string[] DuringTaskDialogue { get { return duringTaskDialogue; } }
    public string[] CompleteTaskDialogue { get { return completeTaskDialogue; } }
    #endregion

    #region Public Methods
    public override bool TryComplete()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i].GetType() == typeof(SOFindTask))
            {
                tasks[i].TryComplete();
            }
        }
        // Add to central building.
        if (IsComplete && buildBuildingOnComplete)
        {
            HouseProgress.Instance.IncreaseProgressLevel();
        }

        return IsComplete;
    }

    public override void StartTask()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].StartTask();
        }
    }
    #endregion
}