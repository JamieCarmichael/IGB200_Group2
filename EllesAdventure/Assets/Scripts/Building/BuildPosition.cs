using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A building postion where things can be made.
/// </summary>
public class BuildPosition : MonoBehaviour, IIntertactable
{
    #region Fields
    [SerializeField] private string buildingName;

    [SerializeField] private string buildingType;

    [Tooltip("What materials are required to make the building.")]
    [SerializeField] private BuildingSign.ItemDetails[] buildingMaterial;

    private BuildingSign.ItemDetails[] materialsRequired;

    [Tooltip("Object being built.")]
    [SerializeField] private GameObject objectBuilt;


    [Tooltip("The game object for the area when the building needs to be built.")]
    [SerializeField] GameObject buildAreaObject;

    [Tooltip("The string for the animation to interact with this object.")]
    [SerializeField] private string interactAminationString;

    [SerializeField] private BuildingSign sign;

    private bool interactable = true;

    private bool isBuilt = false;

    private Collider thisCollider;

    private List<Pickup> itemsUsed = new List<Pickup>();


    [TextArea]
    [Tooltip("The dialoge when the trigger is activated")]
    [SerializeField] private string[] cantBuildDialoge = new string[1] { "You can build this yet!" };


    [Header("Effects")]
    [SerializeField] private Effect onBuildEffect;
    [SerializeField] private Effect onDestroyEffect;
    [SerializeField] private Effect onAddMaterialEffect;

    [Header("Text Prompt")]
    [Tooltip("If true prompts are shown.")]
    [SerializeField] private bool showPrompt = false;
    [Tooltip("The transform of the object that the text prompt will apear over.")]
    [SerializeField] private Transform proptLocation;
    //[Tooltip("The text displayed in the text prompt when the building is ready to be built.")]
    //[SerializeField] private string buildProptText;
    [Tooltip("The text displayed in the text prompt when the building can be destroyed.")]
    [SerializeField] private string destroyProptText;
    //[Tooltip("The text displayed in the text prompt when materials can be added.")]
    //[SerializeField] private string addProptText;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = GetComponent<Collider>();

        materialsRequired = (BuildingSign.ItemDetails[])buildingMaterial.Clone();
        DisplayMaterialRequired();
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    /// <summary>
    /// Try to use an item. The item should be the players current held item. 
    /// Will return true if the item is used or false if it is not used.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool UseItem(string item)
    {
        int index = GetItemIndex(item);
        if (index == -1) 
        {
            return false;
        }
        if (materialsRequired[index].numberOfItems <= 0 )
        {
            return false;
        }

        materialsRequired[index].numberOfItems--;
        itemsUsed.Add(PlayerManager.Instance.PlayerInteract.HeldObject);
        PlayerManager.Instance.PlayerInteract.RemoveHeldObject();
        return true;
    }

