using System;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: This script is used to hold some variables that are used to identify items.
///             Structs and enums.
/// </summary>
public class UsableItems 
{
    public enum Item
    {
        None,
        Notepad,
        WoodPlank,
        MetalPipe,
        Screws,
        Nails,
        Tool,
        ScaffoldingBrackets,
        Can,
        Bottle,
        Key1,
        Key2
    }

    [Serializable]
    public struct Items
    {
        public Item thisItem;
        public int numberOfItems;
    }
}