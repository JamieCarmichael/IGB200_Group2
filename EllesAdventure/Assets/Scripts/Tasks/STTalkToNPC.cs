using Cinemachine;
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

    [TextArea]
    [Tooltip("The dialoge when the task is given to the player.")]
    [SerializeField] private string[] dialogue;

    [Tooltip("If a cinemachine virtual camera is put in here the camera will change to use this virtual camera during the dialogue.")]
    [SerializeField] CinemachineVirtualCamera lookCamera;
    #endregion


    #region Public Methods

    public override bool DoSubtask()
    {
        if (lookCamera != null)
        {
            lookCamera.gameObject.SetActive(true);
        }
        DialogueManager.Instance.DisplayDialogue(dialogue, NPC.ThisProfile.CurrentProfile.name, NPC.ThisProfile.CurrentProfile.image, onEndEvent);
        return true;
    }

    public override void StartTask()
    {
        if (task == null)
        {
            task = GetComponent<Task>();
        }

        NPC.SetCurrentTask(this, task.CurrentSubTask == 0);


        if (lookCamera != null)
        {
            onEndEvent.AddListener(DisableCamera);
        }

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
    private void DisableCamera()
    {
        if (lookCamera != null)
        {
            lookCamera.gameObject.SetActive(false);
        }
    }
    #endregion
}