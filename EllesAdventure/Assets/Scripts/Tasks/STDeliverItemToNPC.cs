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

    public override bool DoSubtask()
    {
        if (!DeliverItem(PlayerManager.Instance.PlayerInteract.HeldItem))
        {
            // No item delivered.
            DialogueManager.Instance.DisplayDialogue(noItemDialogue, null);
            return false;
        }
        if (!CheckIfItemsAreDelivered())
        {
            // Item delivered.
            DialogueManager.Instance.DisplayDialogue(deliverItemDialogue, null);
            return false;
        }
        // Last item delivered.
        DialogueManager.Instance.DisplayDialogue(lastItemDialogue, onEndEvent);

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
    public bool CheckItem(string itemName)
    {

        foreach (NumberOfItems item in itemsNeeded)
        {
            if (item.itemName == itemName)
            {
                NPC.SetIcon(false);
                return true;
            }
        }
        return false;
    }
}