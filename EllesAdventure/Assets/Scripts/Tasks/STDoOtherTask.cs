using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A Subtask to do some other tasks for an NPC.
/// </summary>

[Serializable]
public class STDoOtherTask : SubTask
{
    #region Fields
    [SerializeField] private TalkToNPC NPC;

    [Tooltip("")]
    [SerializeField] private DialogueManager.DialogueSequence doingDialogueSequence;

    [Tooltip("")]
    [SerializeField] private DialogueManager.DialogueSequence finishedDialogueSequence;

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
            DialogueManager.Instance.DisplayDialogue(doingDialogueSequence, NPC.ThisProfile.CurrentProfile.name, NPC.ThisProfile.CurrentProfile.image);
            return false;
        }

        //NPC.SetIcon(false);
        DialogueManager.Instance.DisplayDialogue(finishedDialogueSequence, NPC.ThisProfile.CurrentProfile.name, NPC.ThisProfile.CurrentProfile.image, onEndEvent);
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