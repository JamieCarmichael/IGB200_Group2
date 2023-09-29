using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

[Serializable]
public class STTalkToNPC : SubTask
{
    #region Fields
    [SerializeField] private TalkToNPC NPC;

    [TextArea]
    [Tooltip("The dialoge when the task is given to the player.")]
    [SerializeField] private string[] dialogue;
    #endregion


    #region Public Methods

    public override bool DoSubtask()
    {
        DialogueManager.Instance.DisplayDialogue(dialogue, onEndEvent);
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