using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>

public class Task : MonoBehaviour
{
    #region Enum/structs
    public enum TaskState
    {
        Incactive,
        Available,
        Active,
        Complete
    }

    #endregion

    #region Fields
    [SerializeField] private string taskName;

    [SerializeField] private bool buildBuildingOnComplete = false;

    [SerializeField] private List<SubTask> subTasks;

    private TaskState taskState = TaskState.Incactive;

    private int currentSubTask = 0;


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
        taskDescription += "\n\t";
        taskDescription += subTasks[currentSubTask].GetName();
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
        if(subTasks[currentSubTask].TryComplete())
        {
            currentSubTask++;
            // If not more sub stasks left then task is complete
            if (currentSubTask >= subTasks.Count)
            {
                FinishTask();
            }
        }
    }

    public void StartTask()
    {
        taskState = TaskState.Available;
        currentSubTask = 0;
        // Set up first subtasks (Show icon for NPC)
    }

    private void FinishTask()
    {
        taskState = TaskState.Complete;

        if (buildBuildingOnComplete)
        {
            HouseProgress.Instance.IncreaseProgressLevel();
        }
    }
    #endregion


    #region For Editor
    public void AddSubtask(SubTask.SubtaskType newSubTask)
    {
        switch (newSubTask)
        {
            case SubTask.SubtaskType.TalkToNPC:
                subTasks.Add(new STTalkToNPC());
                break;
            case SubTask.SubtaskType.GoToLocation:
                subTasks.Add(new STGoToLocation());
                break;
            case SubTask.SubtaskType.PickUpItem:
                subTasks.Add(new STPickUpItem());
                break;
            case SubTask.SubtaskType.DeliverItem:
                subTasks.Add(new STDeliverItem());
                break;
            case SubTask.SubtaskType.DoOtherTask:
                subTasks.Add(new STDoOtherTask());
                break;
            default:
                break;

        }
    }
    #endregion
}