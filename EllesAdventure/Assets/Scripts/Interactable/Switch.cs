using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A switch that interacts with another object.
/// </summary>
public class Switch : MonoBehaviour, IIntertactable
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

    /// <summary>
    /// If true this object is interaclable.
    /// </summary>
    private bool intertactable = true;

    [Tooltip("The object that indicates when this object is being looked at.")]
    private GameObject lookAtObject;

    [Tooltip("How long the lever takes to opperate.")]
    [SerializeField] private float openTime = 1.0f;
    [Tooltip("The event called after the lever had been used.")]
    [SerializeField] private UnityEvent afterLeverEvent;

    [SerializeField] bool isOpen = true;

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

    private bool isMoving = false;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = gameObject.GetComponent<Collider>();
        animator = GetComponent<Animator>();

        animator.SetBool("Open", isOpen);

        if (highlight)
        {
            renderers = GetComponentsInChildren<Renderer>();
            highlightMaterials = new Material[renderers.Length];
            for (int i = 0; i < highlightMaterials.Length; i++)
            {
                highlightMaterials[i] = renderers[i].materials[1];
            }
        }
    }
    #endregion

    #region IIntertactable
    public void Interact(string item)
    {
        if (isMoving)
        {
            return;
        }
        isOpen = !isOpen;
        animator.SetBool("Open", isOpen);
        Invoke("AfterLever", openTime);
        isMoving = true;
    }

    private void AfterLever()
    {
        isMoving = false;
        afterLeverEvent.Invoke();
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