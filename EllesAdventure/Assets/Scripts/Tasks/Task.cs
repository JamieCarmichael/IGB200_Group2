using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.VolumeComponent;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A task. This is a collection of subtasks that need to be completed.
/// </summary>

public class Task : MonoBehaviour
{
    #region Enum/structs
    public enum TaskState
    {
        Incactive,
        Active,
        Complete
    }

    #endregion

    #region Fields
    [SerializeField] private string taskName;

    [SerializeField] private bool buildBuildingOnComplete = false;

    private SubTask[] subTasks;

    private TaskState taskState = TaskState.Incactive;

    private int currentSubTask = 0;

    [SerializeField] private Task[] nextTasks;

    [SerializeField] private UnityEvent startTaskEvent;

    public int CurrentSubTask { get { return currentSubTask; } }

    /// <summary>
    /// If this task is assigned by STDoOtherTask than this is that task.
    /// </summary>
    private STDoOtherTask owningSubtask;
    #endregion

    #region Properties
    public TaskState ThisTaskState { get { return taskState; } }
    #endregion

    #region Big Methods
    public string GetTaskForNotepad()
    {
        string taskDescription = "";

        if (taskState == TaskState.Complete)
        {
            taskDescription += "<s>";
        }

        taskDescription += taskName;
        taskDescription += "\n";
        taskDescription += "<indent=15%>" + subTasks[currentSubTask].GetName() + "</indent>";
        // Add name of current subtask

        if (taskState == TaskState.Complete)
        {
            taskDescription += "</s>";
        }
        return taskDescription;
    }
    #endregion

    #region Small Methods
    public void DoTask()
    {
        // 
        if(subTasks[currentSubTask].DoSubtask())
        {
            currentSubTask++;
            // If not more sub stasks left then task is complete
            if (currentSubTask >= subTasks.Length)
            {
                FinishTask();
            }
        }
    }

    public void FinishCurrentSubtask()
    {
        Notepad.Instance.UpdateNotepadPrompt(Notepad.Pages.Tasks);

        currentSubTask++;
        // If not more sub stasks left then task is complete
        if (currentSubTask >= subTasks.Length)
        {
            currentSubTask = subTasks.Length - 1;
            FinishTask();
            return;
        }
        subTasks[currentSubTask].StartTask();
    }

    public void StartTask()
    {
        subTasks = GetComponents<SubTask>();
        if (subTasks.Length < 0) 
        {
            Debug.Log("No subtasks found!");
            FinishTask();
        }

        taskState = TaskState.Active;
        currentSubTask = 0;

        // Set up first subtasks (Show icon for NPC)
        subTasks[0].StartTask();

        // Add this task to the task manager
        TaskManager.Instance.AddTask(this);

        startTaskEvent?.Invoke();
    }


    public void StartTask(STDoOtherTask sTDoOtherTask)
    {
        owningSubtask = sTDoOtherTask;
        StartTask();
    }

    private void FinishTask()
    {
        taskState = TaskState.Complete;

        if (buildBuildingOnComplete)
        {
            HouseProgress.Instance.IncreaseProgressLevel();
        }

        foreach (Task task in nextTasks)
        {
            task.StartTask();
        }

        if (owningSubtask != null)
        {
            owningSubtask.CheckTask();
        }
    }

    public void CheckItemPickup(string itemName)
    {
        if (subTasks[currentSubTask].GetType() == typeof(STDeliverItemToNPC))
        {
            STDeliverItemToNPC thisSubtask = (STDeliverItemToNPC)subTasks[currentSubTask];
            thisSubtask.PickUpItem(itemName);
        }
    }
    public void DropItemPickup()
    {
        if (subTasks[currentSubTask].GetType() == typeof(STDeliverItemToNPC))
        {
            STDeliverItemToNPC thisSubtask = (STDeliverItemToNPC)subTasks[currentSubTask];
            thisSubtask.PutDownItem();
        }
    }
    #endregion
}