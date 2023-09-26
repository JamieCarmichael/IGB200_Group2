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


    [SerializeField] private List<Task> allTasks = new List<Task>();

    private List<Task> activeTasks = new List<Task>();
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

        // Start first task
        StartTask(allTasks[0]);
    }
    #endregion

    #region Public Methods
    public void StartTask(Task newTask)
    {
        activeTasks.Add(newTask);
        newTask.StartTask();
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
    #endregion

}