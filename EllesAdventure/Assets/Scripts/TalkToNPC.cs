using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Makes NPC say something.
/// </summary>
public class TalkToNPC : MonoBehaviour, IIntertactable
{
    #region Fields

    public Collider ThisCollider { get; private set; }
    public bool Intertactable 
    {
        get
        {
            return intertactable;
        }
    }

    private bool intertactable = true;

    [SerializeField] private string[] startDialiogueChain;
    [SerializeField] private string[] endDialiogueChain;

    public bool dialogue1 = true;
    public bool dialogue2 = false;

    [SerializeField] private Transform player;

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
    public void Interact()
    {
        transform.rotation = Quaternion.LookRotation(player.position - transform.position);
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