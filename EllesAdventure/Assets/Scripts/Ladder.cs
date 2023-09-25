using UnityEditor;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A ladder object that the player can climb on.
/// </summary>
public class Ladder : MonoBehaviour, IIntertactable
{
    #region Fields
    [Tooltip("The position that the player goes to to start climbing the laddder.")]
    [SerializeField] private Transform climbOnPosition;

    /// <summary>
    /// The collider for this object.
    /// </summary>
    private Collider thisCollider;
    /// <summary>
    /// The collider for this object.
    /// </summary>
    public Collider ThisCollider { get { return thisCollider; } } 
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
        thisCollider = GetComponent<Collider>();

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
        PlayerManager.Instance.ClimbLadder(this);
        StopLookAt();
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
        return climbOnPosition.position;
    }
    #endregion
}