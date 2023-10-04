using UnityEditor;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Gate object. Can Be opened and have an event run after opening.
/// </summary>
public class Gate : MonoBehaviour, IIntertactable 
{
    #region Fields
    private Collider thisCollider;

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

    [Tooltip("If true this object can be interacted with.")]
    [SerializeField] private bool intertactable = false;

    [SerializeField] bool isOpen = false;


    [SerializeField] private Effect onMoveEffect;

    [Header("Text Prompt")]
    [Tooltip("The transform of the object that the text prompt will apear over.")]
    [SerializeField] private Transform proptLocation;
    [Tooltip("The text displayed in the text prompt.")]
    [SerializeField] private string proptText;
    [Tooltip("If true this object will have a highlight that activates when the objects is selected.")]
    [SerializeField] private bool highlight = false;

    // Object renderers to have highlight applied.
    private Renderer[] renderers;
    private Material[] highlightMaterials;

    private Animator animator;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = gameObject.GetComponent<Collider>();
        animator = GetComponent<Animator>();

        if (highlight)
        {
            renderers = GetComponentsInChildren<Renderer>();
            highlightMaterials = new Material[renderers.Length];
            for (int i = 0; i < highlightMaterials.Length; i++)
            {
                highlightMaterials[i] = renderers[i].materials[1];
            }
        }

        if (isOpen)
        {
            OpenGate(false);
        }
        else
        {
            CloseGate(false);
        }
    }
    #endregion

    #region Class Methods
    /// <summary>
    /// Open this gate.
    /// </summary>
    public void OpenGate()
    {
        if (onMoveEffect != null)
        {
            onMoveEffect.PlayEffect();
        }
        animator.SetBool("Open", true);
        isOpen = true;
    }
    public void OpenGate(bool playEffect)
    {
        if (playEffect && onMoveEffect != null)
        {
            onMoveEffect.PlayEffect();
        }
        animator.SetBool("Open", true);
        isOpen = true;
    }
    public void CloseGate()
    {
        if (onMoveEffect != null)
        {
            onMoveEffect.PlayEffect();
        }
        animator.SetBool("Open", false);
        isOpen = false;
    }
    public void CloseGate(bool playEffect)
    {
        if (playEffect && onMoveEffect != null)
        {
            onMoveEffect.PlayEffect();
        }
        animator.SetBool("Open", false);
        isOpen = false;
    }
    public void ToggleOpen()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            OpenGate();
        }
        else
        {
            CloseGate();
        }
    }
    #endregion

    #region IIntertactable
    public void Interact(string item)
    {
        ToggleOpen();
    }

    public void LookAt()
    {
        UIManager.Instance.TextPrompt.DisplayPrompt(proptLocation.position, proptText);

        if (highlight)
        {
            for (int i = 0; i < highlightMaterials.Length; i++)
            {
                highlightMaterials[i].SetFloat("_alpha", 1);
            }
        }
    }

    public void StopLookAt()
    {
        UIManager.Instance.TextPrompt.HidePrompt();
        if (highlight)
        {
            for (int i = 0; i < highlightMaterials.Length; i++)
            {
                highlightMaterials[i].SetFloat("_alpha", 0);
            }
        }
    }

    public Vector3 GetClosestPoint(Vector3 playerPos)
    {
        Vector3 closestPoint = thisCollider.ClosestPoint(playerPos);
        closestPoint.y = transform.position.y;
        return closestPoint;
    }
    #endregion
}