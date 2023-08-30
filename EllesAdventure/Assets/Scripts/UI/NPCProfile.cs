using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Information about an NPC.
/// </summary>

[Serializable]
public class NPCProfile
{ 
    public Sprite npcImage;
    public string name;
    public string occupation;
    public string info;
    [TextArea]
    public string bio;
}