    /// <summary>
    /// Retrieve the index of an item within the building materials array.
    /// If the item is not in the array returns -1.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private int GetItemIndex(string item)
    {
        for (int i = 0; i < materialsRequired.Length; i++)
        {
            if (materialsRequired[i].itemName == item)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Checks if all of the building materials needed to make the buiding are present.
    /// Returns true if the buiding can be made.
    /// </summary>
    /// <returns></returns>
    private bool CheckIfBuildingHasAllMaterials()
    {
        for (int i = 0; i < materialsRequired.Length; i++)
        {
            if (materialsRequired[i].numberOfItems > 0)
            {
                return false;
            }
        }
        return true;
    }

    // Makes the building. Disables this script and any no longer necisary objects.
    private void MakeBuilding()
    {
        objectBuilt.SetActive(true);
        buildAreaObject.SetActive(false);
        isBuilt = true;
    }

    // Destroy the building and return the materials.
    private void UnMakeBuilding()
    {
        objectBuilt.SetActive(false);
        buildAreaObject.SetActive(true);
        isBuilt = false;
        materialsRequired = (BuildingSign.ItemDetails[])buildingMaterial.Clone();

        // Only make effect if items have been added.
        if (itemsUsed.Count > 0 )
        {
            if (onDestroyEffect != null)
            {
                onDestroyEffect.PlayEffect();
            }
        }

        foreach (Pickup item in itemsUsed)
        {
            item.ReturnToStart();
        }
        itemsUsed = new List<Pickup>();
        DisplayMaterialRequired();
    }

    /// <summary>
    /// Update the text showing what matyerials are required.
    /// </summary>
    private void DisplayMaterialRequired()
    {
        string text = "";
        for (int i = 0; i < materialsRequired.Length; i++)
        {
            text += materialsRequired[i].itemName.ToString() + ": " + materialsRequired[i].numberOfItems + "\n";
        }

        sign.DisplayMaterialRequired(materialsRequired, buildingName);
    }

    /// <summary>
    /// Returns true if player is on this object.
    /// </summary>
    /// <returns></returns>
    private bool CheckIfPlayerOnTop()
    {
        Vector3 playerPos = PlayerManager.Instance.PlayerMovement.transform.position;

        Bounds b = thisCollider.bounds;

        Vector3 BL = b.min;
        Vector3 TR = b.max;

        // Player is standing at a higher level than the building.
        if (playerPos.y > TR.y)
        {
            return true;
        }
        if (playerPos.x < BL.x || playerPos.x > TR.x || playerPos.z < BL.z || playerPos.z > TR.z)
        {
            return false;
        }

        return true;
    }
    #endregion

    #region IInteractable
    public string InteractAminationString { get { return interactAminationString; } }

    public bool Intertactable 
    { 
        get 
        {
            if(CheckIfPlayerOnTop())
            {
                return false;
            }

            return interactable; 
        } 
    }


    public void Interact(string item)
    {
        if (CheckIfPlayerOnTop())
        {
            return;
        }

        StopLookAt();

        if (!interactable)
        {
            return;
        }
        // Cant make this building.
        if (!PlayerManager.Instance.PlayerInteract.CanMakeBuidlingType(buildingType))
        {
            DialogueManager.Instance.DisplayDialogue(cantBuildDialoge);
            return;
        }
        if (isBuilt)
        {
            UnMakeBuilding();
            return;
        }

        if (CheckIfBuildingHasAllMaterials())
        {
            if (onBuildEffect != null)
            {
                onBuildEffect.PlayEffect();
            }
            MakeBuilding();
            return;
        }
        if (UseItem(item))
        {
            if (onAddMaterialEffect != null)
            {
                onAddMaterialEffect.PlayEffect();
            }

            // Update visual
            DisplayMaterialRequired();
            return;
        }
        // No item in hand. Return materials.
        if (item == "")
        {
            UnMakeBuilding();
            return;
        }

        // Nothing happened.
    }
    public void LookAt()
    {
        if (showPrompt)
        {
            if (CheckIfPlayerOnTop())
            {
                StopLookAt();
                return;
            }

            if (isBuilt)
            {
                UIManager.Instance.TextPrompt.DisplayPrompt(proptLocation.position, destroyProptText);
                return;
            }

            //if (CheckIfBuildingHasAllMaterials())
            //{
            //    UIManager.Instance.TextPrompt.DisplayPrompt(proptLocation.position, buildProptText);
            //    return;
            //}
            //if (PlayerManager.Instance.PlayerInteract.HeldItem != "")
            //{
            //    UIManager.Instance.TextPrompt.DisplayPrompt(proptLocation.position, addProptText);
            //    return;
            //}

            // If some materials are delivered but not all materials.
            if (itemsUsed.Count > 0 && !CheckIfBuildingHasAllMaterials())
            {
                UIManager.Instance.TextPrompt.DisplayPrompt(proptLocation.position, destroyProptText);
                return;
            }


            StopLookAt();
        }
    }

    public void StopLookAt()
    {
        if (showPrompt)
        {
            UIManager.Instance.TextPrompt.HidePrompt();
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