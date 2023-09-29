using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
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
    [SerializeField] private bool hasItem = false;

    [SerializeField] private string getItemTaskName;

    [SerializeField] private NumberOfItems[] itemsNeeded;

    [SerializeField] private TalkToNPC NPC;

    [TextArea]
    [Tooltip("")]
    [SerializeField] private string[] deliverItemDialogue;

    [TextArea]
    [Tooltip("")]
    [SerializeField] private string[] noItemDialogue;

    [TextArea]
    [Tooltip("")]
    [SerializeField] private string[] lastItemDialogue;
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
            DialogueManager.Instance.DisplayDialogue(noItemDialogue, NPC.ThisProfile.CurrentProfile.name, NPC.ThisProfile.CurrentProfile.image);
            return false;
        }
        if (!CheckIfItemsAreDelivered())
        {
            // Item delivered.
            DialogueManager.Instance.DisplayDialogue(deliverItemDialogue, NPC.ThisProfile.CurrentProfile.name, NPC.ThisProfile.CurrentProfile.image);
            return false;
        }
        // Last item delivered.
        DialogueManager.Instance.DisplayDialogue(lastItemDialogue, NPC.ThisProfile.CurrentProfile.name, NPC.ThisProfile.CurrentProfile.image, onEndEvent);

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