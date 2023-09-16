using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: This script is used to hold some variables that are used to identify items.
///             Structs.
/// </summary>
public class UsableItems 
{
    [Serializable]
    public struct Items
    {
        public string itemName;
        public int numberOfItems;
        public Sprite itemImage;
    }
}