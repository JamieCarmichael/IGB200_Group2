using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

[CreateAssetMenu(fileName = "NewSODoTask", menuName = "ScriptableObjects/SODoTask")]
public class SODoTask : SOTask
{
    #region Fields

    [TextArea]
    [SerializeField] private string[] giveTaskDialogue;

    [TextArea]
    [SerializeField] private string[] duringTaskDialogue;

    [TextArea]
    [SerializeField] private string[] finishTaskDialogue;

    [SerializeField] private SOTask[] tasks;

    [SerializeField] private string taskName;
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

    public override string TaskName
    {
        get { return taskName; }
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
            if (tasks[i].GetType() == typeof(SOFindTask))
            {
                tasks[i].TryComplete();
            }
        }
        return IsComplete;
    }

    public override void StartTask()
    {
        GiveTaskDialogue = giveTaskDialogue;
        DuringTaskDialogue = duringTaskDialogue;
        FinishTaskDialogue = finishTaskDialogue;

        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].StartTask();
        }
    }
    #endregion
}