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

    public SubTask[] subTasks;

    public TaskState taskState = TaskState.Incactive;

    public int currentSubTask = 0;


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
        currentSubTask++;
        // If not more sub stasks left then task is complete
        if (currentSubTask >= subTasks.Length)
        {
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

        taskState = TaskState.Available;
        currentSubTask = 0;

        // Set up first subtasks (Show icon for NPC)
        subTasks[0].StartTask();
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
}