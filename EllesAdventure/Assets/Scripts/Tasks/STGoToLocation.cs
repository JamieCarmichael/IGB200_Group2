using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A subtasks to go to a location adn hit a trigger.
/// </summary>

[Serializable]
public class STGoToLocation : SubTask
{
    #region Fields
    [SerializeField] private PlayerTriggerLocation triggerLocation;

    [Tooltip("The dialoge when the trigger is activated")]
    [SerializeField] private DialogueManager.DialogueSequence dialogueSequence;

    #endregion


    #region Public Methods

    public override bool DoSubtask()
    {
        DialogueManager.Instance.DisplayDialogue(dialogueSequence, onEndEvent);

        return true;
    }

    public override void StartTask()
    {
        if (task == null)
        {
            task = GetComponent<Task>();
        }

        triggerLocation.SetCurrentTask(this);
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