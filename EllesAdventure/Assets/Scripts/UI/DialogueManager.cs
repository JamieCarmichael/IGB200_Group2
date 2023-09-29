using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;

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

    [SerializeField] private TextMeshProUGUI nameTextField;

    [SerializeField] private Image talkerImage;

    /// <summary>
    /// True if there is dialogue being displayed.
    /// </summary>
    private bool dialogueDisplayed = false;
    /// <summary>
    /// The current dialogue object.
    /// </summary>
    private string[] currentDialogue;
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
    public void DisplayDialogue(string[] newDialogue)
    {
        if (dialogueDisplayed)
        {
            return;
        }
        if (newDialogue.Length == 0)
        {
            return;
        }

        currentDialogue = newDialogue;
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

    public void DisplayDialogue(string[] newDialogue, string newTalkerName, Sprite newTalkerSprite)
    {
        if (dialogueDisplayed)
        {
            return;
        }
        if (newDialogue.Length == 0)
        {
            return;
        }

        currentDialogue = newDialogue;
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
    public void DisplayDialogue(string[] newDialogue, UnityEvent onFinishEvent)
    {
        PlayerManager.Instance.PlayerInteract.ClearAnimations();

        if (dialogueDisplayed)
        {
            return;
        }
        if (newDialogue.Length == 0)
        {
            onFinishEvent?.Invoke();
            return;
        }

        onFinishDialogueEvent = onFinishEvent;

        currentDialogue = newDialogue;
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
    public void DisplayDialogue(string[] newDialogue, string newTalkerName, Sprite newTalkerSprite, UnityEvent onFinishEvent)
    {
        PlayerManager.Instance.PlayerInteract.ClearAnimations();

        if (dialogueDisplayed)
        {
            return;
        }
        if (newDialogue.Length == 0)
        {
            onFinishEvent?.Invoke();
            return;
        }

        onFinishDialogueEvent = onFinishEvent;

        currentDialogue = newDialogue;
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
        if (dialogueIndex >= currentDialogue.Length)
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

        textField.text = currentDialogue[dialogueIndex];

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
    #endregion
}