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


    [SerializeField] private string requiredItem;
    public string RequiredItem { get { return requiredItem; } }
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = gameObject.GetComponent<Collider>();
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
    public void Interact()
    {
        OpenGate();
    }

    public void StartLookAt()
    {
        //lookAtObject.SetActive(true);
    }

    public void StopLookAt()
    {
        //lookAtObject.SetActive(false);
    }

    public Vector3 GetClosestPoint(Vector3 playerPos)
    {
        Vector3 closestPoint = thisCollider.ClosestPoint(playerPos);
        closestPoint.y = transform.position.y;
        return closestPoint;
    }
    #endregion
}