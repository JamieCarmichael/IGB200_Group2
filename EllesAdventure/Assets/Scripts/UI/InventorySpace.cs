using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: The UI object showing an inventory object.
/// </summary>
public class InventorySpace : MonoBehaviour, IPointerEnterHandler
{
    #region Fields
    /// <summary>
    /// The inventory item on being shown on this object.
    /// </summary>
    private SOInventoryItem thisItem;
    /// <summary>
    /// The image displaying the inventory item.
    /// </summary>
    private Image image;
    /// <summary>
    /// The text field displaying the number of the item in the inventory.
    /// </summary>
    private TextMeshProUGUI itemCount;
    /// <summary>
    /// The image displaying the inventory item.
    /// </summary>
    private Image thisImage
    {
        get
        {
            if(image == null)
            {
                image = GetComponent<Image>();
            }
            return image;
        }
        set { image = value; }
    }
    /// <summary>
    /// The text field displaying the number of the item in the inventory.
    /// </summary>
    private TextMeshProUGUI thisItemCount
    {
        get
        {
            if (itemCount == null)
            {
                itemCount = GetComponentInChildren<TextMeshProUGUI>();
            }
            return itemCount;
        }
        set {  itemCount = value; }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Set the item being showed by this object and enable it.
    /// </summary>
    /// <param name="item"></param>
    public void SetInventory(InventoryObject item)
    {
        thisItem = item.item;

        thisImage.sprite = item.item.itemSprite;
        thisItemCount.text = item.count.ToString();

        gameObject.SetActive(true);
    }

    /// <summary>
    /// The name of this item.
    /// </summary>
    public string Name
    {
        get { return thisItem.itemName; }
    }

    /// <summary>
    /// The description of this item.
    /// </summary>
    public string Description
    {
        get { return thisItem.itemDescription; }
    }

    /// <summary>
    /// Displays the description when the mouse is over this item.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Notepad.Instance.SetInventoryDiscription(thisItem);
    }
    #endregion
}