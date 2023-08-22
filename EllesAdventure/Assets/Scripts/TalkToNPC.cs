using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Makes NPC say something.
/// </summary>
public class TalkToNPC : MonoBehaviour, IIntertactable
{
    #region Fields
    [TextArea]
    [Tooltip("The dialogue said before the NPC has a task.")]
    [SerializeField] private string[] beforeTaskDialogue;
    [TextArea]
    [Tooltip("The dialogue said after a task has been completed for this NPC.")]
    [SerializeField] private string[] afterTaskDialogue;

    public bool taskEnabled = false;
    private bool taskStarted = false;
    private bool taskComplete = false;

    private Collider thisCollider;

    public SOTask task;

    [SerializeField] private UnityEvent startTaskEvent;
    [SerializeField] private UnityEvent finishTaskEvent;

    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = gameObject.GetComponent<Collider>();
    }
    #endregion

    #region Public Methods
    public void TaskFinished()
    {
        taskComplete = true;
    }

    public void EnableTask()
    {
        taskEnabled = true;
    }
    #endregion

    #region Private Methods
    private void MakeDialogue()
    {
        if (taskEnabled)
        {
            if (!taskStarted)
            {
                DialogueManager.Instance.DisplayDialogue(task.GiveTaskDialogue);
                taskStarted = true;
                task.StartTask();
                startTaskEvent.Invoke();
            }
            else if (!taskComplete)
            {
                DialogueManager.Instance.DisplayDialogue(task.DuringTaskDialogue);
            }
            else
            {
                DialogueManager.Instance.DisplayDialogue(task.FinishTaskDialogue);
                taskEnabled = false;
                finishTaskEvent.Invoke();
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

    public string InteractAminationString
    {
        get
        {
            return interactAminationString;
        }
    }

    [SerializeField] private string interactAminationString;

    public bool Intertactable
    {
        get
        {
            return intertactable;
        }
    }

    private bool intertactable = true;

    public void Interact()
    {
        transform.rotation = Quaternion.LookRotation(PlayerManager.Instance.PlayerTransform.position - transform.position);

        if (taskEnabled && taskStarted)
        {
            taskComplete = task.TryComplete();
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