using TMPro;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A sign for a building.
/// </summary>
public class BuildingSign : MonoBehaviour 
{
    #region Fields
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
    public void DisplayMaterialRequired(UsableItems.Items[] materials, string buildingName)
    {
        for (int i = 0; i < buildingNameTextFields.Length; i++)
        {
            buildingNameTextFields[i].text = buildingName;
        }

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
}