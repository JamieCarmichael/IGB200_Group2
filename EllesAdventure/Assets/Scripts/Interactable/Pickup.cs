using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Allows an object to be pciked up by the player when they interact with it.
/// </summary>
public class Pickup : MonoBehaviour, IIntertactable
{
    #region Fields

    private Vector3 startPos;

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
            if (PlayerManager.Instance.PlayerInteract.CanUseItemType(itemIdentifier))
            {
                return intertactable;
            }
            else
            {
                return false;
            }

        }
    }
    private bool intertactable = true;
        
    private Collider thisCollider;

    private bool isHeld = false;

    public bool IsHeld { get { return isHeld; } }

    [SerializeField] private string itemName;

    public string Item { get { return itemName; } }

    [SerializeField] private string itemIdentifier = "default";

    public string ItemIdentifier { get { return itemIdentifier; } }

    [SerializeField] private Effect putDownEffect;

    [Header("Text Prompt")]
    [Tooltip("The transform of the object that the text prompt will apear over.")]
    [SerializeField] private Transform proptLocation;
    [Tooltip("The text displayed in the text prompt.")]
    [SerializeField] private string proptText;
    [Tooltip("If true this object will have a highlight that activates when the objects is selected.")]
    [SerializeField] private bool highlight = false;

    // Object renderers to have highlight applied.
    private Renderer[] renderers;
    private List<Material> highlightMaterials;

    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = GetComponent<Collider>();
        startPos = transform.position;

        if (highlight)
        {
            renderers = GetComponentsInChildren<Renderer>();
            highlightMaterials = new List<Material>();
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].materials.Length >= 2)
                {
                    highlightMaterials.Add(renderers[i].materials[1]);
                }
            }
        }
    }
    #endregion

    #region Public Methods
    public void PutDown(Vector3 newPos)
    {
        isHeld = false;
        transform.parent = null;
        transform.position = newPos;
        
        if (putDownEffect != null)
        {
            putDownEffect.PlayEffect();  
        }
    }

    public void Use()
    {
        gameObject.SetActive(false);
    }

    public void ReturnToStart()
    {
        isHeld = false;
        transform.position = startPos;
        gameObject.SetActive(true);
    }
    #endregion

    #region Private Methods

    #endregion

    #region IIntertactable
    public void Interact(string item)
    {
        if (PlayerManager.Instance.PlayerInteract.HeldObject != null)
        {
            return;
        }

        isHeld = true;
        transform.parent = PlayerManager.Instance.ItemCarryTransform;
        transform.position = transform.parent.position;

        StopLookAt();
    }

    public void LookAt()
    {
        if (!isHeld)
        {
            UIManager.Instance.TextPrompt.DisplayPrompt(proptLocation.position, proptText);

            if (highlight)
            {
                for (int i = 0; i < highlightMaterials.Count; i++)
                {
                    highlightMaterials[i].SetFloat("_alpha", 1);
                }
            }
        }
        else
        {
            StopLookAt();
        }
    }

    public void StopLookAt()
    {
        UIManager.Instance.TextPrompt.HidePrompt();
        if (highlight)
        {
            for (int i = 0; i < highlightMaterials.Count; i++)
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