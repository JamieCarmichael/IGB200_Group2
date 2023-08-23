using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A UI object that displays information for the player.
///             Should be part of the pause menu.
///             Contains tasks and inventory.
/// </summary>
public class Notepad : MonoBehaviour 
{
    #region Fields
    /// <summary>
    /// A list of all currenly active tasks.
    /// </summary>
    private List<SODoTask> activeTasks = new List<SODoTask>();

    [Tooltip("The text field that shows information in the notepad.")]
    [SerializeField] private TextMeshProUGUI textField;
    [Tooltip("The UI object that enables the notepad to be seen.")]
    [SerializeField] private GameObject notebookObject;

    [Tooltip("How many characters can make up an item name. Spaces will be added to make all item names this long.")]
    [SerializeField] private int itemStringLength = 20;

    /// <summary>
    /// If true the player has the notepad and it will be shown in the pause menu.
    /// </summary>
    private bool notepadAquired = false;

    /// <summary>
    /// Pages on the notepad.
    /// </summary>
    public enum Pages
    {
        Tasks, Inventroy
    }

    /// <summary>
    /// The currrent page opened in the notepad.
    /// </summary>
    private Pages page = Pages.Tasks;
    #endregion

    #region Unity Call Functions
    private void OnEnable()
    {
        notebookObject.SetActive(notepadAquired);

        // Select current page.
        switch (page) 
        {
            case Pages.Tasks:
                ShowTasks();
                break; 
            case Pages.Inventroy:
                ShowInventory();
                break; 
            default:
                break;
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Set if the notepad is visable in the Pause menu.
    /// </summary>
    /// <param name="isAquired">If true the notepad is able to be seen.</param>
    public void AquireNotepad(bool isAquired)
    {
        notepadAquired = isAquired;
    }
    /// <summary>
    /// Add a task to the player tasks.
    /// </summary>
    /// <param name="newTask"></param>
    public void AddTask(SODoTask newTask)
    {
        activeTasks.Add(newTask);
    }
    /// <summary>
    /// Remove a task from the players tasks.
    /// </summary>
    /// <param name="task"></param>
    public void RemoveTask(SODoTask task)
    {
        activeTasks.Remove(task);
    }
    /// <summary>
    /// Show the players inventory.
    /// </summary>
    public void ShowInventory()
    {
        page = Pages.Inventroy;

        Dictionary<string, int> inventoryDictionary = PlayerManager.Instance.InventoryDictionary;

        textField.text = "";

        foreach (KeyValuePair<string, int> item in inventoryDictionary)
        {
            int paddingSpace = itemStringLength - item.Key.Length;
            textField.text += item.Key + string.Concat(System.Linq.Enumerable.Repeat(" ", paddingSpace)) + "\t" + item.Value + "\n";
        }
    }
    /// <summary>
    /// .Show the players tasks.
    /// </summary>
    public void ShowTasks()
    {
        page = Pages.Tasks;

        textField.text = "";

        foreach (SODoTask task in activeTasks)
        {
            textField.text += task.TaskName + "\n" + task.Description;
        }
    }
    #endregion
}