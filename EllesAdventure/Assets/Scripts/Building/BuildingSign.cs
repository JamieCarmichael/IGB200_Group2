using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A sign for a building.
/// </summary>
public class BuildingSign : MonoBehaviour 
{
    [Serializable]
    public struct ItemDetails
    {
        public string itemName;
        public int numberOfItems;
        public Sprite itemImage;
    }

    #region Fields
    [SerializeField] private string buildText = "E to build";

    [SerializeField] private TextMeshPro[] buildingNameTextFields;
    [SerializeField] private SignRow[] frontSignRows;
    [SerializeField] private SignRow[] backSignRows;
    #endregion

    #region Public Methods
    /// <summary>
    /// Display the materials needed on this sign.
    /// </summary>
    /// <param name="materials"></param>
    /// <param name="buildingName"></param>
    public void DisplayMaterialRequired(ItemDetails[] materials, string buildingName)
    {
        for (int i = 0; i < buildingNameTextFields.Length; i++)
        {
            buildingNameTextFields[i].text = buildingName;
        }

        // If all materials are dilivered
        if (AllMaterialsDelivered(materials))
        {
            for (int i = 0; i < frontSignRows.Length; i++)
            {
                if (i == 0)
                {
                    frontSignRows[i].DisplayText(buildText);
                }
                else
                {
                    frontSignRows[i].gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < backSignRows.Length; i++)
            {
                if (i == 0)
                {
                    backSignRows[i].DisplayText(buildText);
                }
                else
                {
                    backSignRows[i].gameObject.SetActive(false);
                }
            }
            return;
        }

        // Still needs materials
        for (int i = 0; i < frontSignRows.Length; i++)
        {
            if (i < materials.Length)
            {
                frontSignRows[i].FillRow(materials[i]);
            }
            else
            {
                frontSignRows[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < backSignRows.Length; i++)
        {
            if (i < materials.Length)
            {
                backSignRows[i].FillRow(materials[i]);
            }
            else
            {
                backSignRows[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region Private Methods
    private bool AllMaterialsDelivered(ItemDetails[] materials)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].numberOfItems > 0)
            {
                return false;
            }
        }
        return true;
    }
    #endregion
}