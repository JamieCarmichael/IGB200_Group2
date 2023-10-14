using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A subtasks to go and talk to an NPC.
/// </summary>

[Serializable]
public class STTalkToNPC : SubTask
{
    #region Fields
    [SerializeField] private TalkToNPC NPC;

    [Tooltip("The dialoge when the task is given to the player.")]
    [SerializeField] private DialogueManager.DialogueSequence dialogueSequence;
    #endregion


    #region Public Methods

    public override bool DoSubtask()
    {
        DialogueManager.Instance.DisplayDialogue(dialogueSequence, NPC.ThisProfile.CurrentProfile.name, NPC.ThisProfile.CurrentProfile.image, onEndEvent);
        return true;
    }

    public override void StartTask()
    {
        if (task == null)
        {
            task = GetComponent<Task>();
        }

        NPC.SetCurrentTask(this, task.CurrentSubTask == 0);

        onEndEvent.AddListener(StopTask);
    }

    public override void StopTask()
    {
        task.FinishCurrentSubtask();
    }

    public override bool CheckTask()
    {
        return false;
    }
    #endregion
}