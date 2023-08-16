using UnityEngine;
using TMPro;

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

    private bool dialogueDisplayed = false;

    private string[] dialogueChain = new string[0];
    private int dialogueIndex = 0; 

    private PlayerMovement playerMovement;
    private PlayerInteract playerInteract;
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

        playerInteract = FindObjectOfType<PlayerInteract>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Show the dialogue object.
    /// </summary>
    /// <param name="text">The text to display.</param>
    public void DisplayDialogue(string[] newdialogueChain)
    {
        if (dialogueDisplayed)
        {
            return;
        }

        dialogueChain = newdialogueChain;
        dialogueObject.SetActive(true);
        dialogueDisplayed = true;
        dialogueIndex = 0;
        NextDialogue();  

        playerInteract.enabled = false;
        playerMovement.enabled = false;

        InputManager.Instance.PlayerInput.InGame.Interact.performed += context => NextDialogue();
    }

    #endregion

    #region Private Methods
    private void NextDialogue()
    {
        if (dialogueIndex >= dialogueChain.Length)
        {
            HideDialogue();
            return;
        }

        textField.text = dialogueChain[dialogueIndex];

        dialogueIndex++;
    }
    /// <summary>
    /// Hide the dialogue object.
    /// </summary>
    private void HideDialogue()
    {
        dialogueObject.SetActive(false);
        dialogueDisplayed = false;


        playerInteract.enabled = true;
        playerMovement.enabled = true;


        InputManager.Instance.PlayerInput.InGame.Interact.performed -= context => NextDialogue();
    }
    #endregion
}