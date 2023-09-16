using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Makes NPC say something.
/// </summary>
public class TalkToNPC : MonoBehaviour, IIntertactable
{
    /// <summary>
    /// Has a task along with an event that plays when it is accepted and another event when it is completed.
    /// </summary>
    [Serializable]
    public struct TaskWithEvents
    {
        public SODoTask task;
        public UnityEvent startTaskEvent;
        public UnityEvent finishTaskEvent;
    }


    #region Fields
    public string InteractAminationString
    {
        get
        {
            return interactAminationString;
        }
    }
    [Tooltip("The string for the trigger to run the animation for this interaction.")]
    [SerializeField] private string interactAminationString;

    public bool Intertactable
    {
        get
        {
            return intertactable;
        }
    }

    private bool intertactable = true;

    [TextArea]
    [Tooltip("The dialogue said before the NPC has a task.")]
    [SerializeField] private string[] beforeTaskDialogue;
    [TextArea]
    [Tooltip("The dialogue said after a task has been completed for this NPC.")]
    [SerializeField] private string[] afterTaskDialogue;

    [Tooltip("If true the first task will be able to be done when the game starts.")]
    [SerializeField] private bool taskEnabled = false;
    /// <summary>
    /// Has the player started doing a task for this NPC.
    /// </summary>
    private bool taskStarted = false;
    /// <summary>
    /// Has the player completed the task for this NPC.
    /// </summary>
    private bool taskComplete = false;

    private Collider thisCollider;

    [Tooltip("The tasks that this NPC can have.")]
    [SerializeField] private TaskWithEvents[] tasks;

    /// <summary>
    /// What task is currently being done.
    /// </summary>
    private int currentTaskNumber = 0;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = gameObject.GetComponent<Collider>();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Start doing a task.
    /// </summary>
    /// <param name="taskNumber">The index of the task that is being started.</param>
    public void EnableTask(int taskNumber)
    {
        currentTaskNumber = taskNumber; 
        taskStarted = false;
        taskComplete = false;
        taskEnabled = true;
    }

    #endregion

    #region Private Methods
    /// <summary>
    /// Makes the current lot of dialoge and shows it with the dialogue manager.
    /// </summary>
    private void MakeDialogue()
    {
        if (taskEnabled)
        {
            if (!taskStarted)
            {
                DialogueManager.Instance.DisplayDialogue(tasks[currentTaskNumber].task.StartTaskDialogue);
                taskStarted = true;
                tasks[currentTaskNumber].task.StartTask();
                tasks[currentTaskNumber].startTaskEvent.Invoke();
                UIManager.Instance.Notepad.AddTask(tasks[currentTaskNumber].task);
            }
            else if (!taskComplete)
            {
                DialogueManager.Instance.DisplayDialogue(tasks[currentTaskNumber].task.DuringTaskDialogue);
            }
            else
            {
                DialogueManager.Instance.DisplayDialogue(tasks[currentTaskNumber].task.CompleteTaskDialogue);
                taskEnabled = false;
                UIManager.Instance.Notepad.RemoveTask(tasks[currentTaskNumber].task);
                tasks[currentTaskNumber].finishTaskEvent.Invoke();
            }
        }

        else if (!taskComplete)
        {
            DialogueManager.Instance.DisplayDialogue(beforeTaskDialogue);
        }
        else
        {
            DialogueManager.Instance.DisplayDialogue(afterTaskDialogue);
        }
    }
    #endregion

    #region IIntertactable
    public void Interact(string item)
    {
        transform.rotation = Quaternion.LookRotation(PlayerManager.Instance.PlayerTransform.position - transform.position);

        if (taskEnabled && taskStarted)
        {
            taskComplete = tasks[currentTaskNumber].task.TryComplete();
        }

        MakeDialogue();
    }

    public void StartLookAt()
    {
    }

    public void StopLookAt()
    {
    }

    public Vector3 GetClosestPoint(Vector3 playerPos)
    {
        Vector3 closestPoint = thisCollider.ClosestPoint(playerPos);
        closestPoint.y = transform.position.y;
        return closestPoint;
    }
    #endregion
}