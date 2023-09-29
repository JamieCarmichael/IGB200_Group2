using System;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

[Serializable]
public class STGoToLocation : SubTask
{
    #region Fields
    [SerializeField] private PlayerTriggerLocation triggerLocation;

    [TextArea]
    [Tooltip("The dialoge when the trigger is activated")]
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