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
    [SerializeField] private Pickup.ItemDetails[] buildingMaterial;

    private Pickup.ItemDetails[] materialsRequired;

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
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = GetComponent<Collider>();

        materialsRequired = (Pickup.ItemDetails[])buildingMaterial.Clone();
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
        materialsRequired = (Pickup.ItemDetails[])buildingMaterial.Clone();

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
    #endregion

    #region IInteractable
    public string InteractAminationString { get { return interactAminationString; } }

    public bool Intertactable { get { return interactable; } }


    public void Interact(string item)
    {
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
            MakeBuilding();
            return;
        }
        if (UseItem(item))
        {

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