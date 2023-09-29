using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>
public class TaskManager : MonoBehaviour 
{
    #region Fields
    public static TaskManager Instance { get; private set; }

    [SerializeField] private List<Task> startTasks = new List<Task>();

    [SerializeField] private List<Task> activeTasks = new List<Task>();
    #endregion

    #region Unity Call Functions
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Start tasks
        foreach (Task task in startTasks)
        {
            StartTask(task);
        }
    }
    #endregion

    #region Public Methods
    private void StartTask(Task newTask)
    {
        newTask.StartTask();
    }

    public void AddTask(Task newTask)
    {
        activeTasks.Add(newTask);
    }

    public string GetTaskListForNotepad()
    {
        string tasksString = "";
        foreach (Task task in activeTasks)
        {
            if (task.ThisTaskState == Task.TaskState.Active || task.ThisTaskState == Task.TaskState.Complete)
            {
                tasksString += task.GetTaskForNotepad();
                tasksString += "\n";
            }
        }
        return tasksString;
    }

    public void PickUpItem(string itemName)
    {
        foreach (Task task in activeTasks)
        {
            if (task.ThisTaskState == Task.TaskState.Active)
            {
                task.CheckItemPickup(itemName);
            }
        }
    }
    public void PutDownItem()
    {
        foreach (Task task in activeTasks)
        {
            if (task.ThisTaskState == Task.TaskState.Active)
            {
                task.DropItemPickup();
            }
        }
    }
    #endregion

}