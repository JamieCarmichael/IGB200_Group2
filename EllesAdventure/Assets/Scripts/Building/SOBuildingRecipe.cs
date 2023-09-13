using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A list of building materials required to make a building.
/// </summary>


[CreateAssetMenu(fileName = "NewSOBuildingRecipe", menuName = "ScriptableObjects/SOBuildingRecipe")]
public class SOBuildingRecipe : ScriptableObject 
{
    [Tooltip("The prefab for the building being made.")]
    public GameObject BuildingPrefab;
    [Tooltip("What materials are required to make the building.")]
    public UsableItems.Items[] BuildingMaterial;
}