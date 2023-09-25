using UnityEngine;
using UnityEngine.Events;

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
    [Tooltip("The is how long the gate takes to open. After this time the after open event runs.")]
    [SerializeField] private float openTime = 1.0f;
    [Tooltip("This event is called after the gate has finished opening.")]
    [SerializeField] private UnityEvent afterOpenEvent;


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
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = gameObject.GetComponent<Collider>();

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

    #region Class Methods
    /// <summary>
    /// Open this gate.
    /// </summary>
    public void OpenGate()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Open");
        Invoke("AfterOpenGate", openTime);
        this.enabled = false;
    }

    /// <summary>
    /// This is invoked after the gate has finished opening.
    /// </summary>
    private void AfterOpenGate()
    {
        afterOpenEvent.Invoke();
    }
    #endregion

    #region IIntertactable
    public void Interact(string item)
    {
        OpenGate();
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