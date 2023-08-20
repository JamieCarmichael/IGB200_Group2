using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Makes NPC say something.
/// </summary>
public class TalkToNPC : MonoBehaviour, IIntertactable
{
    #region Fields
    [Tooltip("The dialogue said in the first interaction.")]
    [SerializeField] private DialogueManager.Dialogue startDialiogueChain;
    [Tooltip("The dialogue said in the second interaction.")]
    [SerializeField] private DialogueManager.Dialogue endDialiogueChain;

    public bool dialogue1 = true;
    public bool dialogue2 = false;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        ThisCollider = gameObject.GetComponent<Collider>();
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
        if (dialogue1)
        {
            DialogueManager.Instance.DisplayDialogue(startDialiogueChain);
            dialogue1 = false;
        }
        else if (dialogue2)
        {
            DialogueManager.Instance.DisplayDialogue(endDialiogueChain);
            dialogue2 = false;
        }
    }
    #endregion

    #region IIntertactable
    public Collider ThisCollider { get; private set; }
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
    #endregion
}