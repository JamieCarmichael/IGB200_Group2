using UnityEngine;
using TMPro;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>
public class BuildPosition : MonoBehaviour, IIntertactable
{
    #region Fields
    [Tooltip("The position that the building will be build at. Generally this object but may very if the building is being built off centre from this object.")]
    [SerializeField] Transform buildingPosition;
    [Tooltip("The game object representing the mesh of the area to be built on. This will be disabled when the building is built.")]
    [SerializeField] GameObject buildingMesh;
    [Tooltip("The Recipe for the building in this area.")]
    [SerializeField] SOBuildingRecipe buildingRecipe;
    [Tooltip("The string for the animation to interact with this object.")]
    [SerializeField] private string interactAminationString;
    [Tooltip("Any text fields that are displaying the required items.")]
    [SerializeField] private TextMeshPro[] textFields;

    private UsableItems.Item requiredItem = UsableItems.Item.None;

    private UsableItems.Items[] buildingMaterial;

    private bool interactable = true;

    private Collider thisCollider;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = GetComponent<Collider>();
        buildingMaterial = (UsableItems.Items[])buildingRecipe.BuildingMaterial.Clone();
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
    private bool UseItem(UsableItems.Item item)
    {
        int index = GetItemIndex(item);
        if (index == -1) 
        {
            return false;
        }
        if (buildingMaterial[index].numberOfItems <= 0 )
        {
            return false;
        }

        buildingMaterial[index].numberOfItems--;
        PlayerManager.Instance.PlayerInteract.RemoveHeldObject();
        return true;
    }

    /// <summary>
    /// Retrieve the index of an item within the building materials array.
    /// If the item is not in the array returns -1.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private int GetItemIndex(UsableItems.Item item)
    {
        for (int i = 0; i < buildingMaterial.Length; i++)
        {
            if (buildingMaterial[i].thisItem == item)
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
        for (int i = 0; i < buildingMaterial.Length; i++)
        {
            if (buildingMaterial[i].numberOfItems > 0)
            {
                return false;
            }
        }
        return true;
    }

    // Makes the building. Disables this script and any no longer necisary objects.
    private void MakeBuilding()
    {
        Instantiate(buildingRecipe.BuildingPrefab, buildingPosition.position, buildingPosition.rotation, this.transform);
        buildingMesh.SetActive(false);
        thisCollider.enabled = false;
        interactable = false;
        this.enabled = false;
    }

    /// <summary>
    /// Update the text showing what matyerials are required.
    /// </summary>
    private void DisplayMaterialRequired()
    {
        string text = "";
        for (int i = 0; i < buildingMaterial.Length; i++)
        {
            text += buildingMaterial[i].thisItem.ToString() + ": " + buildingMaterial[i].numberOfItems + "\n";
        }

        for (int i = 0;i < textFields.Length;i++)
        {
            textFields[i].text = text;
        }
    }
    #endregion

    #region IInteractable
    public string InteractAminationString { get { return interactAminationString; } }

    public UsableItems.Item RequiredItem { get { return requiredItem; } }

    public bool Intertactable { get { return interactable; } }


    public void Interact(UsableItems.Item item)
    {
        if (!interactable)
        {
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