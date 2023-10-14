using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A subtask to collect some items and deliver them to an NPC.
/// </summary>

[Serializable]
public class STDeliverItemToNPC : SubTask
{
    [Serializable]
    public struct NumberOfItems
    {
        public string itemName;
        public int numberOfItems;
    }

    #region Fields
    private bool hasItem = false;

    [Tooltip("The name of the task displayued in the Notepad when the item being retrieved is held.")]
    [SerializeField] private string getItemTaskName;

    [SerializeField] private NumberOfItems[] itemsNeeded;

    [SerializeField] private TalkToNPC NPC;

    [Tooltip("The dialogue displayed when an item is delivered if it is not the last item.")]
    [SerializeField] private DialogueManager.DialogueSequence deliverItemDialogueSequence;

    [Tooltip("The dialogue displayed when a desired item is not held.")]
    [SerializeField] private DialogueManager.DialogueSequence noItemDialogueSequence;

    [Tooltip("The dialogue displayed when the item delivered is the last item needed for the task.")]
    [SerializeField] private DialogueManager.DialogueSequence lastItemDialogueSequence;
    #endregion


    #region Public Methods

    public override string GetName()
    {
        if (!hasItem)
        {
            return taskName;
        }
        else
        {
            return getItemTaskName;
        }
    }

    public override bool DoSubtask()
    {
        if (!DeliverItem(PlayerManager.Instance.PlayerInteract.HeldItem))
        {
            // No item delivered.
            DialogueManager.Instance.DisplayDialogue(noItemDialogueSequence, NPC.ThisProfile.CurrentProfile.name, NPC.ThisProfile.CurrentProfile.image);
            return false;
        }
        if (!CheckIfItemsAreDelivered())
        {
            // Item delivered.
            DialogueManager.Instance.DisplayDialogue(deliverItemDialogueSequence, NPC.ThisProfile.CurrentProfile.name, NPC.ThisProfile.CurrentProfile.image);
            return false;
        }
        // Last item delivered.
        DialogueManager.Instance.DisplayDialogue(lastItemDialogueSequence, NPC.ThisProfile.CurrentProfile.name, NPC.ThisProfile.CurrentProfile.image, onEndEvent);

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
    }

    public override void StopTask()
    {
        task.FinishCurrentSubtask();
    }
    #endregion

    private bool CheckIfItemsAreDelivered()
    {
        foreach (NumberOfItems item in itemsNeeded)
        {
            if (item.numberOfItems > 0)
            {
                return false;
            }
        }
        return true;
    }

    private bool DeliverItem(string itemType)
    {
        for (int i = 0; i < itemsNeeded.Length; i++)
        {
            if (itemsNeeded[i].itemName == itemType)
            {
                if (itemsNeeded[i].numberOfItems > 0)
                {
                    itemsNeeded[i].numberOfItems--;
                    PlayerManager.Instance.PlayerInteract.RemoveHeldObject();
                    return true;
                }
                else
                {
                    break;
                }
            }
        }
        return false;
    }

    public override bool CheckTask()
    {
        return CheckIfItemsAreDelivered();
    }

    public void PickUpItem(string itemName)
    {
        foreach (NumberOfItems item in itemsNeeded)
        {
            if (item.itemName == itemName)
            {
                if (item.numberOfItems > 0)
                {
                    NPC.SetIcon(false);
                    hasItem = true;
                }
                return;
            }
        }
    }
    public void PutDownItem()
    {
        NPC.HideIcon();
        hasItem = false;
    }
}