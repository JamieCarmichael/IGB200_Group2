using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using Cinemachine;
using System.Collections;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Displays dialogue text on screen.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Serializable]
    public struct DialogueSequence
    {
        public DialogueItem[] dialogueItems;
    }

    [Serializable]
    public struct DialogueItem
    {
        [TextArea]
        public string dialogue;
        [Tooltip("If a cinemachine virtual camera is put in here the camera will change to use this virtual camera during the dialogue.")]
        public CinemachineVirtualCamera lookCamera;
        [Tooltip("The player will not be able to continue the dialogue for this period of time (seconds).")]
        public float dialogueLockTime;
    }

    #region Fields
    public static DialogueManager Instance { get; private set; }

    [Tooltip("The UI game object that the dialogue is attached to.")]
    [SerializeField] private GameObject dialogueObject;
    [Tooltip("The text mesh pro text object that the dialogue is displayed in.")]
    [SerializeField] private TextMeshProUGUI textField;
    [Tooltip("The text field that the NPC talkings name is displayed in.")]
    [SerializeField] private TextMeshProUGUI nameTextField;
    [Tooltip("The image that the NPC talkings image is displayed in.")]
    [SerializeField] private Image talkerImage;

    [SerializeField] private GameObject continePrompt;

    /// <summary>
    /// True if there is dialogue being displayed.
    /// </summary>
    private bool dialogueDisplayed = false;
    /// <summary>
    /// The current dialogue sequence.
    /// </summary>
    private DialogueSequence currentDialogueSequence;
    /// <summary>
    /// The name of the NPC currently talking.
    /// </summary>
    private string talkerName;
    /// <summary>
    /// The sprite of the NPC currently talking.
    /// </summary>
    private Sprite talkerSprite;
    /// <summary>
    /// The index of the next dialogue to be displayed.
    /// </summary>
    private int dialogueIndex = 0;

    /// <summary>
    /// The players movement script.
    /// </summary>
    private PlayerMovement playerMovement;
    /// <summary>
    /// The players interaction script.
    /// </summary>
    private PlayerInteract playerInteract;

    private UnityEvent onFinishDialogueEvent;

    private bool isWaiting = false;
    private CinemachineVirtualCamera currentCamera;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        playerInteract = PlayerManager.Instance.PlayerTransform.GetComponent<PlayerInteract>();
        playerMovement = PlayerManager.Instance.PlayerTransform.GetComponent<PlayerMovement>();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Show the dialogue object.
    /// </summary>
    /// <param name="newDialogue"></param>
    public void DisplayDialogue(DialogueSequence newDialogue)
    {
        if (dialogueDisplayed)
        {
            return;
        }
        if (newDialogue.dialogueItems.Length == 0)
        {
            return;
        }

        currentDialogueSequence = newDialogue;
        talkerName = null;
        talkerSprite = null;

        dialogueObject.SetActive(true);
        dialogueDisplayed = true;
        dialogueIndex = 0;
        NextDialogue();

        playerInteract.enabled = false;
        playerMovement.enabled = false;

        InputManager.Instance.PlayerInput.InGame.Interact.performed += NextDialogue;
    }

    public void DisplayDialogue(DialogueSequence newDialogue, string newTalkerName, Sprite newTalkerSprite)
    {
        if (dialogueDisplayed)
        {
            return;
        }
        if (newDialogue.dialogueItems.Length == 0)
        {
            return;
        }

        currentDialogueSequence = newDialogue;

        talkerName = newTalkerName;
        talkerSprite = newTalkerSprite;

        dialogueObject.SetActive(true);
        dialogueDisplayed = true;
        dialogueIndex = 0;
        NextDialogue();

        playerInteract.enabled = false;
        playerMovement.enabled = false;

        InputManager.Instance.PlayerInput.InGame.Interact.performed += NextDialogue;
    }

    /// <summary>
    /// Show the dialogue object.
    /// </summary>
    /// <param name="newDialogue"></param>
    /// <param name="onFinishEvent"></param>
    public void DisplayDialogue(DialogueSequence newDialogue, UnityEvent onFinishEvent)
    {
        PlayerManager.Instance.PlayerInteract.ClearAnimations();

        if (dialogueDisplayed)
        {
            return;
        }

        if (newDialogue.dialogueItems.Length == 0)
        {
            onFinishEvent?.Invoke();
            return;
        }

        onFinishDialogueEvent = onFinishEvent;

        currentDialogueSequence = newDialogue;
        talkerName = null;
        talkerSprite = null;

        dialogueObject.SetActive(true);
        dialogueDisplayed = true;
        dialogueIndex = 0;
        NextDialogue();

        playerInteract.enabled = false;
        playerMovement.enabled = false;

        InputManager.Instance.PlayerInput.InGame.Interact.performed += NextDialogue;
    }
    /// <summary>
    /// Show the dialogue object.
    /// </summary>
    /// <param name="newDialogue"></param>
    /// <param name="newTalkerSprite"></param>
    /// <param name="newTalkerName"></param>
    /// <param name="onFinishEvent"></param>
    public void DisplayDialogue(DialogueSequence newDialogue, string newTalkerName, Sprite newTalkerSprite, UnityEvent onFinishEvent)
    {
        PlayerManager.Instance.PlayerInteract.ClearAnimations();

        if (dialogueDisplayed)
        {
            return;
        }
        if (newDialogue.dialogueItems.Length == 0)
        {
            onFinishEvent?.Invoke();
            return;
        }

        onFinishDialogueEvent = onFinishEvent;

        currentDialogueSequence = newDialogue;
        talkerName = newTalkerName;
        talkerSprite = newTalkerSprite;

        dialogueObject.SetActive(true);
        dialogueDisplayed = true;
        dialogueIndex = 0;
        NextDialogue();

        playerInteract.enabled = false;
        playerMovement.enabled = false;

        InputManager.Instance.PlayerInput.InGame.Interact.performed += NextDialogue;
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Display the text for the next dialogue option.
    /// </summary>
    private void NextDialogue()
    {
        if (isWaiting)
        {
            return;
        }
        // Hide prompt
        continePrompt.SetActive(false);

        // Reset the camera being used.
        if (currentCamera != null)
        {
            currentCamera.gameObject.SetActive(false);
        }

        if (dialogueIndex >= currentDialogueSequence.dialogueItems.Length)
        {
            FinishDialogue();
            return;
        }
        if (talkerName != null && talkerName != "")
        {
            nameTextField.text = talkerName;
            nameTextField.enabled = true;
        }
        else
        {
            nameTextField.enabled = false;
        }
        if (talkerSprite != null)
        {
            talkerImage.sprite = talkerSprite;
            talkerImage.enabled = true;
        }
        else
        {
            talkerImage.enabled = false;
        }

        textField.text = currentDialogueSequence.dialogueItems[dialogueIndex].dialogue;

        isWaiting = true;
        StartCoroutine(EnableNext(currentDialogueSequence.dialogueItems[dialogueIndex].dialogueLockTime));
        // set camera
        if (currentDialogueSequence.dialogueItems[dialogueIndex].lookCamera != null)
        {
            currentCamera = currentDialogueSequence.dialogueItems[dialogueIndex].lookCamera;
            currentCamera.gameObject.SetActive(true);
        }
        else
        {
            if (currentCamera != null)
            {
                currentCamera.gameObject.SetActive(false);
            }
            currentCamera = null;
        }

        dialogueIndex++;
    }

    /// <summary>
    /// Display the text for the next dialogue option.
    /// </summary>
    /// <param name="obj"></param>
    private void NextDialogue(InputAction.CallbackContext obj)
    {
        NextDialogue();
    }

    /// <summary>
    /// Hide the dialogue object. Calls the event at the end of the dialogue.
    /// </summary>
    private void FinishDialogue()
    {
        onFinishDialogueEvent?.Invoke();
        onFinishDialogueEvent = null;

        dialogueObject.SetActive(false);
        dialogueDisplayed = false;


        playerInteract.enabled = true;
        playerMovement.enabled = true;

        InputManager.Instance.PlayerInput.InGame.Interact.performed -= NextDialogue;
    }

    /// <summary>
    /// A corutine to make it so the dialogue can not contunue until timer has run.
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator EnableNext(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;

        // Hide prompt
        continePrompt.SetActive(true);
    }
    #endregion
}