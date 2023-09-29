using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

[Serializable]
public class STDoOtherTask : SubTask
{
    #region Fields
    [SerializeField] private TalkToNPC NPC;

    [TextArea]
    [Tooltip("")]
    [SerializeField] private string[] doingDialogue;
    [TextArea]
    [Tooltip("")]
    [SerializeField] private string[] finishedDialogue;

    [SerializeField] private Task[] tasksToBeDone;
    #endregion

    #region Public Methods
    private bool CheckTasks()
    {
        for (int i = 0; i < tasksToBeDone.Length; i++)
        {
            if (tasksToBeDone[i].ThisTaskState != Task.TaskState.Complete)
            {
                return false;
            }
        }
        return true;
    }


    public override bool DoSubtask()
    {
        if (!CheckTasks())
        {
            DialogueManager.Instance.DisplayDialogue(doingDialogue, null);
            return false;
        }

        //NPC.SetIcon(false);
        DialogueManager.Instance.DisplayDialogue(finishedDialogue, onEndEvent);
        return true;
    }

    public override void StartTask()
    {
        if (task == null)
        {
            task = GetComponent<Task>();
        }

        NPC.SetCurrentTask(this);

        onEndEvent.AddListener(StopTask);

        foreach (Task task in tasksToBeDone)
        {
            task.StartTask(this);
        }
    }

    public override void StopTask()
    {
        task.FinishCurrentSubtask();
    }

    public override bool CheckTask()
    {
        if (!CheckTasks())
        {
            // Tasks not done
            return false;
        }
        NPC.SetIcon(false);
        return true;
    }
    #endregion
}