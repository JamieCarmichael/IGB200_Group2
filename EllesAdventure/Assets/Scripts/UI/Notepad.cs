using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [Tooltip("The UI object that enables the notepad to be seen.")]
    [SerializeField] private GameObject notebookObject;

    //[Tooltip("How many characters can make up an item name. Spaces will be added to make all item names this long.")]
    //[SerializeField] private int itemStringLength = 20;

    /// <summary>
    /// If true the player has the notepad and it will be shown in the pause menu.
    /// </summary>
    private bool notepadAquired = false;

    /// <summary>
    /// Pages on the notepad.
    /// </summary>
    public enum Pages
    {
        Tasks, Inventroy, Profile, Map
    }

    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;

    [Header("Tasks")]
    [SerializeField] private GameObject tasksPanel;
    [SerializeField] private TextMeshProUGUI taskName;
    [SerializeField] private TextMeshProUGUI taskDescription;

    private int currentTask = 0;


    [Header("Inventory")]
    [SerializeField] private GameObject inventoryPanel;


    [Header("Profile")]
    [SerializeField] private GameObject profilePanel;


    [Header("Map")]
    [SerializeField] private GameObject mapPanel;


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
            case Pages.Profile:
                ShowProfiles();
                break;
            case Pages.Map:
                ShowMap();
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
        gameObject.SetActive(notepadAquired);
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
    /// .Show the players tasks.
    /// </summary>
    public void ShowTasks()
    {
        ShowTasks(currentTask);
    }

    private void ShowTasks(int taskNumber)
    {
        if (taskNumber >= activeTasks.Count || taskNumber < 0)
        {
            taskNumber = 0;
        }
        page = Pages.Tasks;

        tasksPanel.SetActive(true);
        inventoryPanel.SetActive(false);
        profilePanel.SetActive(false);
        mapPanel.SetActive(false);

        if (activeTasks.Count == 0)
        {
            taskName.text = "No Task";
            taskDescription.text = "";
            return;
        }

        taskName.text = activeTasks[taskNumber].TaskName;
        taskDescription.text = activeTasks[taskNumber].Description;

        // Set page buttons
        if (taskNumber >= activeTasks.Count - 1)
        {
            nextButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
        }
        if (taskNumber <= 0)
        {
            previousButton.SetActive(false);
        }
        else
        {
            previousButton.SetActive(true);
        }

    }


    /// <summary>
    /// Show the players inventory.
    /// </summary>
    public void ShowInventory()
    {
        page = Pages.Inventroy;

        tasksPanel.SetActive(false);
        inventoryPanel.SetActive(true);
        profilePanel.SetActive(false);
        mapPanel.SetActive(false);


        Dictionary<string, int> inventoryDictionary = PlayerManager.Instance.InventoryDictionary;

        //textField.text = "";

        //foreach (KeyValuePair<string, int> item in inventoryDictionary)
        //{
        //    int paddingSpace = itemStringLength - item.Key.Length;
        //    textField.text += item.Key + string.Concat(System.Linq.Enumerable.Repeat(" ", paddingSpace)) + "\t" + item.Value + "\n";
        //}
    }


    public void ShowProfiles()
    {
        page = Pages.Profile;

        tasksPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        profilePanel.SetActive(true);
        mapPanel.SetActive(false);
    }

    public void ShowMap()
    {
        page = Pages.Map;

        tasksPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        profilePanel.SetActive(false);
        mapPanel.SetActive(true);

    }

    public void NextButton()
    {
        switch (page)
        {
            case Pages.Tasks:
                currentTask += 1;
                ShowTasks(currentTask);
                break;
            case Pages.Inventroy:
                break;
            case Pages.Profile:
                break;
            case Pages.Map:
                break;
            default:
                break;
        }
    }
    public void PreviousButton()
    {
        switch (page)
        {
            case Pages.Tasks:
                currentTask -= 1;
                ShowTasks(currentTask);
                break;
            case Pages.Inventroy:
                break;
            case Pages.Profile:
                break;
            case Pages.Map:
                break;
            default:
                break;
        }
    }
    #endregion
}