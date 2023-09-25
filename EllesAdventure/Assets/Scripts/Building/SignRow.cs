using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A row on a sign for a building
/// </summary>

public class SignRow : MonoBehaviour
{
    #region Fields
    
    [SerializeField] private TextMeshPro itemName;

    [SerializeField] private SpriteRenderer itemImage;

    [SerializeField] private TextMeshPro itemCount;

    [SerializeField] private SpriteRenderer tickImage;

    [SerializeField] private Vector2 spriteSize = new Vector2(0.2f, 0.2f);

    #endregion

    #region Public Methods
    /// <summary>
    /// Fills the row with the item details.
    /// </summary>
    /// <param name="item"></param>
    public void FillRow(Pickup.ItemDetails item)
    {
        itemName.text = item.itemName.ToString();
        itemImage.enabled = true;
        itemImage.sprite = item.itemImage;

        // Set the scale of the object so that all sprites are the same size.
        itemImage.transform.localScale = Vector2.one / item.itemImage.bounds.size *  spriteSize;

        if (item.numberOfItems <= 0)
        {
            tickImage.enabled = true;
            itemCount.enabled = false;
        }
        else
        {
            tickImage.enabled = false;
            itemCount.enabled = true;
            itemCount.text = item.numberOfItems.ToString();
        }

        gameObject.SetActive(true);
    }

    public void DisplayText(string textToDisplay)
    {
        itemName.text = textToDisplay;
        itemImage.enabled = false;
        tickImage.enabled = false;
        itemCount.enabled = false;
    }
    #endregion
}