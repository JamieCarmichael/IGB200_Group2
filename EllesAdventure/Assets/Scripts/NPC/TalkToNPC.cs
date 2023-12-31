using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Makes NPC say something.
/// </summary>
public class TalkToNPC : MonoBehaviour, IIntertactable
{
    #region Fields
    [SerializeField] private Profile profile;

    public Profile ThisProfile { get { return profile; } }

    private bool firstInteraction = true;

    [Header("Task Icon")]
    [SerializeField] private NPCTaskIndicator icon;

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

    [Tooltip("A list of all dialogue options. The first one is the start dialogue others can be used as the situation changes.")]
    [SerializeField] private DialogueManager.DialogueSequence[] dialogueSequences;


    private int currentDialogueOption = 0;

    private Collider thisCollider;

    [Header("Text Prompt")]
    [Tooltip("The transform of the object that the text prompt will apear over.")]
    [SerializeField] private Transform proptLocation;
    [Tooltip("The text displayed in the text prompt.")]
    [SerializeField] private string proptText;

    // New task
    private SubTask currentTask = null;
    public SubTask CurrentTask 
    { 
        get 
        { 
            return currentTask; 
        } 
    }
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = gameObject.GetComponent<Collider>();

        if (currentTask == null)
        {
            icon.HideIcon();
        }
    }
    #endregion

    #region Public Methods
    public void SetIcon(bool isNewTask)
    {
        if (isNewTask)
        {
            icon.ShowNewIcon();
        }
        else
        {
            icon.ShowRunningIcon();
        }
    }
    public void HideIcon()
    {
        icon.HideIcon();
    }

    public void SetCurrentTask(SubTask newTask, bool isStartOfTask)
    {
        SetIcon(isStartOfTask);

        currentTask = newTask;
    }
    public void SetCurrentTask(SubTask newTask)
    {
        currentTask = newTask;
    }

    public void SetDialogueOption(int dialogueOptionIndex)
    {
        currentDialogueOption = dialogueOptionIndex;
    }

        #endregion

    #region Private Methods
    /// <summary>
    /// Makes the current lot of dialoge and shows it with the dialogue manager.
    /// </summary>
    private void MakeDialogue()
    {
        SubTask startOfMethodTask = currentTask;
        if (currentTask != null)
        {
            if (currentTask.DoSubtask())
            {
                // If the current task is the task from the start of the method then remove it.
                // This is because the task may be replaced when doing the task.
                if (currentTask == startOfMethodTask)
                {
                    currentTask = null;
                }
                icon.HideIcon();
            }
        }
        else
        {
            DialogueManager.Instance.DisplayDialogue(dialogueSequences[currentDialogueOption], profile.CurrentProfile.name, profile.CurrentProfile.image);
        }
    }

    private IEnumerator StartTalking()
    {
        float timer = 0.0f;
        float timeToRotate = 1.0f;

        Vector3 lookDir = PlayerManager.Instance.PlayerTransform.position - transform.position;
        lookDir.y = 0;


        PlayerManager.Instance.PlayerInteract.enabled = false;
        PlayerManager.Instance.PlayerMovement.enabled = false;

        while (timer < timeToRotate)
        {
            timer += Time.deltaTime;

            Vector3 faceDir = Vector3.Slerp(transform.forward, lookDir, timer / timeToRotate);

            transform.rotation = Quaternion.LookRotation(faceDir);

            if (Vector3.Angle(faceDir, lookDir) < 0.1f)
            {
                break;
            }

            yield return null;
        }
        transform.rotation = Quaternion.LookRotation(lookDir);

        PlayerManager.Instance.PlayerInteract.enabled = true;
        PlayerManager.Instance.PlayerMovement.enabled = true;

        MakeDialogue();
    }
    #endregion

    #region IIntertactable
    public void Interact(string item)
    {

        if (firstInteraction)
        {
            firstInteraction = false;
            profile.IncreaseProfileStage();
        }

        StopLookAt();

        StartCoroutine(StartTalking());
    }

    public void LookAt()
    {
        UIManager.Instance.TextPrompt.DisplayPrompt(proptLocation.position, proptText);
    }

    public void StopLookAt()
    {
        UIManager.Instance.TextPrompt.HidePrompt();
    }

    public Vector3 GetClosestPoint(Vector3 playerPos)
    {
        Vector3 closestPoint = thisCollider.ClosestPoint(playerPos);
        closestPoint.y = transform.position.y;
        return closestPoint;
    }

    public void UpdateProfile()
    {
        profile.IncreaseProfileStage();
    }
    #endregion
}