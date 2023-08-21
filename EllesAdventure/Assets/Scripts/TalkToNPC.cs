using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Makes NPC say something.
/// </summary>
public class TalkToNPC : MonoBehaviour, IIntertactable
{
    #region Fields
    [Tooltip("The dialogue said in the first interaction.")]
    [SerializeField] private DialogueManager.Dialogue startDialogue;
    [Tooltip("The dialogue said in the second interaction.")]
    [SerializeField] private DialogueManager.Dialogue endDialogue;

    [Tooltip("The dialogue said in the second interaction.")]
    [SerializeField] private DialogueManager.Dialogue itemDialogue;
    [Tooltip("Item needed for dialogue.")]
    [SerializeField] private string itemNeeded;
    [Tooltip("How many items are needed.")]
    [SerializeField] private int numberOfItemsdNeeded;

    public bool dialogueItem = false;
    public bool dialogue1 = true;
    public bool dialogue2 = false;

    private Collider thisCollider;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = gameObject.GetComponent<Collider>();
    }
    #endregion

    #region Public Methods
    public void ChangeDialogue()
    {
        dialogue2 = true;
    }
    #endregion

    #region Private Methods
    private void MakeDialogue()
    {
        if (dialogueItem && PlayerManager.Instance.UseItem(itemNeeded, numberOfItemsdNeeded, true))
        {
            DialogueManager.Instance.DisplayDialogue(itemDialogue);
            dialogueItem = false;
        }

        else if (dialogue1)
        {
            DialogueManager.Instance.DisplayDialogue(startDialogue);
            dialogue1 = false;
        }
        else if (dialogue2)
        {
            DialogueManager.Instance.DisplayDialogue(endDialogue);
            dialogue2 = false;
        }
    }
    #endregion

    #region IIntertactable
    public void EnableDialogueItem()
    {
        dialogueItem = true;
    }

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