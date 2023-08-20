using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Displays dialogue text on screen.
/// </summary>
public class DialogueManager : MonoBehaviour 
{
    #region Fields
    public static DialogueManager Instance { get; private set; }

    [Tooltip("The UI game object that the dialogue is attached to.")]
    [SerializeField] private GameObject dialogueObject;
    [Tooltip("The text mesh pro text object that the dialogue is displayed in.")]
    [SerializeField] private TextMeshProUGUI textField;

    /// <summary>
    /// True if there is dialogue being displayed.
    /// </summary>
    private bool dialogueDisplayed = false;
    /// <summary>
    /// The current dialogue object.
    /// </summary>
    private Dialogue currentDialogue;
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

    /// <summary>
    /// A Dialogue object. Contains the strings for the conversation and an event to be called at the end.
    /// </summary>
    [Serializable]
    public struct Dialogue
    {
        [Tooltip("The array of dialogue messages in a conversation.")]
        [TextArea]
        public string[] dialiogueChain;

        [Tooltip("The event called when the dialogue has finished.")]
        public UnityEvent endEvent;
    }
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
    public void DisplayDialogue(Dialogue newDialogue)
    {
        if (dialogueDisplayed)
        {
            return;
        }

        currentDialogue = newDialogue;
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
        if (dialogueIndex >= currentDialogue.dialiogueChain.Length)
        {
            FinishDialogue();
            return;
        }

        textField.text = currentDialogue.dialiogueChain[dialogueIndex];

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
        dialogueObject.SetActive(false);
        dialogueDisplayed = false;


        playerInteract.enabled = true;
        playerMovement.enabled = true;

        currentDialogue.endEvent.Invoke();

        InputManager.Instance.PlayerInput.InGame.Interact.performed -= NextDialogue;
    }
    #endregion
}